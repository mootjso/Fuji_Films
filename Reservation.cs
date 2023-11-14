public class Reservation
{
    public string ReservationId { get; set; }
    public int UserId { get; set; }
    public int TicketId { get; set; }
    public int MovieId { get; set; }

    public Reservation(string reservationId, int userId, int ticketId, int movieId)
    {
        ReservationId = reservationId;
        UserId = userId;
        TicketId = ticketId;
        MovieId = movieId;
    }
}