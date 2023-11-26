public static class MovieHandler
{
    public const string FileName = "movies.json";
    public static List<Movie> Movies;

    static MovieHandler()
    {
        Movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
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
    }

    public static List<string> GetMovieTitles() => Movies.Select(movie => movie.Title).ToList();

    public static void ViewCurrentMovies(Action<Movie> func, bool isAdmin = false)
    {
        Movies = JSONMethods.ReadJSON<Movie>(FileName).ToList();
        int oldMovieCount = Movies.Count;
        if (Movies.Count == 0)
        {
            List<string> menuOption = new() { "Back" };
            Menu.Start("Current Movies\n\nThere are no movies currently available", menuOption, isAdmin);
            return;
        }
        string menuText = "Current Movies\n\nSelect a movie for more information:";
        List<string> menuOptionsFull = GetMovieTitles();
        List<string> menuOptions;
        if (menuOptionsFull.Count >= 10)
            menuOptions = menuOptionsFull.GetRange(0, 10);
        else
            menuOptions = menuOptionsFull.GetRange(0, menuOptionsFull.Count);
        menuOptions.AddRange(new List<string> { "[Back]", "[Previous Page]", "[Next Page]" });
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
            else if (selection == menuOptions.Count - 1 && pageNumber < (maxPages - 1)) // Next page
                pageNumber++;
            else if (selection == menuOptions.Count - 2 && pageNumber != 0) // Previous page
                pageNumber--;
            else if (selection == menuOptions.Count - 3)
                return;
            else if (selection >= 0 && selection < menuOptions.Count - 2)
            {
                selection += (pageNumber * 10);
                Movie movie = Movies[selection];
                func(movie);
                int newMovieCount = JSONMethods.ReadJSON<Movie>(FileName).Count();
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
            menuOptions.AddRange(new List<string> { "[Back]", "[Previous Page]", "[Next Page]" });
        }
    }
}