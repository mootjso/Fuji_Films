using System.Net.Sockets;

public class CheckOutHandler
{
    public const string FileName = "revenuePerShow.json";
    public static List<Revenue> Revenue;

    static CheckOutHandler()
    {
        Revenue = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

    }

    public static void AddShowToRevenue()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>("shows.json").ToList();
        List<Revenue> revenues = JSONMethods.ReadJSON<Revenue>("revenuePerShow.json").ToList();

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

                Movie? movie = MovieHandler.GetMovieById(show.Id)!;
                Revenue? revenue = new Revenue(show.Id, movie.Title, totalRevenueUpToNow);

                revenues.Add(revenue);
                JSONMethods.WriteToJSON(revenues, FileName);
            }
        }
    }

    public static double GetTotalRevenueUpToNow(int showId)
    {
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>("tickets.json").ToList();
        double revenue = 0;

        foreach (var ticket in tickets)
        {
            if (ticket.ShowId == showId)
                revenue += ticket.Price;
        }
        return revenue;
    }

    public static void CheckOut()
    {

    }
}