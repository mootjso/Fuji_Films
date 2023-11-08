using System.ComponentModel.Design;

public static class AdminHandler
{
    public static void StartMenu()
    {   
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial report", "Add/Remove movie", "Change movie details", "Take out seat(s)", "Log out"};
        
        while (true)
        {   
            int selection = Menu.Start(MenuText, MenuOptions, true);

            const int FinancialReportOption = 0;
            const int AddRemoveMovieOption = 1;
            const int ChangeMovieDetailsOption = 2;
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
                    Console.WriteLine("\nBon Voyage, Captain!");
                    Thread.Sleep(500);
                    Console.WriteLine("\nMay your guidance bring us waves of cinematic success! ");
                    Thread.Sleep(2000);
                    break;
                default:
                    break;
            }
        }
    }

    public static void AddMovie()
    {
        DisplayAsciiArt.AdminHeader();
        string title, language, description, genre;
        int id, runTime, genreCount;
        bool isAdult;
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
        isAdult = GetInputDataBool("IsAdult");

        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
        movies.Add(new Movie(id, title, language, description, genres, runTime, isAdult));
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

    private static bool GetInputDataBool(string information)
    {
        Console.Write($"{information} (True/False): ");
        bool info = false;
        string input;
        while (true)
        {
            input = "";
            input += Console.ReadLine();
            if (!(input == "True" || input == "False"))
            {
                Console.WriteLine("Invalid input -> True or False");
                continue;
            }
            info = input switch
            {
                "True" => true,
                "False" => false,
                _ => false
            };
            break;
        }
        return info;
    }
}
