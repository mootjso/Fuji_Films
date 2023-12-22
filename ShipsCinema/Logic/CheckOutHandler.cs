using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;

public class CheckOutHandler
{
    public const string FileName = "Datasources/revenuePerShow.json";
    public const string FileQuarterYearName = "Datasources/revenuePerQuarterYear.json";
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
                int year = show.DateAndTime.Year;

                Movie? movie = MovieHandler.GetMovieById(show.MovieId)!;
                Revenue? revenue = new Revenue(
                    show.Id,
                    movie.Title,
                    totalRevenueUpToNow,
                    month,
                    year
                );

                revenues.Add(revenue);
                JSONMethods.WriteToJSON(revenues, FileName);
            }
        }
    }

    public static void RevenueQuarterYearIfStatement(Ticket ticket, double moneyAdded)
    {
        List<Revenue> revenuesPerShow = JSONMethods.ReadJSON<Revenue>(FileName).ToList();
        List<RevenueQuartly> quarterYearRevenues = JSONMethods
            .ReadJSON<RevenueQuartly>(FileQuarterYearName)
            .ToList();

        foreach (var revenue in revenuesPerShow)
        {
            int quarter = DetermineQuarter(revenue.MonthDate);
            var existingQuarterRevenue = quarterYearRevenues.FirstOrDefault(
                qr =>
                    qr.MovieTitle == revenue.MovieTitle
                    && qr.YearDate == revenue.YearDate
                    && qr.QuarterYear == quarter
            );

            if (existingQuarterRevenue == null)
            {
                Show? show = ShowHandler.GetShowById(revenue.ShowId);
                Movie? movie = MovieHandler.GetMovieById(show.MovieId);

                RevenueQuartly newRevenueQuarter = new RevenueQuartly(
                    movie.Id,
                    movie.Title,
                    revenue.TotalRevenue,
                    quarter,
                    revenue.YearDate
                );
                quarterYearRevenues.Add(newRevenueQuarter);
            }
            else
            {
                if (ticket.ShowId == revenue.ShowId)
                {
                    existingQuarterRevenue.TotalRevenue += moneyAdded;
                }
            }
        }
        JSONMethods.WriteToJSON(quarterYearRevenues, FileQuarterYearName);
    }

    public static int DetermineQuarter(int month)
    {
        if (month <= 3)
        {
            return 1;
        }
        else if (month <= 6)
        {
            return 2;
        }
        else if (month <= 9)
        {
            return 3;
        }
        else if (month <= 12)
        {
            return 4;
        }
        else
        {
            return 0;
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

    public static bool CheckOut()
    {
        bool checkOut = true;
        bool confirm = false;

        Console.CursorVisible = true;
        while (checkOut)
        {
            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine(
                "Please enter your credit card number:\nEXAMPLE: 4321-2432-2432-3424"
            );
            Console.ForegroundColor = ConsoleColor.Blue;
            string creditCardInput = Console.ReadLine();
            Console.ResetColor();

            bool correctCardFormat = creditCardInput.All(num => Char.IsDigit(num) || num == '-');

            if (creditCardInput.Length != 19 || !correctCardFormat)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Credit card does NOT exist!\nPlease try again\nEnter the following format including the '-': XXXX-XXXX-XXXX-XXXX"
                );
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.CursorVisible = true;
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine(
                "Please input the expiration date:\nRequired format: MM/YY, Example: 02/25"
            );
            Console.ForegroundColor = ConsoleColor.Blue;
            string experationCodeInput = Console.ReadLine();

            bool correctExperationFormat = experationCodeInput.All(
                num => Char.IsDigit(num) || num == '/'
            );

            if (experationCodeInput.Length != 5 || !correctExperationFormat)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Incorrect format, please try again.\nEnter the following format including the '/': XX/XX"
                );
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.CursorVisible = true;
                continue;
            }

            Console.Clear();
            Console.ResetColor();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.WriteLine(
                "Please input the CVC code (3 numbers on the back of the card):\nEXAMPLE: 454"
            );
            Console.ForegroundColor = ConsoleColor.Blue;
            string cvc = Console.ReadLine();
            Console.ResetColor();

            bool correctCVCFormat = cvc.All(num => Char.IsDigit(num));

            if (cvc.Length != 3 || !correctCVCFormat)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Wrong Card Verification Code, please use the correct format (e.g.: 454)"
                );
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadLine();
                Console.Clear();
                Console.CursorVisible = true;
                continue;
            }

            Console.CursorVisible = false;
            confirm = true;
            while (confirm == true)
            {
                Console.Clear();
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();
                Console.WriteLine(
                    $"Please confirm the following credit card details:\n\nCredit card number: {creditCardInput}\nExpiration date: {experationCodeInput}\nCVC: {cvc}\n\nIs this correct? (Y/N)\n"
                );
                ConsoleKey pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.Y)
                {
                    Console.Clear();
                    Console.ResetColor();
                    DisplayAsciiArt.Header();
                    AdHandler.DisplaySnacks();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tickets successfully booked!\n");
                    Console.ResetColor();
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return true;
                }
                else if (pressedKey == ConsoleKey.N)
                {
                    confirm = false;
                    break;
                }
            }
        }

        return false;
    }
}
