public static class AdminHandler
{
    public static void StartMenu()
    {   
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial report", "Add movie", "Add/Remove show","Change movie details", "Take out seat(s)", "Log out"};
        
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
                    AddMovie();
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
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("\nTAKE OUT SEAT(S) NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
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
}
