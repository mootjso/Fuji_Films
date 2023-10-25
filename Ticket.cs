public class Ticket
{
    // TODO Make sure the Id is set appropriately
    public int ShowId;
    public int Row;
    public int Column;
    public double Price;
    public string Color;

    public Ticket(Show movie, int row, int column, double price, string color)
    {
        ShowId = movie.Id;
        Row = row;
        Column = column;
        Price = price;
        Color = color;
    }
}