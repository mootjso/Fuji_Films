using System.Net.Sockets;

public class CheckOutHandler
{
    public const string FileName = "Datasources/revenuePerShow.json";
    public static List<Revenue> Revenues;

    static CheckOutHandler()
    {
        Revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

    }

    public static void AddShowToRevenue()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>(ShowHandler.FileName).ToList();
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

        foreach (var show in shows)
        {
            bool showExists = false;
            foreach (var revenue in revenues)
            {
                if (revenue.ShowId == show.Id)
                {
                    showExists = true;
                    break;
                }
            }

            if (!showExists)
            {
                double totalRevenueUpToNow = GetTotalRevenueUpToNow(show.Id);
                int month = show.DateAndTime.Month;

                Movie? movie = MovieHandler.GetMovieById(show.MovieId)!;
                Revenue? revenue = new Revenue(show.Id, movie.Title, totalRevenueUpToNow, month);

                revenues.Add(revenue);
                JSONMethods.WriteToJSON(revenues, FileName);
            }
        }
    }

    public static double GetTotalRevenueUpToNow(int showId)
    {
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>(TicketHandler.FileName).ToList();
        double revenue = 0;

        foreach (var ticket in tickets)
        {
            if (ticket.ShowId == showId)
                revenue += ticket.Price;
        }
        return revenue;
    }

    public static void AddToExistingRevenue(int showId, double moneyAdded)
    {
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

        foreach (var revenue in revenues)
        {
            if (revenue.ShowId == showId)
            {
                revenue.TotalRevenue += moneyAdded;
                break;
            }
        }
        JSONMethods.WriteToJSON(revenues, FileName);
    }

    public static void CheckOut()
    {
        bool checkOut = true;
        bool confirm = false;

        while (checkOut)
        {
            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please enter your credit card number:\nEXAMPLE 4321-2432-2432-3424\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string creditCardInput = Console.ReadLine();
            Console.ResetColor();
            if (creditCardInput.Length != 19)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Credit card does NOT exist!\nRETRY!\nPlease type it like this (don't forget the '-'): XXXX-XXXX-XXXX-XXXX\n");
                Console.ResetColor();
                Console.WriteLine("Press any button to continue");
                Console.ReadLine();
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please input the experation date:\nEXAMPLE MM/YY, 02/25\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string experationCodeInput = Console.ReadLine();
            if (experationCodeInput.Length != 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong experation code!\nRETRY!\nPlease type it like this (don't forget the '/'): XX/XX\n");
                Console.ResetColor();
                Console.WriteLine("Press any button to continue");
                Console.ReadLine();
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please input the CVC code (on the back):\nEXAMPLE 454\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string cvc = Console.ReadLine();
            Console.ResetColor();
            if (cvc.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Card Verification Code!\nRETRY!\n");
                Console.ResetColor();
                Console.WriteLine("Press any button to continue");
                Console.ReadLine();
                Console.Clear();
                continue;
            }
            confirm = true;

            while (confirm = true)
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();
                Console.WriteLine($"Please confirm the following credit card details:\n\nCredit card number: {creditCardInput}\nExperation date: {experationCodeInput}\nCVC: {cvc}\n\nIs this correct? (Y/N)\n");
                string confirmInput = Console.ReadLine().ToLower();
                if (confirmInput == "y")
                {
                    Console.Clear();
                    Console.ResetColor();
                    DisplayAsciiArt.Header();
                    AdHandler.DisplaySnacks();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tickets successfully booked!\n");
                    Console.ResetColor();
                    Console.WriteLine("Press any button to continue");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                else if (confirmInput == "n")
                {
                    confirm = false;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input, please only input either:\n y or n!\n\nRETRY!\n");
                    Console.ResetColor();
                }
            }
        }
    }
}