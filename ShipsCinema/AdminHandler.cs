public static class AdminHandler
{
    public static void StartMenu(User adminAccount)
    {   
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial report", "Movies: Add/Remove/Edit/View", "Add/Remove show", "Take out seat(s)", "Log out"};
        
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
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("\nFINANCIAL REPORT NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
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
        int id, runTime, genreCount, ageRating;
        List<string> genres = new();
        Console.CursorVisible = true;
        id = GetInputDataInt("ID");
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
        Movie movieToAdd = new Movie(id, title, language, description, genres, runTime, ageRating);
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

    private static void RemoveMovieFromJson(Movie movieToRemove, List<Movie> movies)
    {
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Are you sure you want to remove \"{movieToRemove.Title}\"? (Y/N)");
            string? choice = Console.ReadLine();
            if (choice != null)
                choice = choice.ToUpper();
            switch (choice)
            {
                case "Y":
                    movies = movies.Where(m => m.Id != movieToRemove.Id).ToList();
                    JSONMethods.WriteToJSON(movies, JSONMethods.MovieFileName);
                    Console.WriteLine($"Movie \"{movieToRemove.Title}\" has been removed");
                    Console.WriteLine("Press any key to continue");
                    inMenu = false;
                    break;
                case "N":
                    Console.WriteLine($"Deletion of \"{movieToRemove.Title}\" aborted");
                    Console.WriteLine("Press any key to continue");
                    inMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid option, please pick \"Y\" or \"N\"");
                    Console.WriteLine("Press any key to continue");
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
            Console.WriteLine("Enter the ID of the movie you want to remove");
            Console.CursorVisible = true;
            if (int.TryParse(Console.ReadLine(), out id))
                break;
            Console.WriteLine("Please enter a valid integer");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
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
            Console.WriteLine($"No movie with ID {id} found");
        else
        {
            RemoveMovieFromJson(movieToRemove, movies);
        }
        Console.ReadLine();
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
            if (information == "ID")
            {
                int highestId = GetHighestID();
                if (highestId > 0)
                    Console.Write($" (Highest ID is {highestId}): ");
                else
                {
                    Console.Write($" (Highest Id: n/a): ");
                }
            }
            else
                Console.Write(": ");
            if (int.TryParse(Console.ReadLine(), out input))
            {
                if (CheckIDAvailable(input) && information == "ID")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ID {input} is unvailable");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    continue;
                }
                if (input > 0)
                    return input;
            }
            Console.WriteLine("Invalid number\nPress any key to continue");
            Console.ReadLine();
        }
    }

    public static void TakeOutSeats(User adminAccount)
    {
        Show? show = ShowHandler.SelectShowFromSchedule(true);
        if (show == null)
            return;

        Theater theater = TheaterHandler.CreateTheater(show);

        TheaterHandler.SelectSeats(adminAccount, theater, null);
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

    public static bool CheckIDAvailable(int id)
    {
        IEnumerable<Movie> movies = JSONMethods.ReadJSON<Movie>(MovieHandler.FileName);
        return movies.Any(m => m.Id == id);
    }
}
