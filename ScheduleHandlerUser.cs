public static class ScheduleHandlerUser
{
    private const string FileName = "shows.json";
    public static List<Show> Movies;

    static ScheduleHandlerUser()
    {
        Movies = JSONMethods.ReadJSON<Show>(FileName);
    }

    public static Show? SelectMovieFromSchedule()
    {
        bool inMenu = true;
        while (inMenu)
        {
            List<string> dates = GetDatesInFuture();
            if (dates.Count == 0)
            {
                List<string> menuOption = new() { "Back" };
                Menu.Start("There are no movies scheduled at the moment, please come back later.\n", menuOption);

                return null;
            }

            // Date selection
            dates.Add("Back");
            int index = Menu.Start("Select a date to see the movies for that day\n", dates);
            if (index == dates.Count || index == dates.Count - 1)
                break;

            string dateString = dates[index];
            DateTime date = DateTime.Parse(dateString);

            List<Show> moviesForDate = ShowHandler.GetShowsByDate(date);

            // Create list of formatted strings to display to the user
            List<string> movieMenuString = CreateListMovieStrings(moviesForDate);
            
            index = Menu.Start($"Date: {dateString}\n", movieMenuString);
            if (index == movieMenuString.Count || index == movieMenuString.Count - 1)
                continue;
            
            return moviesForDate[index];
        }
        return null;
    }

    //
    public static List<string> GetAllDates()
    {
        List<DateTime> dates = new();
        foreach (Show movie in Movies)
        {
            DateTime date = movie.DateAndTime;
            if (!dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    //
    public static List<string> GetDatesInFuture()
    {
        List<DateTime> dates = new();
        foreach (var _show in ShowHandler.Shows)
        {
            DateTime date = _show.DateAndTime;
            if (date.Date >= DateTime.Today && !dates.Contains(date.Date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    //
    public static List<Show> GetMoviesByDate(DateTime selectedDate)
    {
        List<Show> movies = new();
        foreach (Show movie in Movies)
        {
            if (movie.DateAndTime == selectedDate)
                movies.Add(movie);
        }
        return movies;
    }

    //
    public static List<string> CreateListMovieStrings(List<Show> shows)
        // Creates list of formatted strings: Start time - end time : Title
    {
        List<string> movieMenuStrings = new();
        foreach (var _show in shows)
        {
            Movie movie = MovieHandler.GetMovieById(_show.MovieId)!;
            movieMenuStrings.Add($"{_show.StartTimeString} - {_show.EndTimeString}: {movie.Title}  ");
        }
        movieMenuStrings.Add("Back");
        return movieMenuStrings;
    }
}