public class Revenue
{
    public int ShowId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public int MonthDate;
    public Revenue(int showId, string movieTitle, double totalRevenue, int monthDate)
    {
        ShowId = showId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
        MonthDate = monthDate;
    }
}