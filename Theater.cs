public class Theater
{
    public const double BlueSeatPrice = 10;
    public const double YellowSeatPrice = 15;
    public const double RedSeatPrice = 20;

    public int ShowId;
    public List<Seat> Seats = new();

    public Theater(int showId)
    {
        ShowId = showId;
        CreateTickets();
    }

    public void CreateTickets()
    {
        int rows = SeatDatabase.SeatArrangement.GetLength(0);
        int columns = SeatDatabase.SeatArrangement.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                switch (SeatDatabase.SeatArrangement[i, j])
                {
                    case 0:
                        Seats.Add(new Seat(i, j, -1, -1, false));
                        break;
                    case 1:
                        Seats.Add(new Seat(i, j, BlueSeatPrice, -1, true));
                        break;
                    case 2:
                        Seats.Add(new Seat(i, j, YellowSeatPrice, -1, true));
                        break;
                    case 3:
                        Seats.Add(new Seat(i, j, RedSeatPrice, -1, true));
                        break;
                }
            }
        }
    }
}