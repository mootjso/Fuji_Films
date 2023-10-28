public static class Admin
{
    public static void StartMenu()
    {   
        string MenuText = $"\nWelcome Captain!\n\nWhat would you like to do?";
        
        List<string> MenuOptions = new() {"Financial report", "Add/Remove movie", "Change movie description", "Take out seat(s)", "Log out"};
        
        while (true)
        {   
            int selection = AdminMenu.Start(MenuText, MenuOptions);

            const int FinancialReportOption = 0;
            const int AddRemoveMovieOption = 1;
            const int ChangeMovieDescriptionOption = 2;
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
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine("\nAdd movie NOT IMPLEMENTED\n\nPRESS ANY KEY TO CONTINUE TO THE MAIN MENU");
                    Console.ReadKey();
                    break;       
                case ChangeMovieDescriptionOption:
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    ScheduleHandlerAdmin.EditMovieDescription();
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
}
