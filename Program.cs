public class Program
{
    private static void Main()
    {
        while (true)
        {
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };

            UserType userType = UserType.InvalidUser;
            while (userType == UserType.InvalidUser)
            {
                DisplayAsciiArt.Standby();

                userType = LoginHandler.LogIn();
                switch (userType)
                {
                    case UserType.RegularUser:
                        break;
                    case UserType.AdminUser:
                        AdminHandler.StartMenu();
                        break;
                    case UserType.InvalidUser:
                        break;
                    default:
                        break;
                }
            }

            string userGreeting = userType == UserType.RegularUser
                ? $"Hello, {LoginHandler.loggedInUser.FirstName} {LoginHandler.loggedInUser.LastName}\n"
                : "Hello, Admin!\n";

            List<string> menuOptionsLoggedIn = new() { "Current Movies", "Show Schedule", "My Reservations", "Log Out" };
            while (userType == UserType.RegularUser)
            {
                int selection = Menu.Start(userGreeting, menuOptionsLoggedIn);
                switch (selection)
                {
                    case 0:
                        Console.Clear();
                        MovieHandler.ViewCurrentMovies();
                        break;
                    case 1:
                        Show? selectedShow = ShowHandler.SelectShowFromSchedule();
                        if (selectedShow is null)
                            continue;

                        var theater = TheaterHandler.CreateTheater(selectedShow);

                        List<Ticket>? tickets = TheaterHandler.SelectSeats(LoginHandler.loggedInUser, theater);
                        if (tickets is null)
                            continue;

                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\nCHECKOUT FUNCTIONALITY NOT IMPLEMENTED\n\nPRESS ANY KEY TO GO BACK");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n  NOT IMPLEMENTED\n\nPRESS ANY KEY TO GO BACK");
                        Console.ReadKey();
                        break;
                    case 3:
                        userType = UserType.InvalidUser;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
