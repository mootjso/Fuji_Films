using System.Net.Sockets;

public class CheckOutHandler
{
    public const string FileName = "Datasources/revenuePerShow.json";
    public static List<Revenue> Revenue;

    static CheckOutHandler()
    {
        Revenue = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

    }

    public static void AddShowToRevenue()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>("shows.json").ToList();
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>("revenuePerShow.json").ToList();

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
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>("tickets.json").ToList();
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
        while(true)
        {
            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Please enter your credit card number:\nEXAMPLE 4321-2432-2432\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            string creditCardInput = Console.ReadLine();
            if (creditCardInput.Contains("-"))
            {
                creditCardInput = $"{creditCardInput.Substring(0, 4)}{creditCardInput.Substring(5, 4)}{creditCardInput.Substring(10, 4)}";
            }
            if (creditCardInput.Length != 12)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Credit card does NOT exist!\nRETRY!\n");
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
            if (experationCodeInput.Contains("/"))
            {
                experationCodeInput = $"{experationCodeInput.Substring(0, 2)}{experationCodeInput.Substring(3, 2)}";
            }
            if (experationCodeInput.Length != 4)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong experation code!\nRETRY!\n");
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
            if (cvc.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Card Verification Code!\nRETRY!\n");
                Console.WriteLine("Press any button to continue");
                Console.ReadLine();
                Console.Clear();
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine("Tickets successfully booked!\n");
            Console.WriteLine("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            return;
        }
    }
}