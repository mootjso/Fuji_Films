public class Revenue
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public double TotalRevenue;
    public Revenue(int movieId, string movieTitle, double totalRevenue)
    {
        MovieId = movieId;
        MovieTitle = movieTitle;
        TotalRevenue = totalRevenue;
    }
}