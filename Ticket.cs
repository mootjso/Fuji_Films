public class Ticket
{
    // TODO Make sure the Id is set appropriately
    public ScheduledMovie MovieInSchedule;
    public string Position;
    public double Price;
    public string Color;

    public Ticket(ScheduledMovie movie, string position, double price, string color)
    {
        MovieInSchedule = movie;
        Position = position;
        Price = price;
        Color = color;
    }
}