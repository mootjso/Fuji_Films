public class Ticket
{
    // TODO Make sure the Id is set appropriately
    public int ScheduledMovieId = 0;
    public string Position;
    public double Price;
    public string Color;

    public Ticket(string position, double price, string color)
    {
        Position = position;
        Price = price;
        Color = color;
    }
}