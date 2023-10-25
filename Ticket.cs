public class Ticket
{
    public int ShowId;
    public int Row;
    public int Column;
    public double Price;
    public string Color;

    public Ticket(Show show, int row, int column, double price, string color)
    {
        ShowId = show.Id;
        Row = row;
        Column = column;
        Price = price;
        Color = color;
    }
}