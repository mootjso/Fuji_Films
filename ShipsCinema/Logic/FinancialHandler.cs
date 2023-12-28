using System.Globalization;

public static class FinancialHandler
{
    public static List<string> GetYears()
    {
        // TO BE IMPLEMENTED
        return new List<string> { "2023", "2024" };
    }

    public static void CSVCreater(string year, string quarter, string infoBy)
    {
        List<RevenueQuartly> revenueData = JSONMethods.ReadJSON<RevenueQuartly>(CheckOutHandler.FileQuarterYearName).ToList();

        string fileName = $"{year}-{quarter}-{infoBy}.csv";
        string directoryPath = @"..\..\..\..\FinancialReports";

        string filePath = Path.Combine(directoryPath, fileName);
        using (var writer = new StreamWriter(filePath))
        {
            if (infoBy == "byMovie")
            {
                writer.WriteLine("Movie Title;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets;Year;Quarter of the Year;");

                foreach (var data in revenueData)
                {
                    double revenueWithoutBTW = data.TotalRevenue / 1.09;
                    var line = $"{data.MovieTitle};€{data.TotalRevenue};€{revenueWithoutBTW};{data.TicketAmount};{data.YearDate};{data.QuarterYear}";


                    writer.WriteLine(line);
                }
            }
            else if (infoBy == "byTheater")
            {
                writer.WriteLine("Theater number;Revenue (incl. btw);Revenue (excl. btw);Total Amount Tickets;Year;Quarter of the Year;");

                foreach (var data in revenueData)
                {
                    //double revenueWithoutBTW = data.TotalRevenue / 1.09;
                    //var line = $"{data.MovieTitle};€{data.TotalRevenue};{revenueWithoutBTW};{data.TicketAmount};{data.YearDate};{data.QuarterYear}";

                    //writer.WriteLine(line);
                }
            }
        }
    }
}