using System.Diagnostics;
public class Reservation
{
    public string ReservationId;

    public Reservation()
    {
        ReservationId = GetReservationID();
    }

    public static string GetReservationID()
    {
        Guid ReservationGUID = Guid.NewGuid();
        // Dit zorgt er voor dat de GUID geen "-" heeft zodat ik dit hieronder zelf kan toevoegen.
        string ReservationID = ReservationGUID.ToString("N");
        // Voorbeeld van reservation code: FD8J-FJN8-A4FX
        string ReservationIDString = $"{ReservationID.Substring(0, 4)}-{ReservationID.Substring(4, 4)}-{ReservationID.Substring(8, 4)}";


        Debug.WriteLine("Your Reservation code is: " + ReservationIDString);
        return ReservationIDString;
    }

    // public static void GetReservations()
    // {
    //     List<Ticket> AllTickets = JSONMethods.ReadJSON<Ticket>(JSONMethods.TicketHandler.FileName).ToList();

    // }

}
