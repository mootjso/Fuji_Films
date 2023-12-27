public static class MovieHandler
{
    public const string FileName = "Datasources/movies.json";
    public static List<Movie> Movies;

    static MovieHandler()
    {
        Movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
    }
    
    public static Movie? GetMovieById(int id)
    {
        Movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
        foreach (var movie in Movies)
        {
            if (movie.Id == id)
                return movie;
        }
        return null;
    }

    public static void PrintInfo(Movie movie)
    {
        Console.Write("Title: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(movie.Title + "\n");
        Console.ResetColor();
        Console.WriteLine($"Description:");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(movie.Description);
        Console.ResetColor();
        Console.Write("Genres: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(string.Join(", ", movie.Genres));
        Console.ResetColor();
        Console.Write($"\nLanguage: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(movie.Language);
        Console.ResetColor();
        Console.Write("Runtime: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{movie.Runtime} Minutes\n");
        Console.ResetColor();
        Console.Write($"Age Rating: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{movie.AgeRating}+");
        Console.ResetColor();
    }

    public static void DisplayMovieDetails(Movie movie, bool isAdmin = false)
    {
        Console.Clear();
        if (isAdmin)
            DisplayAsciiArt.AdminHeader();
        else
            DisplayAsciiArt.Header();
        Console.WriteLine("Current Movies\n");
        PrintInfo(movie);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n\nPress any key to go back");
        Console.ResetColor();
        
        Console.ReadKey();
        MovieSelectionMenu(movie, isAdmin);
    }

    public static List<string> GetMovieTitles(List<Movie> movies)
    {
        Movies = movies;
        string title, language, genres, genresTemp, runTime, ageRating;
        List<string> titles = new();
        foreach (var movie in Movies)
        {
            language = movie.Language.ToUpper() switch
            {
                "EN" => "English",
                "FR" => "French",
                "NL" => "Dutch",
                "FI" => "Finnish",
                "DU" => "German",
                _ => movie.Language.ToUpper()
            };
            title = movie.Title.Substring(0, Math.Min(movie.Title.Length, 27));
            if (title.Length != movie.Title.Length)
                title += "...";
            genresTemp = string.Join(", ", movie.Genres);
            genres = genresTemp.Substring(0, Math.Min(genresTemp.Length, 27));
            if (genres.Length != genresTemp.Length)
                genres += "...";
            runTime = $"{movie.Runtime}".PadLeft(3);
            ageRating = $"{movie.AgeRating}+";
            titles.Add($"{title, -30} | {language, -10} | {genres, -30} | {runTime + " min", -8} | {ageRating}");
        }
        return titles;
    }

    public static bool ViewCurrentMovies(Action<Movie> action, bool removeMovie = false, bool isAdmin = false)
    {
        // parameter func -> lambda to perform certain action on movie object
        // 1. View Movie Details -> m => DisplayMovieDetails(m)
        // 2. Remove Movie from Json -> m => RemoveMovieFromJson(m, movies) with movies being the movie 
        string messageWhenEmpty = "Current Movies\n\nThere are currently no movies available";
        string menuText = $"Current Movies\n\nSelect a movie for more information:\n" +
            $"  {"Title", -30} | {"Language", -10} | {"Genres", -30} | {"Runtime", -8} | Age\n" +
            $"  {new string('-', 93)}";

        List<Movie> menuOptionsFullObjects;
        List<string> menuOptionsFull;
        
        if (isAdmin)
        {
            menuOptionsFullObjects = JSONMethods.ReadJSON<Movie>(FileName).ToList();
            menuOptionsFull = GetMovieTitles(menuOptionsFullObjects);
        }
        else
        {
            menuOptionsFullObjects = ShowHandler.GetScheduledMovies();
            menuOptionsFull = GetMovieTitles(menuOptionsFullObjects);
        }
        Func<Movie, bool> func = m =>
        {
            action(m);
            return removeMovie;
        };
        return Menu.MenuPagination(menuOptionsFull, menuText, messageWhenEmpty, func, menuOptionsFullObjects, isAdmin);
    }

    public static void MovieSelectionMenu(Movie movie, bool isAdmin = false)
    {
        List<string> menuOptions = new() { "View Details Dit is om te testen", "View Showings", "Back" };
        string menuText = $"{movie.Title}\n\nSelect an option:";
        int selection = Menu.Start(menuText, menuOptions, isAdmin);
        switch (selection)
        {
            case 0:
                DisplayMovieDetails(movie, isAdmin);
                break;
            case 1:
                ShowHandler.PrintMovieDates(movie, isAdmin);
                break;
            case 2:
                return;
            default:
                break;
        }
    }
    public static void AddMovie()
    {
        DisplayAsciiArt.AdminHeader();
        string title, language, description, genre;
        int highestId, runTime, genreCount, ageRating;
        List<string> genres = new();
        Console.CursorVisible = true;
        title = GetInputDataString("Title");
        if (title == "q")
            return;
        language = GetInputDataString("Language");
        if (language == "q")
            return;
        description = GetInputDataString("Description");
        if (description == "q")
            return;

        genreCount = GetInputDataInt("How many genres?");
        if (genreCount == -1)
            return;
        for (int i = 0; i < genreCount; i++)
        {
            genre = GetInputDataString($"Genre {i + 1}");
            if (title == "q")
                return;
            genres.Add(genre);
        }

        runTime = GetInputDataInt("Runtime (minutes)");
        if (runTime == -1)
            return;
        ageRating = GetInputDataInt("Age Rating");
        if (ageRating == -1)
            return;
        highestId = GetHighestID();
        Movie movieToAdd = new Movie(highestId, title, language, description, genres, runTime, ageRating);
        bool inMenu = true;
        ConsoleKey choice;
        while (inMenu)
        {
            Console.CursorVisible = false;
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Please confirm the movie details\n");
            PrintInfo(movieToAdd);
            Console.WriteLine("\n\nAre you sure the movie details are correct?\n[Y] Yes, add the new movie\n[N] No, this information is incorrect");
            choice = Console.ReadKey().Key;
            switch (choice)
            {
                case ConsoleKey.Y:
                    inMenu = false;
                    break;
                case ConsoleKey.N:
                    inMenu = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\nAddition of movie \"{movieToAdd.Title}\" aborted");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadLine();
                    return;
                default:
                    continue;
            }
        }
        Console.Clear();
        DisplayAsciiArt.AdminHeader();
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
        movies.Add(movieToAdd);
        JSONMethods.WriteToJSON(movies, JSONMethods.MovieFileName);
        Console.WriteLine("Adding a new movie\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Movie \"{movieToAdd.Title}\" has been added");
        Console.ResetColor();
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static void RemoveMovieFromJson(Movie movieToRemove)
    {
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
        ConsoleKey choice;

        while (true)
        {
            Console.WriteLine($"Are you sure you want to delete {movieToRemove.Title}? (Y/N)");
            choice = Console.ReadKey(true).Key;
            switch (choice)
            {
                case ConsoleKey.Y:
                    movies = movies.Where(m => m.Id != movieToRemove.Id).ToList();
                    JSONMethods.WriteToJSON(movies, FileName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{movieToRemove.Title} has been removed");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey(true);
                    return;
                case ConsoleKey.N:
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid key, please choose either [Y] or [N]");
                    Console.ResetColor();
                    break;
            }
        }
    }

    public static void RemoveMovie()
    {
        bool backSelected = ViewCurrentMovies(RemoveMovieFromJson, true, true);
        if (backSelected)
            return;
        RemoveMovie();
    }


    private static string GetInputDataString(string information)
    {
        string input = "";
        while (true)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Adding new movie\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter 'q' at any of the prompts to go back.");
            Console.ResetColor();
            Console.Write($"{information}: ");
            Console.ForegroundColor = Program.InputColor;
            input += Console.ReadLine();
            Console.ResetColor();
            if (input.Length > 0)
                break;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input");
            Console.ResetColor();
            Console.WriteLine("Press any key to try again");
            Console.ReadKey();
            Console.CursorVisible = true;
        }
        return input;
    }

    private static int GetInputDataInt(string information)
    {
        int input;
        while (true)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Adding new movie\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter 'q' at any of the prompts to go back.");
            Console.ResetColor();
            Console.Write($"{information}: ");
            Console.ForegroundColor = Program.InputColor;
            string userInput = Console.ReadLine();
            Console.ResetColor();
            if (userInput == "q")
                return -1;

            if (int.TryParse(userInput, out input))
            {
                if (input > 0)
                    return input;
            }
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid number");
            Console.ResetColor();
            Console.WriteLine("Press any key to try again");
            Console.ReadKey();
            Console.CursorVisible = true;
        }
    }

    public static int GetHighestID()
    {
        IEnumerable<Movie> movies = JSONMethods.ReadJSON<Movie>(FileName);
        if (movies.Count() > 0)
        {
            return movies.Max(m => m.Id);
        }
        return 0;
    }
}