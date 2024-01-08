public static class FinancialHandler
{
    public static List<string> GetYears()
    {
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>(CheckOutHandler.FileName).ToList();
        List<Revenue> otherRevenues = JSONMethods.ReadJSON<Revenue>(CheckOutHandler.FileQuarterYearName).ToList();
        revenues.AddRange(otherRevenues);
        var years = revenues.Select(r => r.YearDate).Distinct().OrderDescending().ToList();
        var yearsAsInt = years.Select(r => r.ToString()).ToList();
        
        return yearsAsInt;
    }

    public static bool CSVCreater(string year, int quarter, string infoBy)
    {
        int yearAsInt = Int32.Parse(year);

        List<RevenueQuartly> revenueQuarterData = JSONMethods.ReadJSON<RevenueQuartly>(CheckOutHandler.FileQuarterYearName).ToList();
        List<Revenue> revenueData = JSONMethods.ReadJSON<Revenue>(CheckOutHandler.FileName).ToList();

        string fileName = $"{year}-q{quarter}-{infoBy}.csv";
        string directoryPath = "FinancialReports";
        if (!Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, fileName);

        try
        {
            using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)))
            {
                if (infoBy == "byMovie")
                {
                    writer.WriteLine("Year;Quarter of the Year;Movie Title;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets");

                    foreach (var data in revenueQuarterData)
                    {
                        if (quarter == data.QuarterYear)
                        {
                            if (year == $"{data.YearDate}")
                            {
                                int ticketAmount = GetTicketAmountForMovie(yearAsInt, quarter, data.MovieId);

                                double revenueWithoutBTW = data.TotalRevenue / 1.09;
                                var line = $"{data.YearDate};{data.QuarterYear};{data.MovieTitle};€{data.TotalRevenue.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{ticketAmount}";

                                writer.WriteLine(line);

                            }
                        }
                    }
                }
                else if (infoBy == "byTheater")
                {
                    writer.WriteLine("Theater number;Year;Quarter;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets");

                    double revenueBTW1 = 0;
                    double revenueWithoutBTW1 = 0;
                    int TicketAmount1 = 0;

                    double revenueBTW2 = 0;
                    double revenueWithoutBTW2 = 0;
                    int TicketAmount2 = 0;

                    double revenueBTW3 = 0;
                    double revenueWithoutBTW3 = 0;
                    int TicketAmount3 = 0;

                    // Determine revenue per theater
                    foreach (var data in revenueData)
                    {
                        int QuarterCheck = CheckOutHandler.DetermineQuarter(data.MonthDate);

                        if (quarter == QuarterCheck)
                        {
                            if (year == $"{data.YearDate}")
                            {
                                Show? show = ShowHandler.GetShowById(data.ShowId);

                                switch (show.TheaterNumber)
                                {
                                    case 1:
                                        revenueBTW1 += data.TotalRevenue;
                                        revenueWithoutBTW1 = data.TotalRevenue / 1.09;
                                        break;
                                    case 2:
                                        revenueBTW2 += data.TotalRevenue;
                                        revenueWithoutBTW2 = data.TotalRevenue / 1.09;
                                        break;
                                    case 3:
                                        revenueBTW3 += data.TotalRevenue;
                                        revenueWithoutBTW3 = data.TotalRevenue / 1.09;
                                        break;
                                }
                            }
                        }
                    }

                    // Determine amount of tickets per theater
                    TicketAmount1 = GetTicketAmountForTheater(yearAsInt, quarter, 1);
                    TicketAmount2 = GetTicketAmountForTheater(yearAsInt, quarter, 2);
                    TicketAmount3 = GetTicketAmountForTheater(yearAsInt, quarter, 3);

                    var line1 = $"{1};{year};{quarter};€{revenueBTW1.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW1.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount1}";
                    var line2 = $"{2};{year};{quarter};€{revenueBTW2.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW2.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount2}";
                    var line3 = $"{3};{year};{quarter};€{revenueBTW3.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW3.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount3}";

                    writer.WriteLine(line1);
                    writer.WriteLine(line2);
                    writer.WriteLine(line3);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Financial Reports\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please close the Excel file before trying to update it.");
            Console.ResetColor();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
        return false;
    }

    private static bool IsInQuarter(int year, int quarter, DateTime date)
    {
        DateTime quarterStart = new DateTime(year, (quarter - 1) * 3 + 1, 1);
        DateTime quarterEnd = quarterStart.AddMonths(3).AddDays(-1);

        return date >= quarterStart && date <= quarterEnd;
    }

    private static int GetTicketAmountForTheater(int year, int quarter, int theaterNum)
    {
        var showsInTheaterOne = ShowHandler.Shows
                        .Where(s => s.TheaterNumber == theaterNum && IsInQuarter(year, quarter, s.DateAndTime))
                        .Select(s => s.Id).ToList();

        return TicketHandler.Tickets
            .Where(t => showsInTheaterOne.Contains(t.ShowId))
            .Count();
    }

    private static int GetTicketAmountForMovie(int year, int quarter, int movieId)
    {
        var showIdsForMovie = ShowHandler.Shows
                                    .Where(s => IsInQuarter(year, quarter, s.DateAndTime) && s.MovieId == movieId)
                                    .Select(s => s.Id).ToList();

        return TicketHandler.Tickets
            .Where(t => showIdsForMovie.Contains(t.ShowId))
            .Count();
    }
}