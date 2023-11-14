using System.Diagnostics;
public static class ReservationHandler
{
    public const string FileName = "reservations.json";
    public static List<Reservation> Reservations;

    static ReservationHandler()
    {
        Reservations = AppInitializer.GetReservationObjects();

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

    public static List<Reservation> GetReservationsByUser(User user)
    {
        var reservationsUser = new List<Reservation>();
        foreach (var reservation in Reservations)
            if (reservation.UserId == user.Id)
                reservationsUser.Add(reservation);
        return reservationsUser;
    }
}