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
                DisplayAsciiArt.Standby();
                DisplayAsciiArt.OpeningLogo();

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
                    case 0:
                        Console.Clear();
                        DisplayMovie.Start();
                        break;
                    case 1:
                        Console.Clear();
                        MovieScheduleHandlerUser.Start();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n  NOT IMPLEMENTEND\n\nPRESS ANY KEY TO GO BACK");
                        Console.ReadKey();
                        break;
                    case 3:
                        loggedIn = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}