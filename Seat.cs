public class Seat
{
    public string PositionName;
    public int Row;
    public int Column;
    public double Price;
    public int UserId;
    public bool IsSeat;
    public bool IsAvailable;

    public Seat(int row, int column, double price, int userId, bool isSeat, bool isAvailable)
    {
        PositionName = $"{Convert.ToChar(row + 'A')}{column + 1}";
        Row = row;
        Column = column;
        Price = price;
        UserId = userId;
        IsSeat = isSeat;
        IsAvailable = isAvailable;
    }
}