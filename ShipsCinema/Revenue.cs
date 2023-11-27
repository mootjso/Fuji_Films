public class Revenue
{
    public int ShowId { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public Revenue(int showId, int movieId, string movieTitle, double totalRevenue)
    {
        ShowId = showId;
        MovieId = movieId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
    }
}