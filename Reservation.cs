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
        Guid ReservationGUID = Guid.NewGuid();
        // Dit zorgt er voor dat de GUID geen "-" heeft zodat ik dit hieronder zelf kan toevoegen.
        string ReservationID = ReservationGUID.ToString("N");
        string ReservationIDString = $"{ReservationID.Substring(0, 4)}-{ReservationID.Substring(4, 4)}-{ReservationID.Substring(8, 4)}";


        Debug.WriteLine("Your Reservation code is: " + ReservationIDString);
        return ReservationIDString;
    }
}
