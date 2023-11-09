using System.Diagnostics;
public class Reservation
{
    public string ReservationID;
    public int UserID;
    public static List<Ticket> tickets;

    public Reservation()
    {
        ReservationID = GetReservationID();
    }

    public static string GetReservationID()
    {
        Guid ReservationID = Guid.NewGuid();
        string ReservationIDString = ReservationID.ToString();

        Debug.WriteLine("Your UUID is: " + ReservationIDString);
        return ReservationIDString;
    }
}
