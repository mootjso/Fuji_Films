public class Program
{
    private static void Main()
    {
        while (true)
        {
            // Login Menu
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };

            bool loggedIn = false;
            while (!loggedIn)
            {
                //DisplayAsciiArt.Standby();
                //DisplayAsciiArt.OpeningLogo();

                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n  LOGGING IN NOT IMPLEMENTEND\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                        loggedIn = true;
                        Console.ReadKey();
                        break;
                    case 1:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n  REGISTERING NOT IMPLEMENTEND\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                        loggedIn = true;
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n         Thank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\n         We hope to sea you soon!");
                        Thread.Sleep(1500);
                        break;
                    default:
                        break;
                }
            }

            // Main Menu Registered Users
            menuText = "Hello, [USERNAME]\n";
            menuOptions = new() { "Current Movies", "Movie Schedule", "My Reservations", "Log Out" };
            while (loggedIn)
            {
                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:  // View all current movies
                        Console.Clear();
                        DisplayMovie.Start();
                        break;
                    case 1:  // View schedule and make reservation
                        ScheduledMovie? selectedMovie = ScheduleHandlerUser.SelectMovieFromSchedule();
                        if (selectedMovie is null)
                            continue;
                        var newAuditorium = new Auditorium_1(selectedMovie);
                        List<Ticket> tickets = newAuditorium.SelectSeats();
                        Checkout.Start(tickets);
                        // TODO Implement payment functionality (run that from within the Auditorium class so tickets list only gets returned if payment was succesful)
                        // add the tickets to the users reservations
                        break;
                    case 2:  // View all reservations
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n  NOT IMPLEMENTEND\n\nPRESS ANY KEY TO GO BACK");
                        Console.ReadKey();
                        break;
                    case 3:  // Log out
                        loggedIn = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}