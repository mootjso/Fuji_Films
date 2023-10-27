public class Theater
{
    public const double BlueSeatPrice = 10;
    public const double YellowSeatPrice = 15;
    public const double RedSeatPrice = 20;

    public int ShowId;
    public int NumOfSeats;
    public List<Seat> Seats = new();

    public Theater(int showId, int theaterNumber)
    {
        ShowId = showId;
        NumOfSeats = theaterNumber == 1 ? 150 : theaterNumber == 2 ? 300 : 500;
        CreateSeats(theaterNumber);
    }

    public void CreateSeats(int theaterNumber)
    {
        int[,] seatArrangement = theaterNumber == 1 ? SeatDatabase.SeatArrangementSmall : theaterNumber == 2 ? SeatDatabase.SeatArrangementMedium : SeatDatabase.SeatArrangementLarge;
        
        int rows = seatArrangement.GetLength(0);
        int columns = seatArrangement.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                switch (seatArrangement[i, j])
                {
                    case 0:
                        Seats.Add(new Seat(i, j, -1, -1, false, true));
                        break;
                    case 1:
                        Seats.Add(new Seat(i, j, BlueSeatPrice, -1, true, true));
                        break;
                    case 2:
                        Seats.Add(new Seat(i, j, YellowSeatPrice, -1, true, true));
                        break;
                    case 3:
                        Seats.Add(new Seat(i, j, RedSeatPrice, -1, true, true));
                        break;
                }
            }
        }
    }
}