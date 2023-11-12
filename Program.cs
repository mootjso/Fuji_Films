using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    private static void Main()
    {
        while (true)
        {
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };

            User loggedInUser = null;
            while (loggedInUser == null)
            {
                DisplayAsciiArt.Standby();

                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    case 0:
                        loggedInUser = LoginHandler.LogIn();

                        if (loggedInUser.IsAdmin)
                        {
                            AdminHandler.StartMenu();
                            return; // Admin Shutdown
                        }
                        break;
                    case 1:
                        loggedInUser = LoginHandler.Register();
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\nThank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\nWe hope to see you soon!");
                        Thread.Sleep(1500);
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }

            string userGreeting = loggedInUser.IsAdmin
                ? "Hello, Admin!\n"
                : $"Hello, {loggedInUser.FirstName} {loggedInUser.LastName}\n";

            List<string> menuOptionsLoggedIn = new() { "Current Movies", "Show Schedule", "My Reservations", "Log Out" };
            while (loggedInUser != null)
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

                        List<Ticket>? tickets = TheaterHandler.SelectSeats(loggedInUser, theater);
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
                        loggedInUser = null; // Log out the user
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
