public class Reservation
{
    public string ReservationId { get; set; }
    public int UserId { get; set; }
    public int ShowId { get; set; }
    public int MovieId { get; set; }

    public Reservation(string reservationId, int userId, int showId, int movieId)
    {
        ReservationId = reservationId;
        UserId = userId;
        ShowId = showId;
        MovieId = movieId;
    }
}