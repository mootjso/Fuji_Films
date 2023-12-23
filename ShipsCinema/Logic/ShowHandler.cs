using Microsoft.VisualBasic;
using System;
using System.Globalization;

public static class ShowHandler
{
    public const string FileName = "Datasources/shows.json";
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
        List<string> menuOptions = new() { "Add Showing", "View/Remove Showing", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Showing Schedule\n\nSelect an option:", menuOptions, true);

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

    private static List<Show> GetShowsByDate(string date)
    {
        List<Show> showings = JSONMethods.ReadJSON<Show>(FileName).ToList();
        return showings.Where(s => s.DateString == date).ToList();
    }

    public static void AddShow()
    {
        bool inMenu = true;
        while (inMenu)
        {
            // Movie Selection
            List<string> movieTitles = MovieHandler.GetMovieTitles(JSONMethods.ReadJSON<Movie>(MovieHandler.FileName).ToList());
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
            DateTime selectedTime = TimeSelection(movie, dateString, selectedTheaterNum);

            Show? show = new(movie, selectedTime, selectedTheaterNum) { Id = LatestShowID += 1 };

            // Check if Theater is available at the chosen time
            show = TheaterIsAvailable(show, dateString);
            if (show == null)
                return;

            // Confirm or cancel selection
            string menuHeader = $"Show Schedule\n\nMovie: {movie.Title}\nTheater: {selectedTheater}\nDate: {show.DateString}\nTime: {show.StartTimeString} - {show.EndTimeString}\n\nAdd this show to the schedule:";
            int selection = ConfirmSelection(menuHeader, true);
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

    // Returns the showing that is already planned at the time of the newShow, or returns null if time is available
    private static Show? TimeIsValid(Show newShow, Movie newMovie) 
    {
        DateTime startTimeNewShow = newShow.DateAndTime;
        DateTime endTimeNewShow = newShow.DateAndTime.AddMinutes(15 + newMovie.Runtime);

        foreach (Show oldShow in Shows)
        {
            if (oldShow.TheaterNumber == newShow.TheaterNumber)
            {
                Movie oldMovie = MovieHandler.GetMovieById(oldShow.MovieId)!;
                DateTime startTimeOldShow = oldShow.DateAndTime;
                DateTime endTimeOldShow = oldShow.DateAndTime.AddMinutes(15 + oldMovie.Runtime);
                DateTime earliestPossibleStart = startTimeOldShow.AddMinutes(-newMovie.Runtime - 15);

                if (startTimeNewShow < endTimeOldShow && endTimeNewShow > startTimeOldShow)
                    return oldShow;
            }
        }

        return null;
    }

    private static DateTime GetPossibleStartTime(Show? oldShow, Show newShow, Movie newMovie, bool earlierTime = true)
    {
        DateTime newStartTime;
        do
        {
            Movie oldMovie = MovieHandler.GetMovieById(oldShow!.MovieId)!;
            if (earlierTime)
                newStartTime = oldShow!.DateAndTime.AddMinutes(-newMovie.Runtime - 15);
            else  // Get later possible start time
                newStartTime = oldShow.DateAndTime.AddMinutes(15 + oldMovie.Runtime);
            
            newShow.DateAndTime = newStartTime;
            oldShow = TimeIsValid(newShow, newMovie);
        }
        while (oldShow != null);

        return newStartTime;
    }

    public static Show TheaterIsAvailable(Show newShow, string dateString)
    {
        Movie newMovie = MovieHandler.GetMovieById(newShow.MovieId)!;

        Show? oldShow = TimeIsValid(newShow, newMovie);
        if (oldShow == null) // No other showing planned at the new showing time
            return newShow;


        DateTime earlierPossibleStart = GetPossibleStartTime(oldShow, newShow, newMovie);
        DateTime laterPossibleStart = GetPossibleStartTime(oldShow, newShow, newMovie, false);

        // Get a string of the planned movies for that theater for that day

        List<Show> ShowsForDate = GetShowsByDate(newShow.DateAndTime);
        List<string> movieMenuStrings = CreateListMovieStrings(ShowsForDate, newShow.TheaterNumber);
        movieMenuStrings.RemoveAt(movieMenuStrings.Count - 1); // Removes the "Back" option that the CreaListMovieStrings method adds

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nThe theater is already booked at this time, the following showings are planned for this day:");
        Console.ForegroundColor= ConsoleColor.Blue;
        movieMenuStrings.ForEach(s => Console.WriteLine(s));
        Console.ResetColor();
        Console.WriteLine($"\n\n[1] Change to earlier start time: {earlierPossibleStart:HH:mm}\n[2] Change to later start time: {laterPossibleStart:HH:mm}\n[3] Enter a custom time");
        Console.ResetColor();
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.D1 ) // Admin chooses the earlier possible start time
        {
            newShow.DateAndTime = earlierPossibleStart;
            return newShow;
        }
        else if (keyInfo.Key == ConsoleKey.D2) // Admin chooses the later possible start time
        {
            newShow.DateAndTime = laterPossibleStart;
            return newShow;
        }
        else // Admin wants to enter a custom time
        {
            DateTime customTime;
            do
            {
                customTime = TimeSelection(newMovie, dateString, newShow.TheaterNumber);
                if (customTime > earlierPossibleStart && customTime < laterPossibleStart)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nEnter a time before '{earlierPossibleStart:HH:mm}' or after '{laterPossibleStart:HH:mm}'");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nPress any key to try again");
                    Console.ReadKey();
                    Console.ResetColor();
                }
            }
            while (customTime > earlierPossibleStart && customTime < laterPossibleStart);
            newShow.DateAndTime = customTime;
        }

        // POSSIBLE FEATURE: Check if the time is available at a different theater
        return newShow;
    }

