public static class MovieHandler
{
    public const string FileName = "movies.json";
    public static List<Movie> Movies;

    static MovieHandler()
    {
        Movies = AppInitializer.GetMovieObjects();
    }
    
    public static Movie? GetMovieById(int id)
    {
            foreach (var movie in Movies)
            if (movie.Id == id) 
                return movie;
        return null;
    }

    public static void PrintInfo(Movie movie)
    {
        Console.WriteLine($"{movie.Title}");
        Console.WriteLine($"\nDescription:\n{movie.Description}");
        Console.Write("\nGenres: ");
        foreach (var genre in movie.Genres)
        {
            Console.Write($"{genre}; ");
        }
        Console.WriteLine($"\nLanguage: {movie.Language}");
        Console.WriteLine($"Runtime: {movie.Runtime} Minutes");
        Console.WriteLine($"IsAdult: {movie.IsAdult}");
    }

    public static void DisplayMovieDetails(Movie movie)
    {
        Console.Clear();
        DisplayAsciiArt.Header();
        Console.WriteLine("Current Movies\n");
        MovieHandler.PrintInfo(movie);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to go back");
        Console.ResetColor();
        
        Console.ReadKey();
    }

    public static List<string> GetMovieTitles() => Movies.Select(movie => movie.Title).ToList();

    public static void ViewCurrentMovies()
    {
        string menuText = "Current Movies\n\nSelect a movie for more information:";
        List<string> menuOptionsFull = GetMovieTitles();
        List<string> menuOptions = menuOptionsFull.GetRange(0, 10);
        menuOptions.AddRange(new List<string> { "[Previous Page]", "[Next Page]" });
        int pageNumber = 0;
        int pageSize = 10;
        int maxPages = Convert.ToInt32(Math.Ceiling((double)menuOptionsFull.Count / pageSize));
        int firstTitleIndex;
        int endIndex = 1;

        while (true)
        {
            int selection = Menu.Start(menuText, menuOptions);
            if (selection == menuOptions.Count)
                break; // Go back to main menu
            else if (selection == menuOptions.Count - 1 && pageNumber < (maxPages - 1)) // Next page
                pageNumber++;
            else if (selection == menuOptions.Count - 2 && pageNumber != 0) // Previous page
                pageNumber--;
            else if (selection >= 0 && selection < menuOptions.Count - 2)
            {
                selection += (pageNumber * 10);
                Movie movie = MovieHandler.Movies[selection];
                DisplayMovieDetails(movie);
            }
            firstTitleIndex = pageSize * pageNumber;
            // Prevent Error when page has less than 10 entries
            endIndex = menuOptionsFull.Count % 10;
            if (endIndex != 0 && pageNumber == maxPages - 1)
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, endIndex);
            else
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, pageSize);
            menuOptions.AddRange(new List<string> { "[Previous Page]", "[Next Page]" });
        }
    }
}