using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class FinancialHandler
{
    public static List<string> GetYears()
    {
        // TO BE IMPLEMENTED, GETTING ALL THE YEARS THAT THERE IS DATA FOR
        return new List<string> { "2024" };
    }

    public static bool CSVCreater(string year, int quarter, string infoBy)
    {
        List<RevenueQuartly> revenueQuarterData = JSONMethods.ReadJSON<RevenueQuartly>(CheckOutHandler.FileQuarterYearName).ToList();
        List<Revenue> revenueData = JSONMethods.ReadJSON<Revenue>(CheckOutHandler.FileName).ToList();

        string fileName = $"{year}-q{quarter}-{infoBy}.csv";
        string directoryPath = @"..\..\..\..\FinancialReports";
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
                    writer.WriteLine("Movie Title;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets;Year;Quarter of the Year;");

                    foreach (var data in revenueQuarterData)
                    {
                        if (quarter == data.QuarterYear)
                        {
                            if (year == $"{data.YearDate}")
                            {
                                double revenueWithoutBTW = data.TotalRevenue / 1.09;
                                var line = $"{data.MovieTitle};€{data.TotalRevenue.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{data.TicketAmount};{data.YearDate};{data.QuarterYear}";

                                writer.WriteLine(line);

                            }
                        }
                    }
                }
                else if (infoBy == "byTheater")
                {
                    writer.WriteLine("Theater number;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets;Year;Quarter of the Year;");

                    double revenueBTW1 = 0;
                    double revenueWithoutBTW1 = 0;
                    int TicketAmount1 = 0;

                    double revenueBTW2 = 0;
                    double revenueWithoutBTW2 = 0;
                    int TicketAmount2 = 0;

                    double revenueBTW3 = 0;
                    double revenueWithoutBTW3 = 0;
                    int TicketAmount3 = 0;

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

                                foreach (var revenue in revenueQuarterData)
                                {
                                    if (quarter == revenue.QuarterYear)
                                    {
                                        if (year == $"{data.YearDate}")
                                        {
                                            switch (show.TheaterNumber)
                                            {
                                                case 1:
                                                    TicketAmount1 += revenue.TicketAmount;
                                                    break;
                                                case 2:
                                                    TicketAmount2 += revenue.TicketAmount;
                                                    break;
                                                case 3:
                                                    TicketAmount3 += revenue.TicketAmount;
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var line1 = $"{1};€{revenueBTW1.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW1.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount1};{year};{quarter}";
                    var line2 = $"{2};€{revenueBTW2.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW2.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount2};{year};{quarter}";
                    var line3 = $"{3};€{revenueBTW3.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};€{revenueWithoutBTW3.ToString("N2", CultureInfo.GetCultureInfo("nl-NL"))};{TicketAmount3};{year};{quarter}";

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
}