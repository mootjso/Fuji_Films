public static class AdminHandler
{
    public static void StartMenu(User adminAccount)
    {   
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial report", "Add/Remove/View Movies", "Add/Remove show","Change movie details", "Take out seat(s)", "Log out"};
        
        while (true)
        {   
            int selection = Menu.Start(MenuText, MenuOptions, true);

            const int FinancialReportOption = 0;
            const int AddRemoveMovieOption = 1;
            const int AddRemoveShowOption = 2;
            const int ChangeMovieDetailsOption = 3;
            const int TakeOutSeatsOption = 4;
            const int LogOutOption = 5;
  
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
                case ChangeMovieDetailsOption:
                    Console.Clear();
                    ChangeMovieDetails.EditMovieDescription();
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

        runTime = GetInputDataInt("Runtime");
        ageRating = GetInputDataInt("Age Rating");

        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
        movies.Add(new Movie(id, title, language, description, genres, runTime, ageRating));
        JSONMethods.WriteToJSON<Movie>(movies, JSONMethods.MovieFileName);
    }

    public static void RemoveMovie()
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
            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine($"Are you sure you want to remove \"{movieToRemove.Title}\"? (Y/N)");
                string? choice = Console.ReadLine();
                choice?.ToUpper();
                switch (choice)
                {
                    case "Y":
                        movies.Remove(movieToRemove);
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
        Console.ReadLine();
    }

    private static void EditMovieList()
    {
        List<string> menuOptions = new() { "Add Movie", "Remove Movie", "View Movies", "Back" };

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
                    RemoveMovie();
                    break;
                case 2:
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("Not implemented yet.");
                    Console.ReadLine();
                    break;
                case 3:
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
        Console.Write($"{information}: ");
        while (true)
        {
            input += Console.ReadLine();
            if (input.Length > 0) 
                break;
            Console.WriteLine("Invalid input");
        }
        return input;
    }

    private static int GetInputDataInt(string information)
    {
        int input;
        Console.Write($"{information}: ");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out input))
            {
                if (input > 0)
                    return input;
            }
            Console.WriteLine("Invalid number");
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
}
