public class Seat
{
    public int Row;
    public int Column;
    public double Price;
    public int UserId;
    public bool IsSeat;
    public bool IsAvailable;

    public Seat(int row, int column, double price, int userId, bool isSeat, bool isAvailable)
    {
        Row = row;
        Column = column;
        Price = price;
        UserId = userId;
        IsSeat = isSeat;
        IsAvailable = isAvailable;
    }

    public override string ToString() => $"Row {Row + 1}, Seat {Column + 1}";
}