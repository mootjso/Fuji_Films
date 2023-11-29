public class Revenue
{
    public int ShowId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public Revenue(int showId, string movieTitle, double totalRevenue)
    {
        ShowId = showId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
    }
}