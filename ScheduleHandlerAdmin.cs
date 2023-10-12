public static class MovieScheduleHandlerAdmin
{
    private const string FileName = "movie_schedule.json";
    private static int latestScheduledMovieID = 1;
    private static List<ScheduledMovie> scheduledMovies;

    static MovieScheduleHandlerAdmin()
    {
        // PUT THIS IN A SEPARATE FUNCTION TO REUSE WHEN NEEDED
        List<ScheduledMovie> scheduledMovies = JSONMethods.ReadJSON<ScheduledMovie>(FileName);

        latestScheduledMovieID = scheduledMovies.MaxBy(sm => sm.Id).Id;
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
        List<Movie> movies = JSONMethods.ReadJSON<Movie>("movies.json");

        bool inMenu = true;
        while (inMenu)
        {
            // Movie Selection
            List<string> moviesList = GetMovieTitles(movies);
            int index = Menu.Start("Select a movie to add to the schedule:\n", GetMovieTitles(movies));
            if (index == moviesList.Count)  // If user presses left arrow key leave current while loop
                break;
            Movie movieToAdd = movies[index];
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
            ScheduledMovie newMovie = new(movieToAdd, selectedTime) { Id = latestScheduledMovieID += 1 };
            int selection = ConfirmSelection(newMovie);
            if (selection == 0)
            {
                MovieScheduleHandlerUser.Movies.Add(newMovie);

                JSONMethods.WriteToJSON(MovieScheduleHandlerUser.Movies, FileName);
            }
            else
                continue;

            // 
            List<string> menuOptions;
            menuOptions = new() { "Add a new movie to the schedule  ", "Back" };
            int option = Menu.Start("", menuOptions);
            if (option == 1 || option == menuOptions.Count)
                inMenu = false;
        }
    }
    
    public static void RemoveMovieFromSchedule()
    {
        List<string> dates = MovieScheduleHandlerUser.GetAllDates();
        dates.Add("Back");

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
            List<ScheduledMovie> moviesForDate = MovieScheduleHandlerUser.GetMoviesByDate(selectedDate);
            List<string> movieMenuStrings = MovieScheduleHandlerUser.CreateListMovieStrings(moviesForDate);

            index = Menu.Start($"Date: {dateString}\n", movieMenuStrings);
            if (index == movieMenuStrings.Count || index == movieMenuStrings.Count - 1)
            {
                continue;
            }

            ScheduledMovie movieToRemove = moviesForDate[index];
            RemoveFromJson(movieToRemove);

            // Confirmation message
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nThe following movie has been removed from the schedule:\n");

            Console.ResetColor();
            Console.WriteLine(movieMenuStrings[index]);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\nPress any key to continue");
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

    private static int ConfirmSelection(ScheduledMovie movie)
    {
        List<string> menuOptions = new() { "Confirm Selection", "Cancel" };
        string confirmationMessage = $"Movie: {movie.Movie.Title}\n" +
            $"Date: {movie.StartTime.Date.ToString()[..10]}\n" +
            $"Time: {movie.StartTime.TimeOfDay.ToString()[..5]} - {movie.EndTime.TimeOfDay.ToString()[..5]}\n";
        return Menu.Start(confirmationMessage, menuOptions);
    }

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

    private static void RemoveFromJson(ScheduledMovie movieToRemove)
    {
        // TODO PUT THIS IN A SEPARATE FUNCTION (THIS LINE IS ALSO IN THE CONSTRUCTOR)
        List<ScheduledMovie> movies = JSONMethods.ReadJSON<ScheduledMovie>(FileName);
        List<ScheduledMovie> newMovies = new();
        foreach (ScheduledMovie movie in movies)
        {
            if (!(movie.Id == movieToRemove.Id))
            {
                newMovies.Add(movie);
            }
        }
        JSONMethods.WriteToJSON(newMovies, FileName);
    }
}