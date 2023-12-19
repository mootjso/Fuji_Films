public static class ReservationHandler
{
    public const string FileName = "Datasources/reservations.json";
    public static List<Reservation> Reservations;

    static ReservationHandler()
    {
        Reservations = JSONMethods.ReadJSON<Reservation>(FileName).ToList();
    }

    public static string GetReservationID()
    {
        Guid ReservationGUID = Guid.NewGuid();
        // Dit zorgt er voor dat de GUID geen "-" heeft zodat ik dit hieronder zelf kan toevoegen.
        string ReservationID = ReservationGUID.ToString("N");
        // Voorbeeld van reservation code: FD8J-FJN8-A4FX
        string ReservationIDString =
            $"{ReservationID.Substring(0, 4)}-{ReservationID.Substring(4, 4)}-{ReservationID.Substring(8, 4)}";

        return ReservationIDString;
    }

    public static void AddTicketsToReservations()
    {
        // Hier leest hij de json files voor hieronder
        List<Ticket> tickets = JSONMethods.ReadJSON<Ticket>(TicketHandler.FileName).ToList();
        List<Reservation> reservations = JSONMethods
            .ReadJSON<Reservation>(ReservationHandler.FileName)
            .ToList();
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
                    Show? show = ShowHandler.GetShowById(ticket.ShowId)!;
                    Movie? movie = MovieHandler.GetMovieById(show.MovieId)!;
                    Reservation? reservation = new Reservation(
                        ticket.ReservationId!,
                        ticket.UserId,
                        ticket.ShowId,
                        movie.Id,
                        ticket.Row,
                        ticket.Column
                    );

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
        // Ik zet hier alle reservations van de user uit de json file naar deze list
        foreach (var reservation in allReservations)
        {
            if (reservation.UserId == user.Id)
                reservationsUser.Add(reservation);
        }

        var overviewMenuOptions = new List<string>();
        var overviewReservationCodes = new List<string>();
        var overviewCorrectReservation = new List<string>();

        if (reservationsUser.Count > 0)
        {
            // Hier check je ff per reservatie in de list waar alle reservaties van de user staat
            // Je maakt per check een movie en als de movie title niet in de lijst staat overviewMenuOptions dan voeg je hem toe
            foreach (var reservation in reservationsUser)
            {
                Movie movie = MovieHandler.GetMovieById(reservation.MovieId)!;
                if (!overviewMenuOptions.Contains(movie.Title))
                    overviewMenuOptions.Add(movie.Title);
            }

            string overviewMenuText = "Choose a movie from your reservations:\n";
            // Hier maak de menu met de films
            int movieIndex = Menu.Start(overviewMenuText, overviewMenuOptions);
            if (movieIndex == overviewMenuOptions.Count) // User presses left arrow key
                return;
            string selectedMovie = overviewMenuOptions[movieIndex];

            Show showForReservation = null;

            // Hier check je per reservatie de film als de title hetzelfde is als de geselecteerde title
            foreach (var reservation in reservationsUser)
            {
                Movie? movie = MovieHandler.GetMovieById(reservation.MovieId)!;
                if (movie.Title == selectedMovie)
                {
                    // Nu check je per reservatie als de movie.id hetzelfde is als de geselecteerde film
                    // En vervolgens check je als die nog niet in de list staat en voegt het toe aan overviewReservationCodes met de datum voor visuele aspect
                    // En je voegt het toe aan overviewCorrectReservation waar later mee gewerkt wordt.
                    foreach (var reservationCode in reservationsUser)
                    {
                        if (reservationCode.MovieId == movie.Id)
                        {
                            Show show = ShowHandler.GetShowById(reservationCode.ShowId)!;
                            showForReservation = show;
                            if (
                                !overviewReservationCodes.Contains(
                                    $"{reservationCode.ReservationId}, on {show.DateString} from {show.StartTimeString} - {show.EndTimeString}"
                                )
                            )
                            {
                                overviewReservationCodes.Add(
                                    $"{reservationCode.ReservationId}, on {show.DateString} from {show.StartTimeString} - {show.EndTimeString}"
                                );
                                overviewCorrectReservation.Add(reservationCode.ReservationId);
                            }
                        }
                    }
                }
            }

            // Hier maak je vervolgens een menu van de verschillende reservatie codes die bij de film horen die je had geselecteerd
            string overviewReservationCodesText =
                "Please choose the reservation you would like to use:\n";
            // Hier maak je de menu met de reservatie codes
            int selectedReservation = Menu.Start(
                overviewReservationCodesText,
                overviewReservationCodes
            );
            if (selectedReservation == overviewReservationCodes.Count)
                return;
            // Dit is de gekozen reservatie code
            string selectedReservationCode = overviewCorrectReservation[selectedReservation];
            int ReservationInt = 1;

            // Hier print je de stoelen positie die onder die reservatie code staat
            Console.Clear();
            DisplayAsciiArt.Header();
            AdHandler.DisplaySnacks();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{selectedMovie}");
            Console.ResetColor();
            Console.WriteLine(
                $"Date: {showForReservation.DateString}\nTime: {showForReservation.StartTimeString} - {showForReservation.EndTimeString}"
            );
            Console.ResetColor();
            Console.WriteLine(
                $"\nThese are your tickets for reservation {overviewCorrectReservation[selectedReservation]}:\n"
            );
            int reservationInt = 1;
            Console.WriteLine("Ticket No. | Row | Seat");
            Console.WriteLine(new string('-', 28));

            foreach (var reservation in reservationsUser)
            {
                if (reservation.ReservationId == selectedReservationCode)
                {
                    string ticketInfo = $"Ticket {reservationInt}".PadRight(10);
                    string rowInfo = $"Row {reservation.Row}".PadRight(6);
                    string seatInfo = $"Seat: {reservation.Column}";

                    Console.WriteLine($"{ticketInfo} | {rowInfo} | {seatInfo}");
                    reservationInt++;
                }
            }
        }
        // Hier heb je geen reservations
        else
        {
            Console.WriteLine("You currently have no reservations.");
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Press any key to go back");
        Console.ReadKey();
        Console.ResetColor();
    }
}
