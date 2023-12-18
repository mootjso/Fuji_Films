public class Revenue
{
    public int ShowId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public int MonthDate;
    public int YearDate;
    public Revenue(int showId, string movieTitle, double totalRevenue, int monthDate, int yearDate)
    {
        ShowId = showId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
        MonthDate = monthDate;
        YearDate = yearDate;
    }
}
public class RevenueQuartly
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public int QuarterYear;
    public int YearDate;
    public RevenueQuartly(int movieId, string movieTitle, double totalRevenue, int quarterYear, int yearDate)
    {
        MovieId = movieId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
        QuarterYear = quarterYear;
        YearDate = yearDate;
    }
}