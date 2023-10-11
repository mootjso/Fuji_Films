using System;

public static class AddMovieToSchedule

    // TODO IMPLEMENT A FUNCTION TO REMOVE MOVIES FROM SCHEDULE THAT ARE IN THE PAST
{
    private const string FileName = "movie_schedule.json";
    private static int latestScheduledMovieID = 1;

    static AddMovieToSchedule()
    {
        List<ScheduledMovie> scheduledMovies = JSONMethods.ReadJSON<ScheduledMovie>(FileName); ;

        latestScheduledMovieID = scheduledMovies.MaxBy(sm => sm.Id).Id;
    }

    public static void Start()
    {
        List<Movie> movies = JSONMethods.ReadJSON<Movie>("movies.json");
        
        bool inMenu = true;
        while (inMenu)
        {
            List<string> moviesList = GetMovieTitles(movies);
            int index = Menu.Start("Select a movie to add to the schedule:\n", GetMovieTitles(movies));
            if (index == moviesList.Count)  // If user presses left arrow key
                break;
            Movie movieToAdd = movies[index];
            Console.Clear();
            DisplayMovieInfo(movieToAdd);

            List<string> dates = CreateDatesList();
            index = Menu.Start("Select a date:\n", dates);
            // TODO Add option to enter a custom date
            if (index == dates.Count)
                continue;
            string dateString = dates[index];


            // TODO PUT IN SEPARATE FUNCTIONS -------------------------------------------------
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
            // -------------------------------------------------------------------------------

            // Confirm or cancel selection
            ScheduledMovie newMovie = new(movieToAdd, resultDateTime) { Id = latestScheduledMovieID += 1 };
            int selection = ConfirmSelection(newMovie);
            if (selection == 0)
            {
                MovieSchedule.Movies.Add(newMovie);

                JSONMethods.WriteToJSON(MovieSchedule.Movies, FileName);
            }
            else
                continue;

            List<string> menuOptions;
            menuOptions = new() { "Add a new movie to the schedule  ", "Back" };
            int option = Menu.Start("", menuOptions);
            if (option == 1 || option == menuOptions.Count)
                inMenu = false;
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
        for (int i = 0; i < 8; i++)
        {
            dates.Add(DateToString(today));
            today = today.AddDays(1);
        }
        return dates;
    }

    public static void DisplayMovieInfo(Movie movie)
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
}