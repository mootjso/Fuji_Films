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

    public static void ViewCurrentMovies(Action<Movie> func, bool isAdmin = false)
    {
        // parameter func -> lambda to perform certain action on movie object
        // 1. View Movie Details -> m => DisplayMovieDetails(m)
        // 2. Remove Movie from Json -> m => RemoveMovieFromJson(m, movies) with movies being the movie list
        Movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
        int oldMovieCount = Movies.Count;
        if (Movies.Count == 0)
        {
            List<string> menuOption = new() { "Back" };
            Menu.Start("Current Movies\n\nThere are currently no movies available", menuOption, isAdmin);
            return;
        }
        string menuText = $"Current Movies\n\nSelect a movie for more information:\n" +
            $"  {"Title", -30} | {"Language", -10} | {"Genres", -30} | {"Runtime", -8} | Age\n" +
            $"  {new string('-', 93)}";
        
        List<string> menuOptionsFull;
        
        if (isAdmin)
            menuOptionsFull = GetMovieTitles(Movies);
        else
            menuOptionsFull = GetMovieTitles(ShowHandler.GetScheduledMovies());
        List<string> menuOptions;
        if (menuOptionsFull.Count >= 10)
            menuOptions = menuOptionsFull.GetRange(0, 10);
        else
            menuOptions = menuOptionsFull.GetRange(0, menuOptionsFull.Count);
        
        if (menuOptions.Count <= 0)  // No movies scheduled
        {
            List<string> menuOption = new() { "Back" };
            Menu.Start("Current Movies\n\nThere are currently no movies available", menuOption, isAdmin);
            return;
        }
        menuOptions.AddRange(new List<string> { "  Previous Page", "  Next Page", "  Back" });
        int pageNumber = 0;
        int pageSize = 10;
        int maxPages = Convert.ToInt32(Math.Ceiling((double)menuOptionsFull.Count / pageSize));
        int firstTitleIndex;
        int endIndex;

        while (true)
        {
            int selection = Menu.Start(menuText, menuOptions, isAdmin);
            if (selection == menuOptions.Count)
                break; // Go back to main menu
            else if (selection == menuOptions.Count - 2 && pageNumber < (maxPages - 1)) // Next page
                pageNumber++;
            else if (selection == menuOptions.Count - 3 && pageNumber != 0) // Previous page
                pageNumber--;
            else if (selection == menuOptions.Count - 1)
                return;
            else if (selection >= 0 && selection < menuOptions.Count - 3)
            {
                selection += (pageNumber * 10);
                Movie movie = Movies[selection];
                func(movie);
                int newMovieCount = JSONMethods.ReadJSON<Movie>(FileName).Count();
                // Check if movie has been deleted, return if yes
                // That way you don't go back to the movie menu, deleted movie would still be visible there
                if (newMovieCount != oldMovieCount)
                    return;
            }
            firstTitleIndex = pageSize * pageNumber;
            // Prevent Error when page has less than 10 entries
            endIndex = menuOptionsFull.Count % 10;
            if (endIndex != 0 && pageNumber == maxPages - 1)
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, endIndex);
            else
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, pageSize);
            menuOptions.AddRange(new List<string> { "  Previous Page", "  Next Page", "  Back" });
        }
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