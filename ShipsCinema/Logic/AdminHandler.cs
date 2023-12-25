public static class AdminHandler
{
    public static void StartMenu(User adminAccount)
    {   
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial Reports", "Movies: Add/Remove/Edit/View", "Showings: Add/Remove", "Take Out Seat(s)", "Log Out"};
        
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
            MovieHandler.PrintInfo(movieToAdd);
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

    private static void RemoveMovieFromJson(Movie movieToRemove, List<Movie> movies)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.CursorVisible = false;
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Movie Listings\n\nAre you sure you want to remove \"{movieToRemove.Title}\"?\n[Y] Confirm\n[N] Cancel");
            ConsoleKey choice = Console.ReadKey(true).Key;
            switch (choice)
            {
                case ConsoleKey.Y:
                    movies = movies.Where(m => m.Id != movieToRemove.Id).ToList();
                    JSONMethods.WriteToJSON(movies, JSONMethods.MovieFileName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nMovie \"{movieToRemove.Title}\" has been removed");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nPress any key to continue");
                    inMenu = false;
                    break;
                case ConsoleKey.N:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nDeletion of \"{movieToRemove.Title}\" aborted");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nPress any key to continue");
                    inMenu = false;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid option, please pick \"Y\" or \"N\"");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nPress any key to continue");
                    break;
            }
        }
    }

    private static void RemoveMovieByID()
    {
        int id;
        while (true)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Movie Listings\n\nEnter the ID of the movie you want to remove");
            Console.CursorVisible = true;
            if (int.TryParse(Console.ReadLine(), out id))
                break;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nPlease enter a valid integer");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();
            Console.ReadKey();
        }

        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
        Movie? movieToRemove = null;
        foreach (Movie movie in movies)
        {
            if (movie.Id == id)
            {
                movieToRemove = movie;
                break;
            }
        }
        if (movieToRemove is null)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor= ConsoleColor.Red;
            Console.WriteLine($"No movie with ID '{id}' found");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue");
            Console.ResetColor();

        }
        else
        {
            RemoveMovieFromJson(movieToRemove, movies);
        }
        Console.ReadKey();
    }

    private static void RemoveMovieBySelection()
    {
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(MovieHandler.FileName).ToList();
        MovieHandler.ViewCurrentMovies(m => RemoveMovieFromJson(m, movies), true);
    }

    private static void RemoveMovieMenu()
    {
        List<string> menuOptions = new() { "Remove Movie by ID", "Remove Movie by Selection", "Back" };

        bool inMenu = true;
        while (inMenu)
        {
            int index = Menu.Start("Movie Listings\n\nSelect an option:", menuOptions, true);

            switch (index)
            {
                case 0:
                    Console.Clear();
                    RemoveMovieByID();
                    break;
                case 1:
                    Console.Clear();
                    RemoveMovieBySelection();
                    break;
                case 2:
                    inMenu = false;
                    break;
                default:
                    break;
            }
        }
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
                    RemoveMovieMenu();
                    break;
                case 2:
                    Console.Clear();
                    ChangeMovieDetails.EditMovieDescription();
                    break;
                case 3:
                    Console.Clear();
                    MovieHandler.ViewCurrentMovies(m => MovieHandler.MovieSelectionMenu(m, true), true);
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
