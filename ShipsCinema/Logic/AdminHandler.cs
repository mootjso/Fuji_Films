public static class AdminHandler
{
    public static void StartMenu(User adminAccount)
    {
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() { "Financial Reports", "Movies: Add/Remove/Edit/View", "Showings: Add/Remove", "Take Out Seat(s)", "Log Out" };

        while (true)
        {
            int selection = Menu.Start(MenuText, MenuOptions, true);

            const int FinancialReportOption = 0;
            const int AddRemoveMovieOption = 1;
            const int AddRemoveShowOption = 2;
            const int TakeOutSeatsOption = 3;
            const int LogOutOption = 4;

            switch (selection)
            {
                case FinancialReportOption:
                    Console.Clear();
                    FinancialMenu.Start();
                    break;
                case AddRemoveMovieOption:
                    Console.Clear();
                    EditMovieList();
                    break;
                case AddRemoveShowOption:
                    Console.Clear();
                    ShowHandler.EditShowSchedule();
                    break;
                case TakeOutSeatsOption:
                    Console.Clear();
                    TakeOutSeats(adminAccount);
                    break;
                case LogOutOption:
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("\n\n         Bon Voyage, Captain!");
                    Thread.Sleep(800);
                    Console.WriteLine("\nMay your guidance bring us waves of cinematic success! ");
                    Thread.Sleep(1500);
                    return;
                default:
                    break;
            }
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
        language = GetInputDataString("Language");
        description = GetInputDataString("Description");

        genreCount = GetInputDataInt("How many genres?");
        for (int i = 0; i < genreCount; i++)
        {
            genre = GetInputDataString($"Genre {i + 1}");
            genres.Add(genre);
        }

        runTime = GetInputDataInt("Runtime (minutes)");
        ageRating = GetInputDataInt("Age Rating");
        highestId = GetHighestID();
        Movie movieToAdd = new Movie(highestId, title, language, description, genres, runTime, ageRating);
        bool inMenu = true;
        string? choice;
        while (inMenu)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            MovieHandler.PrintInfo(movieToAdd);
            Console.WriteLine("\nAre you sure the movie details are correct? (Y/N)");
            choice = Console.ReadLine();
            if (choice != null)
                choice = choice.ToUpper();
            switch (choice)
            {
                case "Y":
                    inMenu = false;
                    break;
                case "N":
                    inMenu = false;
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Addition of movie \"{movieToAdd.Title}\" aborted");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    return;
                default:
                    Console.WriteLine("Invalid input, please write \"Y\" or \"N\"");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
            }
        }
        Console.Clear();
        DisplayAsciiArt.AdminHeader();
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
        movies.Add(movieToAdd);
        JSONMethods.WriteToJSON(movies, JSONMethods.MovieFileName);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Movie \"{movieToAdd.Title}\" has been added");
        Console.ResetColor();
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static void RemoveMovieFromJson(Movie movieToRemove)
    {
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(MovieHandler.FileName).ToList();
        ConsoleKey choice;

        while (true)
        {
            Console.WriteLine($"Are you sure you want to delete {movieToRemove.Title}? (Y/N)");
            choice = Console.ReadKey(true).Key;
            switch (choice)
            {
                case ConsoleKey.Y:
                    movies = movies.Where(m => m.Id != movieToRemove.Id).ToList();
                    JSONMethods.WriteToJSON(movies, MovieHandler.FileName);
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

    private static void RemoveMovie()
    {
        bool backSelected = MovieHandler.ViewCurrentMovies(RemoveMovieFromJson, true, true);
        if (backSelected)
            return;
        RemoveMovie();
    }

    private static void EditMovieList()
    {
        List<string> menuOptions = new() { "Add Movie", "Remove Movie", "Edit Movie", "View Movies", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Movie Listings\n\nSelect an option:", menuOptions, true);
            if (index == menuOptions.Count || index == menuOptions.Count-1)
            {
                break;
            }
            switch (index)
            {
                case 0:
                    Console.Clear();
                    AddMovie();
                    break;
                case 1:
                    Console.Clear();
                    RemoveMovie();
                    break;
                case 2:
                    Console.Clear();
                    ChangeMovieDetails.EditMovieDescription();
                    break;
                case 3:
                    Console.Clear();
                    MovieHandler.ViewCurrentMovies(m => MovieHandler.MovieSelectionMenu(m, true), isAdmin: true);
                    break;
                case 4:
                    inMenu = false;
                    break;
                default:
                    break;
            }
        }
    }

    private static string GetInputDataString(string information)
    {
        string input = "";
        while (true)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.Write($"{information}: ");
            input += Console.ReadLine();
            if (input.Length > 0) 
                break;
            Console.WriteLine("Invalid input\nPress any key to continue");
            Console.ReadLine();
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
            Console.Write($"{information}");
            Console.Write(": ");
            if (int.TryParse(Console.ReadLine(), out input))
            {
                if (input > 0)
                    return input;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid number");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }

    public static void TakeOutSeats(User adminAccount)
    {
        Show? show = ShowHandler.SelectShowFromSchedule(true);
        if (show == null)
            return;

        Theater theater = TheaterHandler.CreateOrGetTheater(show);

        TheaterHandler.SelectSeats(adminAccount, theater);
    }

    public static int GetHighestID()
    {
        IEnumerable<Movie> movies = JSONMethods.ReadJSON<Movie>(MovieHandler.FileName);
        if (movies.Count() > 0)
        {
            return movies.Max(m => m.Id);
        }
        return 0;
    }
}
