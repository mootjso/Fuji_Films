using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    private static void Main()
    {
        AdminHandler.StartMenu();
        
        while (true)
        {
            string menuText = "Welcome to Ships Cinema!\n\nAre you an existing user or would you like to register a new account?\n";
            List<string> menuOptions = new() { "I am an existing user", "Register a new account", "Exit" };

            bool loggedIn = false;
            while (!loggedIn)
            {
                int selection = Menu.Start(menuText, menuOptions);
                switch (selection)
                {
                    //Login
                    case 0:
                        LoginHandler.LogIn();
                        loggedIn = true;
                        break;
                    //Register
                    case 1:
                        LoginHandler.Register();
                        loggedIn = true;
                        break;
                    case 2:
                        Console.Clear();
                        DisplayAsciiArt.Header();
                        Console.WriteLine("\n\n         Thank you for your visit!");
                        Thread.Sleep(1000);
                        Console.WriteLine("\n         We hope to see you soon!");
                        Thread.Sleep(1500);
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }

            menuText = $"Hello, {LoginHandler.loggedInUser.FirstName} {LoginHandler.loggedInUser.LastName}\n";
            List<string> menuOptionsLoggedIn = new() { "Current Movies", "Show Schedule", "My Reservations", "Log Out" };
            while (loggedIn)
            {
                int selection = Menu.Start(menuText, menuOptionsLoggedIn);
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
                        loggedIn = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
