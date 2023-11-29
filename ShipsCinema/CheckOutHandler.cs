using System.Net.Sockets;

public class CheckOutHandler
{
    public const string FileName = "revenuePerShow.json";
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

                Movie? movie = MovieHandler.GetMovieById(show.Id)!;
                Revenue? revenue = new Revenue(show.Id, movie.Title, totalRevenueUpToNow);

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

    public static bool CheckOut()
    {
        AdHandler.DisplaySnacks();
        Console.WriteLine("Please enter your credit card number:\nEXAMPLE 4321-2432-2432\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        string creditCardInput = Console.ReadLine();
        if (creditCardInput.Contains("-"));
            string creditCard = $"{creditCardInput.Substring(0,4)}{creditCardInput.Substring(5,4)}{creditCardInput.Substring(10,4)}";

        if (creditCard.Length != 12)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Credit card does NOT exist!\nTRANSACTION CANCELLED\n");
            Console.WriteLine("Press any button to continue");
            Console.ReadLine();
            Console.Clear();
            return false;
        }


        Console.WriteLine("Please input the experation date:\nEXAMPLE MM/YY, 02/25\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        string experationCodeInput = Console.ReadLine();
        if (experationCodeInput.Contains("/"));
            string experationCode = $"{experationCodeInput.Substring(0,2)}{experationCodeInput.Substring(3,2)}";

        Console.WriteLine("Please input the CVC code (on the back):\nEXAMPLE 454\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        string CVC = Console.ReadLine();
    }
}