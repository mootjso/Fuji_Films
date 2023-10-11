public static class MovieSchedule
{
    private const string FileName = "movie_schedule.json";
    // TODO Cannot read as ScheduledMovie, have to recast the Movie objects
    public static List<ScheduledMovie> Movies;

    static MovieSchedule()
    {
        Movies = JSONMethods.ReadJSON<ScheduledMovie>(FileName);
    }

    public static void Start()
    {
        bool inMenu = true;
        while (inMenu)
        {
            // No movies scheduled
            List<string> dates = new();
            if (Movies.Count == 0)
            {
                List<string> menuOption = new() { "Back" };
                Menu.Start("There are no movies scheduled at the moment, please come back later.\n", menuOption);
                return;
            }
            // Get a list of all the dates
            else
            {
                foreach (ScheduledMovie movie in Movies)
                {
                    string date = movie.StartTime.Date.ToString()[..10];
                    if (!dates.Contains(date))
                        dates.Add(date);
                }
            }

            dates.Add("Back");
            int index = Menu.Start("Select a date to see the movies for that day\n", dates);
            if (index == dates.Count || index == dates.Count - 1)
            {
                //inMenu = false;
                break;
            }
            string dateString = dates[index];
            DateTime selectedDate = DateTime.Parse(dateString);

            // Create a list for the movies scheduled for the selected date
            List<ScheduledMovie> moviesForSelectedDate = new();
            foreach (ScheduledMovie movie in Movies)
            {
                if (movie.StartTime.Date == selectedDate)
                    moviesForSelectedDate.Add(movie);
            }

            // Create list of formatted strings to display to the user
            List<string> movieMenuString = new();
            foreach (var movie in moviesForSelectedDate)
            {
                movieMenuString.Add($"{movie.StartTime.TimeOfDay.ToString()[..5]} - {movie.EndTime.TimeOfDay.ToString()[..5]}: {movie.Movie.Title}  ");
            }
            movieMenuString.Add("Back");
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
}