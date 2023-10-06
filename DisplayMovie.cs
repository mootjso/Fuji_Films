public static class DisplayMovie
{
    static List<Movie> movies = JSONMethods.ReadJSON(JSONMethods.MovieFileName);
    public static void DisplayMovieList()
    {
        string menuText = "List of Movies:";
        List<string> menuOptionsFull = movies.Select(movie => movie.Title).ToList();
        List<string> menuOptions = menuOptionsFull.GetRange(0, 10);
        menuOptions.AddRange(new List<string> { "Previous Page", "Next Page" });
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
                DisplayMovieDetails(selection);
            }
            firstTitleIndex = pageSize * pageNumber;
            //if (firstTitleIndex > 0)
             //   firstTitleIndex--;
            // Prevent Error when page has less than 10 entries
            endIndex = menuOptionsFull.Count % 10;
            if (endIndex != 0 && pageNumber == maxPages - 1)
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, endIndex);
            else
                menuOptions = menuOptionsFull.GetRange(firstTitleIndex, pageSize);
            menuOptions.AddRange(new List<string> { "Previous Page", "Next Page" });
        }
    }

    public static void DisplayMovieDetails(int selection)
    {
        Console.Clear();
        DisplayAsciiArt.Header();
        Movie movie = movies[selection];
        Console.WriteLine($"Title: {movie.Title}");
        Console.WriteLine($"Language: {movie.Language}");
        Console.WriteLine($"Description: {movie.Description}");
        Console.Write("Genres: ");
        foreach (var genre in movie.Genres)
        {
            Console.Write($"{genre}; ");
        }
        Console.WriteLine();
        Console.WriteLine($"Runtime: {movie.Runtime} Minutes");
        Console.WriteLine($"IsAdult: {movie.IsAdult}");
        Console.WriteLine("\nPress any key to go back");
        Console.ReadKey();
    }
}