public class Ticket
{
    public int ShowId;
    public int UserId;
    public int Row;
    public int Column;
    public double Price;
    public string Color;

    public Ticket(int showId, int userId, int row, int column, double price, string color)
    {
        ShowId = showId;
        UserId = userId;
        Row = row;
        Column = column;
        Price = price;
        Color = color;
    }
}