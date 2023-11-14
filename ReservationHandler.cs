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

    public static void AddTicketsToReservations()
    {
        var tickets = JSONMethods.ReadJSON<Ticket>("tickets.json");
        var reservations = JSONMethods.ReadJSON<Reservation>("reservations.json");

        foreach (var ticket in tickets)
        {
            bool reservationExists = false;
            foreach (var reservation in reservations)
            {
                if (reservation.ShowId == ticket.ShowId)
                {
                    if (reservation.ReservationId == ticket.ReservationId)
                    {
                        reservationExists = true;
                        break;
                    }
                }
            }

            if (!reservationExists)
            {
                // HIER MOET NOG DE SHOW GEVONDEN WORDEN ZODAT WE DE MOVIE TITLE HEBBEN
                var show = GetShowById(ticket.ShowId);
                if (show != null)
                {
                    int movieId = show.MovieId;
                    var reservation = new Reservation(ticket.ReservationId, ticket.UserId, ticket.ShowId, ); //Hier moet de movie id

                    reservations.Add(reservation);
                }
                
            }
        }

        // HIER MOET DE LIST VAN RESERVATIONS DIE VONDEN ZIJN GESAVED WORDEN NAAR JSON
        JSONMethods.WriteToJSON(reservations, "reservations.json");
    }

    public static List<Reservation> GetReservationsByUser(User user)
    {
        var reservationsUser = new List<Reservation>();
        foreach (var reservation in Reservations)
            if (reservation.UserId == user.Id)
                reservationsUser.Add(reservation);
        return reservationsUser;
    }

    public 
}