    private static bool ChooseShowingToRemove(string date)
    {
        Func<Show, bool> func = s => RemoveShowingFromJson(s);
        List<Show> showings = GetShowsByDate(date).OrderBy(s => s.DateAndTime).ToList();
        Func<Show, string> formatter = s => $"{s.StartTimeString} - {s.EndTimeString} | Theater {s.TheaterNumber} | {MovieHandler.GetMovieById(s.MovieId)}";
        Menu.MenuPagination(showings.Select(s => formatter(s)).ToList(), $"Showing Schedule\n\nShowings on {date}", "", func, showings, true);
        return true;
    }

    private static bool RemoveShowingFromJson(Show showing)
    {
        List<Show> showings = JSONMethods.ReadJSON<Show>(FileName).ToList();
        ConsoleKey choice;

        while (true)
        {
            Console.WriteLine($"Are you sure you want to delete this showing? (Y/N)");
            choice = Console.ReadKey().Key;
            switch (choice)
            {
                case ConsoleKey.Y:
                    showings = showings.Where(s => s.DateAndTime != showing.DateAndTime).ToList();
                    JSONMethods.WriteToJSON(showings, FileName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Showing has been removed");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return true;
                case ConsoleKey.N:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Removing of showing aborted");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return false;
            }
        }
    }

    public static void RemoveShow()
    {
        string messageWhenEmpty = "Showing Schedule\n\nThere are currently no shows scheduled";
        string menuText = $"Showing Schedule\n\nSelect a date to see showings for that day:";

        List<string> menuOptionsFull;
        menuOptionsFull = GetAllDates();
        Func<string, bool> func = ChooseShowingToRemove;
        Menu.MenuPagination(menuOptionsFull, menuText, messageWhenEmpty, func, isAdmin: true);
       

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

    private static int ConfirmSelection(string menuHeader, bool isAdmin = false)
    {
        List<string> menuOptions = new() { "Confirm Selection", "Cancel" };
        return Menu.Start(menuHeader, menuOptions, isAdmin);
    }

    private static DateTime TimeSelection(Movie movieToAdd, string dateString, int theaterNumber = 0)
    {
        string selectedTheater = theaterNumber == 1 ? "Small (150 seats)" : theaterNumber == 2 ? "Medium (300 seats)" : "Large (500 seats)";
        DateTime resultDateTime = DateTime.Now;
        bool correctInput = false;
        while (!(correctInput))
        {
            Console.Clear();
            Console.CursorVisible = true;
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Show Schedule\n\nMovie: {movieToAdd.Title}\nTheater: {selectedTheater}\nDate: {dateString}");
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

    public static List<string> CreateListMovieStrings(List<Show> shows, int theaterNumber = -1)
    // Creates list of formatted strings: Start time - end time Theater Title
    {
        List<string> movieMenuStrings = new();
        List<Show> validShows = new();
        foreach (var show in shows)
        {
            validShows.Add(show);
        }
        validShows = validShows.OrderBy(s => s.DateAndTime).ToList(); // Intermediary list to order by theater number if that's wanted
        foreach (var show in validShows)
        {
            Movie movie = MovieHandler.GetMovieById(show.MovieId)!;
            if (show.TheaterNumber == theaterNumber || theaterNumber == -1)
                movieMenuStrings.Add($"{show.StartTimeString} - {show.EndTimeString} | Theater {show.TheaterNumber} | {movie.Title}  ");
        }
        movieMenuStrings.Add("Back");
        return movieMenuStrings;
    }

    public static List<string> GetAllDates()
    {
        Shows = JSONMethods.ReadJSON<Show>(FileName).ToList();
        List<DateTime> dates = new();
        foreach (var _show in Shows)
        {
            DateTime date = _show.DateAndTime.Date;
            if (!dates.Contains(date))
                dates.Add(date);
        }
        List<DateTime> sortedDates = dates.OrderByDescending(d => d).ToList();
        List<string> sortedDateStrings = sortedDates.Select(d => d.ToString("dd-MM-yyyy")).ToList();
        return sortedDateStrings;
    }

    public static void PrintMovieDates(Movie movie, bool isAdmin = false)
    {
        CultureInfo englishCulture = new CultureInfo("en-US");
        Console.Clear();

        var shows = JSONMethods.ReadJSON<Show>(FileName).Where(s => s.DateAndTime >= DateTime.Now);
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
            Console.WriteLine($"{date.DayOfWeek} {date.Day} {date.ToString("MMMM", englishCulture)} {date:yyyy}");

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

    public static List<Movie> GetScheduledMovies()
    {
        var movies = JSONMethods.ReadJSON<Movie>(MovieHandler.FileName);
        var showings = JSONMethods.ReadJSON<Show>(FileName).Where(s => s.DateAndTime >= DateTime.Now);
        var query =
            from movie in movies
            join showing in showings on movie.Id equals showing.MovieId
            select movie;
        return query.Distinct().ToList();
    }
}