public static class Admin
{
    public static void StartMenu()
    {
        string MenuText = "\nWelcome Captain!\n\n\nWhat would you like to do?";
        List<string> MenuOptions = new() {"Financial report", "Change movie description", "Cancel a movie", "Change showtimes", "Take out seat(s)", "Log out"};
        
        while (true)
        {
            int selection = Menu.Start(MenuText, MenuOptions);

            const int FinancialReportOption = 0;
            const int ChangeMovieDescriptionOption = 1;
            const int CancelMovieOption = 2;
            const int ChangeShowtimesOption = 3;
            const int TakeOutSeatsOption = 4;
            const int LogOutOption = 5;

            switch (selection)
            {
                case FinancialReportOption:
                    Console.Clear();
                    Console.WriteLine("\nFINANCIAL REPORT NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;
                case ChangeMovieDescriptionOption:
                    Console.Clear();
                    Console.WriteLine("\nCHANGE MOVIE DESCRIPTION NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;
                case CancelMovieOption:
                    Console.Clear();
                    Console.WriteLine("\nCANCEL A MOVIE NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;
                case ChangeShowtimesOption:
                    Console.Clear();
                    Console.WriteLine("\nCHANGE SHOWTIMES NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;
                case TakeOutSeatsOption:
                    Console.Clear();
                    Console.WriteLine("\nTAKE OUT SEAT(S) NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;
                case LogOutOption:
                    Console.Clear();
                    Console.WriteLine("\nBon Voyage, Captain!");
                    Thread.Sleep(500);
                    Console.WriteLine("\nMay your guidance bring us waves of cinematic success!");
                    Thread.Sleep(2000);
                    break;
                default:
                    break;
            }
        }
    }
}
