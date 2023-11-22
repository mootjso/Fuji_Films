using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        return ReservationIDString;
    }

    public static void AddTicketsToReservations()
    {
        // Hier leest hij de json files voor hieronder
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>("tickets.json").ToList();
        List<Reservation> reservations = JSONMethods.ReadJSON<Reservation>("reservations.json").ToList();
        // Elke ticket in de tickets.json wordt hier per ticket gechecked
        foreach (var ticket in tickets)
        {
            // Hier wordt een boolean gemaakt voor hier later
            bool reservationExists = false;
            // Nu kijken voor elke reservatie in reservations.json wordt gechecked als de ticket er al bestaat.
            foreach (var reservation in reservations)
            {
                if (reservation.ShowId == ticket.ShowId)
                {
                    if (reservation.ReservationId == ticket.ReservationId)
                    {
                        if (reservation.Column == ticket.Column)
                        {
                            if (reservation.Row == ticket.Row)
                            {
                                // Indien de ticket in kwestie al bestaat wordt hij overgeslagen door de boolean op true te zetten
                                reservationExists = true;
                                break;
                            }
                        }
                    }
                }
            }
            // Als de ticket niet bestaat in reservations.json wordt dit uitgevoerd
            if (!reservationExists)
            {
                {
                    // Hier wordt de reservation gemaakt en geadd met de juiste gegevens in reservations.json
                    Show show = ShowHandler.GetShowById(ticket.ShowId);
                    Movie movie = MovieHandler.GetMovieById(show.MovieId);
                    var reservation = new Reservation(ticket.ReservationId, ticket.UserId, ticket.ShowId, movie.Id, ticket.Row, ticket.Column);

                    reservations.Add(reservation);
                    JSONMethods.WriteToJSON(reservations, FileName);
                }

            }
        }
    }

    public static void GetReservationsByUser(User user)
    {
        List<Reservation> allReservations = JSONMethods.ReadJSON<Reservation>(FileName).ToList();

        var reservationsUser = new List<Reservation>();

        foreach (var reservation in allReservations)
        {
            if (reservation.UserId == user.Id)
            {
                reservationsUser.Add(reservation);
            }
        }

        var overviewMenuOptions = new List<string>();

        if (reservationsUser.Count > 0)
        {
            foreach (var reservation in reservationsUser)
            {
                Movie movie = MovieHandler.GetMovieById(reservation.MovieId);
                if (!overviewMenuOptions.Contains(movie.Title))
                {
                    overviewMenuOptions.Add(movie.Title);
                }
            }
            string overviewMenuText = "Choose a movie from your reservations:\n";

            int selectedMovieIndex = Menu.Start(overviewMenuText, overviewMenuOptions);

            // Hier een if statement maken die kijkt naar welke film gekozen is.
            // En dan per film kijken welke reservatie codes er zijn (1x printen) met er achter de .Count() hoeveelheid (mogelijk ook de stoelen ligt er aan hoe mooi ik dat kan maken).
        }
        else
        {
            Console.WriteLine("You have no reservations.");
        }
    }
}