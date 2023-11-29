public static class ShowHandler
{
    public const string FileName = "shows.json";
    private static int LatestShowID;
    public static List<Show> Shows;

    static ShowHandler()
    {
        Shows = JSONMethods.ReadJSON<Show>(FileName).ToList();
        if (Shows.Count > 0)
            LatestShowID = Shows.MaxBy(sm => sm.Id)!.Id;
        else
            LatestShowID = 0;
    }

    public static void EditShowSchedule()
    {
        List<string> menuOptions = new() { "Add show", "View/Remove shows  ", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Show Schedule\n\nSelect an option:", menuOptions, true);

            switch (index)
            {
                case 0:
                    AddShow();
                    break;
                case 1:
                    RemoveShow();
                    break;
                case 2:
                case 3:
                    inMenu = false;
                    break;
                default:
                    break;
            }
        }
    }

    public static Show? GetShowById(int id)
    {
        foreach (var show in Shows)
            if (show.Id == id)
                return show;
        return null;
    }

    public static List<Show> GetShowsByDate(DateTime date)
    {
        List<Show> shows = new();
        foreach (var show in Shows)
        {
            if (show.DateAndTime.Date == date.Date)
                shows.Add(show);
        }
        shows.Sort((s1, s2) => s1.DateAndTime.CompareTo(s2.DateAndTime));

        return shows;
    }

    public static void AddShow()
    {
        bool inMenu = true;
        while (inMenu)
        {
            // Movie Selection
            List<string> movieTitles = MovieHandler.GetMovieTitles();
            if (movieTitles.Count == 0)
            {
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine("Show Schedule");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nCan't add showing as there are no movies available");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n\nPress any key to go back");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            string menuText = $"Show Schedule\n\nSelect a movie:\n" +
            $"  {"Title",-30} | {"Language",-10} | {"Genres",-30} | {"Runtime",-8} | Age\n" +
            $"  {new string('-', 93)}";
            int index = Menu.Start(menuText, movieTitles, true);
            if (index == movieTitles.Count)  // If user presses left arrow key leave current while loop
                break;
            Movie movie = MovieHandler.Movies[index];

            // Theater selection
            string menuString = "Show Schedule\n\nSelect a theater:";
            List<string> menuOptions = new() { "Small (150 seats)", "Medium (300 seats)", "Large (500 seats)" };
            index = Menu.Start(menuString, menuOptions, true);
            if (index == menuOptions.Count)
                break;
            string selectedTheater = menuOptions[index];
            int selectedTheaterNum = index + 1; 
            
            // Date selection
            List<string> dates = CreateDatesList(14);
            index = Menu.Start("Show Schedule\n\nSelect a date:", dates, true);
            if (index == dates.Count)  // If user presses left arrow key leave current while loop
                break;

            // Time selection
            string dateString = dates[index];
            DateTime selectedTime = TimeSelection(movie, dateString);

            Show show = new(movie, selectedTime, selectedTheaterNum) { Id = LatestShowID += 1 };

            // Confirm or cancel selection
            string menuHeader = $"Show Schedule\n\nMovie: {movie.Title}\nTheater: {selectedTheater}\nDate: {show.DateString}\nTime: {show.StartTimeString} - {show.EndTimeString}\n\nAdd this show to the schedule:";
            int selection = ConfirmSelection(show, movie, menuHeader, true);
            if (!(selection == 0))
            {
                break;
            }
            
            Shows.Add(show);
            JSONMethods.WriteToJSON(Shows, FileName);
            
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe show has been added");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            
            Console.ReadKey();
            break;
        }
    }
    public static List<string> GetMovieTitles(List<Movie> movies)
    {
        List<string> movieTitles = new();
        foreach (Movie movie in movies)
        movieTitles.Add(movie.Title);
        return movieTitles;
    }
    public static void RemoveShow()
    {
        List<string> dates = GetAllDates();
        dates.Add("Back");
        if (dates.Count <= 1)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\nThere are currently no shows scheduled");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\nPress any key to go back");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Show Schedule\n\nSelect a date to see the shows for that day:", dates, true);
            if (index == dates.Count || index == dates.Count - 1)
            {
                return;
            }

            string dateString = dates[index];
            DateTime selectedDate = DateTime.Parse(dateString);
            List<Show> ShowsForDate = GetShowsByDate(selectedDate);
            List<string> movieMenuStrings = CreateListMovieStrings(ShowsForDate);

            index = Menu.Start($"Show Schedule\n\nShows on {dateString}:", movieMenuStrings, true);
            if (index == movieMenuStrings.Count || index == movieMenuStrings.Count - 1)
            {
                continue;
            }

            Show show = ShowsForDate[index];
            Movie movie = MovieHandler.GetMovieById(show.MovieId)!;

            // Confirm or cancel selection
            string theaterSize = show.TheaterNumber == 1 ? "Small (150 seats)" : show.TheaterNumber == 2 ? "Medium (300 seats)" : "Large (500 seats)";
            string menuHeader = $"Show Schedule\n\nMovie: {movie.Title}\nTheater: {theaterSize}\nDate: {show.DateString}\nTime: {show.StartTimeString} - {show.EndTimeString}\n\nRemove this show from the schedule:";
            int selection = ConfirmSelection(show, movie, menuHeader, true);
            if (!(selection == 0))
            {
                break;
            }

            Shows.Remove(show);
            JSONMethods.WriteToJSON(Shows, FileName);

            // Confirmation message
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Show Schedule");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe show has been removed");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            Console.ReadKey();

            return;
        }

    }

    private static List<string> CreateDatesList(int count)
    {
        List<string> dates = new();
        DateTime date = DateTime.Now;
        for (int i = 0; i < count; i++)
        {
            dates.Add(date.ToString("dd-MM-yyyy"));
            date = date.AddDays(1);
        }
        return dates;
    }

    private static int ConfirmSelection(Show show, Movie movie, string menuHeader, bool isAdmin = false)
    {
        List<string> menuOptions = new() { "Confirm Selection", "Cancel" };
        return Menu.Start(menuHeader, menuOptions, isAdmin);
    }

    private static DateTime TimeSelection(Movie movieToAdd, string dateString)
    {
        DateTime resultDateTime = DateTime.Now;
        bool correctInput = false;
        while (!(correctInput))
        {
            Console.Clear();
            Console.CursorVisible = true;
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Show Schedule\n\nMovie: {movieToAdd.Title}\nDate: {dateString}");
            Console.Write("\nStart time of the movie (HH:mm): ");
            string? timeString = Console.ReadLine();
            Console.CursorVisible = false;

            string combinedDateTime = dateString + " " + timeString;
            if (DateTime.TryParseExact(combinedDateTime, "dd-MM-yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out resultDateTime))
            {
                correctInput = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIncorrect input, press any key to try again.");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        return resultDateTime;
    }

    public static Show? SelectShowFromSchedule(bool isAdmin = false)
    {
        bool inMenu = true;
        while (inMenu)
        {
            List<string> dates = GetDatesOfFutureShows();
            if (dates.Count == 0)
            {
                List<string> menuOption = new() { "Back" };
                Menu.Start("Show Schedule\n\nThere are no movies scheduled at the moment, please come back later", menuOption);

                return null;
            }

            // Date selection
            dates.Add("Back");

            int index;
            if (!isAdmin)
                index = Menu.Start("Show Schedule\n\nSelect a date:", dates);
            else
                index = Menu.Start("Show Schedule\n\nSelect a date:", dates, true);


            if (index == dates.Count || index == dates.Count - 1)
                return null;

            string dateString = dates[index];
            DateTime date = DateTime.Parse(dateString);

            List<Show> showsForDate = GetShowsByDate(date);

            // Create list of formatted strings to display to the user
            List<string> movieMenuString = CreateListMovieStrings(showsForDate);

            index = Menu.Start($"Show Schedule\n\nShows on {dateString}:", movieMenuString, isAdmin);
            if (index == movieMenuString.Count || index == movieMenuString.Count - 1)
                continue;

            Show show = showsForDate[index];
            // Confirm Selection
            if (!isAdmin)
            {
                
                Movie movie = MovieHandler.GetMovieById(show.MovieId)!;
                string confirmationMessage = $"Show Schedule\n\nMake a reservation for '{movie.Title}' on {show.DateString} at {show.StartTimeString}:";
                int selection = ConfirmSelection(show, movie, confirmationMessage);
                if (!(selection == 0))
                {
                    continue;
                }

                return show;
            }
            else if (isAdmin)
            {
                return show;
            }
        }
        return null;
    }

    public static List<string> GetDatesOfFutureShows()
    {
        List<DateTime> dates = new();
        foreach (var _show in ShowHandler.Shows)
        {
            DateTime date = _show.DateAndTime.Date;
            if (date.Date >= DateTime.Today && !dates.Contains(date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static List<string> CreateListMovieStrings(List<Show> shows)
    // Creates list of formatted strings: Start time - end time : Title
    {
        List<string> movieMenuStrings = new();
        foreach (var _show in shows)
        {
            Movie movie = MovieHandler.GetMovieById(_show.MovieId)!;
            movieMenuStrings.Add($"{_show.StartTimeString} - {_show.EndTimeString} {movie.Title}  ");
        }
        movieMenuStrings.Add("Back");
        return movieMenuStrings;
    }

    public static List<string> GetAllDates()
    {
        List<DateTime> dates = new();
        foreach (var _show in Shows)
        {
            DateTime date = _show.DateAndTime.Date;
            if (!dates.Contains(date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderBy(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static void PrintMovieDates(Movie movie, bool isAdmin = false)
    {
        Console.Clear();

        var shows = JSONMethods.ReadJSON<Show>(FileName);
        var showsFiltered = shows.Where(s => s.MovieId == movie.Id).OrderBy(s => s.DateAndTime);
        var showsFilteredGrouped = showsFiltered.GroupBy(s => s.DateString);
        DateTime date;

        if (isAdmin)
            DisplayAsciiArt.AdminHeader();
        else
            DisplayAsciiArt.Header();

        Console.WriteLine($"Showings of {movie.Title}\n");

        foreach (var day in showsFilteredGrouped)
        {
            date = day.First().DateAndTime;
            Console.WriteLine($"{date.DayOfWeek} {date:D}");
            foreach (var show in day)
            {
                Console.WriteLine($"  {show.StartTimeString} - Auditorium {show.TheaterNumber}");
            }
            Console.WriteLine();
        }

        if (showsFiltered.Count() == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{movie.Title} has not been scheduled yet");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to go back");
        Console.ResetColor();

        Console.ReadKey();
        MovieHandler.MovieSelectionMenu(movie, isAdmin);
    }
}