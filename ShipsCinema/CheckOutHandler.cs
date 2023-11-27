public class CheckOutHandler
{
    public const string FileName = "revenuePerMovie.json";
    public static List<Revenue> Revenue;
    static CheckOutHandler()
    {
        Revenue = JSONMethods.ReadJSON<Revenue>(FileName).ToList();

    }
    // per show bij houden.
    // dan kunnen we per tijd bekijken.

    // ook per movie


}
