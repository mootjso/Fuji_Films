public static class ScheduleHandlerAdmin
{
    private const string FileName = "movie_schedule.json";
    private static int latestScheduledMovieID = 1;

    static ScheduleHandlerAdmin()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>(FileName);

        if (shows.Count > 0)
            latestScheduledMovieID = shows.MaxBy(sm => sm.Id).Id;
        else
            latestScheduledMovieID = 0;
    }
    public static void Start()
    {
        List<string> menuOptions = new() { "View schedule/Remove movie  ", "Add movie", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Movie Schedule", menuOptions);
            switch (index)
            {
                case 0:
                    RemoveMovieFromSchedule();
                    break;
                case 1:
                    AddToSchedule();
                    break;
                case 2:
                    inMenu = false;
                    break;
                default:
                    break;
            }
        }
    }

    public static void AddToSchedule()
    {
        List<Movie> Movies = MovieHandler.Movies;

        bool inMenu = true;
        while (inMenu)
        {
            // Movie Selection
            List<string> moviesList = GetMovieTitles(Movies);
            int index = Menu.Start("Select a movie to add to the schedule:\n", GetMovieTitles(Movies));
            if (index == moviesList.Count)  // If user presses left arrow key leave current while loop
                break;
            Movie movieToAdd = Movies[index];
            Console.Clear();
            DisplayMovieInfo(movieToAdd);

            // Date selection
            List<string> dates = CreateDatesList();
            index = Menu.Start("Select a date:\n", dates);
            if (index == dates.Count)  // If user presses left arrow key leave current while loop
                continue;

            // Time selection
            string dateString = dates[index];
            DateTime selectedTime = TimeSelection(movieToAdd, dateString);

            // Confirm or cancel selection
            Show newMovie = new(movieToAdd, selectedTime) { Id = latestScheduledMovieID += 1 };

            Console.Clear();
            Console.WriteLine(newMovie.DateAndTime);
            Console.ReadKey();

            //int selection = ConfirmSelection(newMovie);
            //if (selection == 0)
            //{
            //    ScheduleHandlerUser.Movies.Add(newMovie);
            //    JSONMethods.WriteToJSON(ScheduleHandlerUser.Movies, FileName);
            //}
            //else
            //    continue;

            // 
            List<string> menuOptions;
            menuOptions = new() { "Add a new movie to the schedule  ", "Back" };
            int option = Menu.Start("Movie Schedule", menuOptions);
            if (option == 1 || option == menuOptions.Count)
                inMenu = false;
        }
    }
    
    public static void RemoveMovieFromSchedule()
    {
        List<string> dates = ShowHandler.GetAllDates();
        dates.Add("Back");
        if (dates.Count <= 1)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("\nMovie Schedule");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nThere are no movies in the schedule.");
            Console.ForegroundColor= ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to go back");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Select a date to see the movies for that day\n", dates);
            if (index == dates.Count || index == dates.Count - 1)
            {
                return;
            }

            string dateString = dates[index];
            DateTime selectedDate = DateTime.Parse(dateString);
            List<Show> moviesForDate = ScheduleHandlerUser.GetMoviesByDate(selectedDate);
            List<string> movieMenuStrings = ScheduleHandlerUser.CreateListMovieStrings(moviesForDate);

            index = Menu.Start($"Date: {dateString}\nSelect the movie you want to remove\n", movieMenuStrings);
            if (index == movieMenuStrings.Count || index == movieMenuStrings.Count - 1)
            {
                continue;
            }

            Show movieToRemove = moviesForDate[index];
            ScheduleHandlerUser.Movies.Remove(movieToRemove);
            RemoveFromJson(movieToRemove);

            // Confirmation message
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe following movie has been removed from the schedule:\n");

            Console.ResetColor();
            Console.WriteLine(movieMenuStrings[index]);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            Console.ReadKey();
            
            return;
        }

    }
    
    private static List<string> GetMovieTitles(List<Movie> movies)
    {
        List<string> movieTitles = new();
        foreach (Movie movie in movies)
            movieTitles.Add(movie.Title);
        return movieTitles;
    }

    private static List<string> CreateDatesList()
    {
        List<string> dates = new();
        DateTime today = DateTime.Now;
        for (int i = 0; i < 14; i++)
        {
            dates.Add(DateToString(today));
            today = today.AddDays(1);
        }
        return dates;
    }

    private static void DisplayMovieInfo(Movie movie)
    {
        Console.Clear();
        DisplayAsciiArt.Header();

        Console.WriteLine("\nSelected movie:\n");
        Console.WriteLine($"   Title: {movie.Title}");
        Console.Write("   Genres: ");
        foreach (var genre in movie.Genres)
        {
            Console.Write($"{genre}; ");
        }
        Console.WriteLine();
        Console.WriteLine($"   Runtime: {movie.Runtime}");
        Console.WriteLine($"   IsAdult: {movie.IsAdult}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
        Console.ResetColor();
    }

    private static string DateToString(DateTime date)
    {
        string dateString = date.ToString();
        if (dateString[1] == '-')
            return $"0{dateString[..9]}";
        return dateString[..10];
    }

    //private static int ConfirmSelection(Show movie)
    //{
    //    List<string> menuOptions = new() { "Confirm Selection", "Cancel" };
    //    string confirmationMessage = $"Movie: {movie.Movie.Title}\n" +
    //        $"Date: {movie.StartTimeString.Date.ToString()[..10]}\n" +
    //        $"Time: {movie.StartTimeString.TimeOfDay.ToString()[..5]} - {movie.EndTimeString.TimeOfDay.ToString()[..5]}\n";
    //    return Menu.Start(confirmationMessage, menuOptions);
    //}

    private static DateTime TimeSelection(Movie movieToAdd, string dateString)
    {
        DateTime resultDateTime = DateTime.Now;
        bool correctInput = false;
        while (!(correctInput))
        {
            Console.Clear();
            Console.CursorVisible = true;
            DisplayAsciiArt.Header();
            Console.WriteLine($"\nMovie: {movieToAdd.Title}\nDate: {dateString}");
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

    private static void RemoveFromJson(Show movieToRemove)
    {
        List<Show> movies = JSONMethods.ReadJSON<Show>(FileName);
        List<Show> newMovies = new();
        foreach (Show movie in movies)
        {
            if (!(movie.Id == movieToRemove.Id))
            {
                newMovies.Add(movie);
            }
        }
        JSONMethods.WriteToJSON(newMovies, FileName);
    }
}