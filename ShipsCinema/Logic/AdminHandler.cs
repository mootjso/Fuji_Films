public static class AdminHandler
{
    private const int MainAdminId = 1;
    
    public static void StartMenu(User adminAccount)
    {
        string MenuText = $"Welcome Captain!\n\nWhat would you like to do?";
        List<string> MenuOptions = new() { "Financial Reports", "Movies: Add/Remove/Edit/View", "Showings: Add/Remove", "Take Out Seat(s)", "Set Admin Rights", "Log Out" };

        while (true)
        {
            int selection = Menu.Start(MenuText, MenuOptions, true);

            const int FinancialReportOption = 0;
            const int AddRemoveMovieOption = 1;
            const int AddRemoveShowOption = 2;
            const int TakeOutSeatsOption = 3;
            const int SetAdminRightsOption = 4;
            const int LogOutOption = 5;

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
                case SetAdminRightsOption:
                    Console.Clear();
                    SetAdminRights();
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
                    MovieHandler.AddMovie();
                    break;
                case 1:
                    Console.Clear();
                    MovieHandler.RemoveMovie();
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

    public static void TakeOutSeats(User adminAccount)
    {
        Show? show = ShowHandler.SelectShowFromSchedule(true);
        if (show == null)
            return;

        Theater theater = TheaterHandler.CreateOrGetTheater(show);

        TheaterHandler.SelectSeats(adminAccount, theater);
    }

    private static void SetAdminRights()
    {
        string header = "  Type".PadRight(8) + " | " + "First Name".PadRight(15) + " | " + "Last Name".PadRight(15) + " | " + "Email".PadRight(22) + " | " + "Phonenumber"
               + "\n--------------------------------------------------------------------------------------";
        List<User> userObjects = LoginHandler.Users // Skip the Main Admin, that account's admin rights cannot be changed
            .Where(u => u.Id != MainAdminId)
            .ToList();
        List<string> userStrings = new();

        while (true)
        {
            userObjects = userObjects
                .OrderByDescending(u => u.IsAdmin)
                .ThenBy(u => u.Id)
                .ToList();
            userStrings = userObjects.Select(u => u.ToString()).ToList();
            userStrings.Add("Back");

            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            int index = Menu.Start($"Set Admin Rights\n\nSelect a user to change the Admin rights:\n\n{header}", userStrings, true);
            if (index == userStrings.Count || index == userStrings.Count - 1) // Back selected or Escape pressed
                return;

            User selectedUser = userObjects[index];
            
            Console.WriteLine($"\nChange the admin rights for {selectedUser.FirstName} {selectedUser.LastName}?\n[Y] Yes, change the Admin Rights\n[N] No, cancel");
            ConsoleKey pressedKey = Console.ReadKey(true).Key;
            if (pressedKey == ConsoleKey.Y)
            {
                selectedUser.IsAdmin = !selectedUser.IsAdmin;
                if (selectedUser.IsAdmin)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{selectedUser.FirstName} {selectedUser.LastName} now has Admin rights");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{selectedUser.FirstName} {selectedUser.LastName} no longer has Admin rights");
                }
                
                Console.ResetColor();
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
            }
            else if (pressedKey == ConsoleKey.N)
            {
                continue;
            }
        }
    }
}
