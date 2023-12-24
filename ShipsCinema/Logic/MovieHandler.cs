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
        Console.WriteLine($"{movie.Title}");
        Console.WriteLine($"\nDescription:\n{movie.Description}");
        Console.Write("\nGenres: ");
        Console.Write(string.Join(", ", movie.Genres));
        Console.WriteLine($"\nLanguage: {movie.Language}");
        Console.WriteLine($"Runtime: {movie.Runtime} Minutes");
        Console.WriteLine($"Age Rating: {movie.AgeRating}");
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
        Console.WriteLine("\nPress any key to go back");
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
                "FI" => "Finish",
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
        List<string> menuOptions = new() { "View Details", "View Showings", "Back" };
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
}