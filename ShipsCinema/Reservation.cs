using System.Data.Common;

public class Reservation
{
    public string ReservationId { get; set; }
    public int UserId { get; set; }
    public int ShowId { get; set; }
    public int MovieId { get; set; }
    public int Row {  get; set; }
    public int Column { get; set; }

    public Reservation(string reservationId, int userId, int showId, int movieId, int row, int column)
    {
        ReservationId = reservationId;
        UserId = userId;
        ShowId = showId;
        MovieId = movieId;
        Row = row;
        Column = column;
    }
}