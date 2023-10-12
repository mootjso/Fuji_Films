public static class MovieScheduleHandlerUser
{
    private const string FileName = "movie_schedule.json";
    public static List<ScheduledMovie> Movies;

    static MovieScheduleHandlerUser()
    {
        Movies = JSONMethods.ReadJSON<ScheduledMovie>(FileName);
    }

    public static void Start()
    {
        bool inMenu = true;
        while (inMenu)
        {
            List<string>  dates = GetDatesInFuture();
            if (dates.Count == 0)
            {
                List<string> menuOption = new() { "Back" };
                Menu.Start("There are no movies scheduled at the moment, please come back later.\n", menuOption);

                return;
            }

            // Date selection
            dates.Add("Back");
            int index = Menu.Start("Select a date to see the movies for that day\n", dates);
            if (index == dates.Count || index == dates.Count - 1)
                break;

            string dateString = dates[index];
            DateTime selectedDate = DateTime.Parse(dateString);

            List<ScheduledMovie> moviesForDate = GetMoviesByDate(selectedDate);

            // Create list of formatted strings to display to the user
            List<string> movieMenuString = CreateListMovieStrings(moviesForDate);
            
            index = Menu.Start($"Date: {dateString}\n", movieMenuString);
            if (index == movieMenuString.Count || index == movieMenuString.Count - 1)
                continue;

            // TODO IMPLEMENT Ticket reservation system ------------------------------------------------
            //
            //
            //------------------------------------------------------------------------------------------

            // NOT IMPLEMENTED Message
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("\n\n    TICKET RESERVATIONS NOT IMPLEMENTED YET\n\n   PRESS ANY KEY TO GO BACK");
            Console.ReadKey();
        }
    }

    public static List<string> GetAllDates()
    {
        List<DateTime> dates = new();
        foreach (ScheduledMovie movie in Movies)
        {
            DateTime date = movie.StartTime.Date;
            if (!dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static List<string> GetDatesInFuture()
    {
        List<DateTime> dates = new();
        foreach (ScheduledMovie movie in Movies)
        {
            DateTime date = movie.StartTime.Date;
            if (date.Date >= DateTime.Today && !dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static List<ScheduledMovie> GetMoviesByDate(DateTime selectedDate)
    {
        List<ScheduledMovie> movies = new();
        foreach (ScheduledMovie movie in Movies)
        {
            if (movie.StartTime.Date == selectedDate)
                movies.Add(movie);
        }
        return movies;
    }

    public static List<string> CreateListMovieStrings(List<ScheduledMovie> movies)
        // Creates list of formatted strings: Start time - end time : Title
    {
        List<string> movieMenuStrings = new();
        foreach (var movie in movies)
        {
            movieMenuStrings.Add($"{movie.StartTime.TimeOfDay.ToString()[..5]} - {movie.EndTime.TimeOfDay.ToString()[..5]}: {movie.Movie.Title}  ");
        }
        movieMenuStrings.Add("Back");
        return movieMenuStrings;
    }
}