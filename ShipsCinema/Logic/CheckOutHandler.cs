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

    public static void RevenueQuarterYearIfStatement(Show selectedShow, double moneyAdded, int newTickets)
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
                    revenue.YearDate,
                    newTickets
                );
                quarterYearRevenues.Add(newRevenueQuarter);
            }
            else
            {
                if (selectedShow.Id == revenue.ShowId)
                {
                    existingQuarterRevenue.TotalRevenue += moneyAdded;
                    existingQuarterRevenue.TicketAmount += newTickets;
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

    public static void WriteHeaders()
    {
        // Headers
        Console.Clear();
        Console.ResetColor();
        DisplayAsciiArt.Header();
        AdHandler.DisplaySnacks();
        Console.WriteLine("Checkout");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nEnter 'q' at any of the prompts to cancel this reservation.");
        Console.WriteLine("Enter 'r' to start over with your payment information.");
        Console.ResetColor();
    }

    public static void WriteBookingInfo(Movie movie, Show show, string seatsInfo)
    {
        // Headers
        Console.Clear();
        Console.ResetColor();
        DisplayAsciiArt.Header();
        AdHandler.DisplaySnacks();
        Console.WriteLine("Please confirm your booking information\n");
        
        // Movie title
        Console.Write("Movie: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(movie.Title);
        Console.ResetColor();
        
        // Theater
        Console.Write("Theater: ");
        string theaterString = show.TheaterNumber == 1 ? "Small (150 seats)" : show.TheaterNumber == 2 ? "Medium (300 seats)" : "Large (500 seats)";
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(theaterString);
        Console.ResetColor();
        
        // Date
        Console.Write("Date: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(show.DateAndTime.ToString("dd-MM-yyyy"));
        Console.ResetColor();
        
        // Time
        Console.Write("\nTime: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{show.StartTimeString} - {show.EndTimeString}");
        Console.ResetColor();

        // Seat info
        Console.Write("\nSelected seats: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(seatsInfo);
        Console.ResetColor();
    }

    public static void WritePaymentInformation(string ccNumber, string expDate, string cvcCode)
    {
        Console.Clear();
        Console.Clear();
        Console.ResetColor();
        DisplayAsciiArt.Header();
        AdHandler.DisplaySnacks();
        Console.WriteLine("Please confirm your payment details\n");
        // Credit card number
        Console.Write("Credit card number: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(ccNumber);
        Console.ResetColor();

        // Expiration Date
        Console.Write("\nExpiration date: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(expDate);
        Console.ResetColor();

        // CVC Code
        Console.Write("\nCVC code: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(cvcCode);
        Console.ResetColor();
    }

    public static bool ConfirmBookingInformation(Movie movie, Show show, string seatsInfo)
    {
        WriteHeaders();
        WriteBookingInfo(movie, show, seatsInfo);
        Console.WriteLine($"\n\nIs this information correct?\n[Y] Yes, continue to checkout\n[N] No, go back to seat selection\n");
        ConsoleKey pressedKey = Console.ReadKey().Key;
        if (pressedKey == ConsoleKey.Y)
            return true;
        
        return false;
    }
    
    public static (bool bookingCorrect, bool paymentConfirmed) CheckOut(Show selectedShow, List<Seat> seats)
    {
        // Error checking
        Movie? selectedMovie = null;
        if (selectedShow != null)
            selectedMovie = MovieHandler.GetMovieById(selectedShow.MovieId);
        if (selectedMovie == null || selectedShow == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong, please contact the Movie Theater staff.");
            Console.ResetColor();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
            return (false, false);
        }

        string seatsString = string.Empty;
        seats.ForEach(s => seatsString += s + "; ");
        // Confirm Booking information
        bool bookingCorrect = ConfirmBookingInformation(selectedMovie, selectedShow, seatsString);
        if (!bookingCorrect)
        {
            return (false, false);
        }

        bool checkOut = true;
        bool confirm = false;
        string? creditCardInput = string.Empty, expirationDate = string.Empty, cvcCode = string.Empty;

        Console.CursorVisible = true;
        while (checkOut)
        {
            // Credit card number
            if (creditCardInput == string.Empty)
            {
                WriteHeaders();
                Console.WriteLine("Please enter your credit card number:\nExample: 4321-2432-2432-3424");
                Console.ForegroundColor = Program.InputColor;
                Console.CursorVisible = true;
                creditCardInput = Console.ReadLine();
                Console.CursorVisible = false;
                creditCardInput ??= string.Empty;
                Console.ResetColor();
            }
            if (creditCardInput == "q")
                return (true, false);
            if (creditCardInput == "r")
            {
                creditCardInput = string.Empty;
                continue;
            }

            bool correctCardFormat = ValidateCreditCardNumber.IsValid(creditCardInput);
            if (!correctCardFormat)
            {
                creditCardInput = string.Empty;
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCredit card does NOT exist!\nPlease enter the following format including the '-': XXXX-XXXX-XXXX-XXXX");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadKey();
                continue;
            }

            // Expiration date
            if (expirationDate == string.Empty)
            {
                WriteHeaders();
                Console.WriteLine("Please input the expiration date:\nRequired format: MM/YY");
                Console.ForegroundColor = Program.InputColor;
                Console.CursorVisible = true;
                expirationDate = Console.ReadLine();
                Console.CursorVisible = false;
                expirationDate ??= string.Empty;
            }
            if (expirationDate == "q")
                return (true, false);
            if (expirationDate == "r")
            {
                creditCardInput = string.Empty;
                expirationDate = string.Empty;
                continue;
            }

            (bool validFormat, bool isExpired) = ValidateExpirationDate.IsExpirationDateValid(expirationDate);

            if (!validFormat)
            {
                expirationDate = string.Empty;
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nIncorrect input.\nPlease enter the following format: MM/YY");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadKey();
                continue;
            }
            else if (isExpired)
            {
                expirationDate = string.Empty;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis card is expired, please use a different credit card.");
                Console.ResetColor();
                Console.WriteLine("Press any button to enter a new credit card number");
                creditCardInput = string.Empty;
                Console.ReadKey();
                Console.CursorVisible = true;
                continue;
            }

            // CVC code
            if (cvcCode == string.Empty)
            {
                WriteHeaders();
                Console.WriteLine("Please input the CVC code (3 numbers on the back of the card):\nExample: 454");
                Console.ForegroundColor = Program.InputColor;
                Console.CursorVisible = true;
                cvcCode = Console.ReadLine()?.ToLower();
                Console.ResetColor();
                Console.CursorVisible = false;
                cvcCode ??= string.Empty;
            }
            if (cvcCode == "q")
            {
                return (true, false);
            }
            if (cvcCode == "r")
            {
                creditCardInput = string.Empty;
                expirationDate = string.Empty;
                cvcCode = string.Empty;
                continue;
            }

            bool CvcIsAllNums = cvcCode.All(num => Char.IsDigit(num));
            if (cvcCode.Length != 3 || !CvcIsAllNums)
            {
                cvcCode = string.Empty;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nWrong Card Verification Code, please enter only the three numbers of your CVC code (e.g.: 454)");
                Console.ResetColor();
                Console.WriteLine("Press any button to try again");
                Console.ReadKey();
                Console.Clear();
                Console.CursorVisible = true;
                continue;
            }

            // Confirm payment information
            Console.CursorVisible = false;
            confirm = true;
            while (confirm == true)
            {
                WritePaymentInformation(creditCardInput, expirationDate, cvcCode);
                Console.WriteLine($"\n\nIs this information correct?\n[Y] Yes, this is correct\n[N] No, re-enter my info\n");
                ConsoleKey pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.Y)
                {
                    Console.Clear();
                    Console.ResetColor();
                    DisplayAsciiArt.Header();
                    AdHandler.DisplaySnacks();
                    Console.WriteLine("Checkout Confirmed\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tickets successfully booked!");
                    Console.ResetColor();
                    Console.WriteLine("\nYou can find your tickets and booking information under 'My Reservations'.");
                    Console.WriteLine("Press any button to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return (true, true);
                }
                else if (pressedKey == ConsoleKey.N)
                {
                    creditCardInput = string.Empty;
                    expirationDate = string.Empty;
                    cvcCode = string.Empty;
                    confirm = false;
                    break;
                }
            }
        }

        return (false, false);
    }
}
