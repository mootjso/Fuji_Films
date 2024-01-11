using System.Net.Quic;

public class Program
{
    public const ConsoleColor InputColor = ConsoleColor.Cyan;
    
    private static void Main()
    {
        bool runApp = true;
        User? loggedInUser = null;
        while (runApp)
        {
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };
            while (loggedInUser == null)
            {
                DisplayAsciiArt.Standby();
                DisplayAsciiArt.OpeningLogo();

                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:
                        loggedInUser = LoginHandler.LogIn();
                        if (loggedInUser == null)
                            continue;
                        break;
                    case 1:
                        loggedInUser = LoginHandler.Register();
                        if (loggedInUser == null)
                            continue;
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n\tThank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\n\tWe hope to sea you soon!");
                        Thread.Sleep(1500);
                        continue;
                    default:
                        break;
                }
            }

            if (loggedInUser.IsAdmin)
            {
                runApp = AdminHandler.StartMenu(loggedInUser);
                loggedInUser = null;
            } 

            menuOptions = new() { "Current Movies", "Showing Schedule", "My Reservations", "Log Out" };
            while (loggedInUser != null)
            {
                menuText = $"Hello, {loggedInUser.FirstName} {loggedInUser.LastName}\n";
                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:
                        Console.Clear();
                        MovieHandler.ViewCurrentMovies(m => MovieHandler.MovieSelectionMenu(m, false, loggedInUser), false);
                        break;
                    case 1:
                        CheckOutHandler.AddShowToRevenue();
                        Console.Clear();
                        Show? selectedShow = ShowHandler.SelectShowFromSchedule();
                        if (selectedShow is null)
                            continue;

                        var theater = TheaterHandler.CreateOrGetTheater(selectedShow);
                        TheaterHandler.SelectSeats(loggedInUser, theater);
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        ReservationHandler.AddTicketsToReservations();
                        ReservationHandler.GetReservationsByUser(loggedInUser);
                        
                        break;
                    case 3:
                        loggedInUser = null;
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n\tThank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\n\tWe hope to sea you soon!");
                        Thread.Sleep(1500);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
