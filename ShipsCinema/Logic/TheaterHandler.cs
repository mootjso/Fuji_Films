using System.Net.Sockets;

public static class TheaterHandler
{
    public const string FileName = "Datasources/theaters.json";

    public static List<Theater> Theaters;

    static TheaterHandler()
    {
        Theaters = JSONMethods.ReadJSON<Theater>(FileName).ToList();
    }

    public static Theater CreateOrGetTheater(Show show)
    {
        var theater = GetTheaterByShowId(show.Id);

        if (theater == null)
        {
            theater = new Theater(show.Id, show.TheaterNumber);
            Theaters.Add(theater);
            JSONMethods.WriteToJSON(Theaters, FileName);
            
            return theater;
        }
        return theater;
    }

    private static void WriteControlsInfo(User user)
    {
        // Controls
        Console.ForegroundColor = ConsoleColor.DarkGray;
        if (!user.IsAdmin)
        {
            Console.WriteLine("Controls:\n[Arrow keys] Navigation\n[Enter]      Select a seat\n[C]          Checkout\n[Esc]        Quit");
        }
        else if (user.IsAdmin)
        {
            Console.WriteLine("Controls:\n[Arrow keys] Navigation\n[Enter]      Select a seat\n[C]          Confirm selection\n[Esc]        Quit");
        }
        Console.ResetColor();
    }

    public static void SelectSeats(User user, Theater theater)
    {
        foreach (var seat in theater.Seats)
            if (!seat.IsReserved)
                seat.UserId = -1;

        string reservationId = ReservationHandler.GetReservationID();
        bool paymentConfirmed = false;
        bool bookingCorrect = false;
        Console.CursorVisible = false;

        List<Ticket> tickets = new();
        Show selectedShow = ShowHandler.GetShowById(theater.ShowId)!;
        Movie selectedMovie = MovieHandler.GetMovieById(selectedShow.MovieId)!;
        List<Seat> selectedSeats = new();

        int selectedRow = 0;
        int selectedColumn = 0;
        int totalRows = theater.Seats.Max(seat => seat.Row);
        int totalColumns = theater.Seats.Max(seat => seat.Column);

        double totalPrice;
        ConsoleKeyInfo keyInfo;

        LoadingBar.Start();

        do  // Do-while loop draws the seating overview, allows the user to select seats and checkout using the arrow keys and Enter
        {
            Console.Clear();

            if (!user.IsAdmin)
            {
                DisplayAsciiArt.Header();
                Console.Write("Choose your Seat(s) for ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(selectedMovie.Title + "\n");
                Console.ResetColor();
            }
            else if (user.IsAdmin)
            {
                DisplayAsciiArt.AdminHeader();
                Console.Write("Select seat(s) for ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(selectedMovie.Title);
                Console.ResetColor();
                Console.Write(" to make them unavailable\n\n");
            }
            

            DrawSeatOverview(theater, selectedRow, selectedColumn, user, selectedSeats);
            DrawMovieScreen(theater);
            Seat selectedSeat = GetSeatByRowAndColumn(theater, selectedRow, selectedColumn)!;
            DisplaySymbolsInfo(selectedSeat, user);
            if (!user.IsAdmin)
            {
                totalPrice = TicketHandler.GetTotalPrice(tickets);
                Console.Write($"Total price of reservation: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(totalPrice + " EUR\n");
            }

            WriteControlsInfo(user);

            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                // User navigates through the seats
                case ConsoleKey.UpArrow:
                    if (selectedRow > 0)
                        selectedRow--;
                    break;

                case ConsoleKey.DownArrow:
                    if (selectedRow < totalRows)
                        selectedRow++;
                    break;

                case ConsoleKey.LeftArrow:
                    if (selectedColumn > 0)
                        selectedColumn--;
                    break;

                case ConsoleKey.RightArrow:
                    if (selectedColumn < totalColumns)
                        selectedColumn++;
                    break;

                // User selects a seat
                case ConsoleKey.Enter:

                    // Deselect seat
                    if (selectedSeats.Contains(selectedSeat) || user.IsAdmin && !selectedSeat.IsReserved && !selectedSeat.IsAvailable)
                    {
                        selectedSeat.IsAvailable = true;
                        selectedSeat.UserId = -1;
                        selectedSeats.Remove(selectedSeat);
                        Ticket ticket = tickets.Find(t => t.UserId == user.Id && t.Row == selectedSeat.Row && t.Column == selectedSeat.Column && selectedShow.Id == t.ShowId)!;
                        tickets.Remove(ticket);
                    }
                    // Valid seat selected
                    else if (selectedSeat.IsSeat && selectedSeat.UserId == -1 && selectedSeat.IsAvailable)
                    {
                        double seatPrice = selectedSeat.Price;
                        string seatColor = seatPrice == Theater.RedSeatPrice ? "Red" : seatPrice == Theater.YellowSeatPrice ? "Yellow" : "Blue";

                        Ticket ticket = new(selectedShow.Id, user.Id, selectedRow, selectedColumn, seatPrice, seatColor, reservationId);

                        if (user.IsAdmin)
                            selectedSeat.IsAvailable = false;

                        selectedSeat.UserId = user.IsAdmin ? -1 : user.Id;
                        selectedSeats.Add(selectedSeat);
                        tickets.Add(ticket);
                    }
                    // Invalid seat selected
                    else
                    {
                        Console.ForegroundColor= ConsoleColor.Red;
                        Console.WriteLine($"\nThis seat is already taken or cannot be chosen.");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to choose another seat");
                        Console.ReadKey();
                    }
                    break;

                // Go to checkout
                case ConsoleKey.C:

                    if (ConfirmReservation(tickets, user.IsAdmin))
                    {
                        if (!user.IsAdmin)
                        {
                            (bookingCorrect, paymentConfirmed) = CheckOutHandler.CheckOut(selectedShow, selectedSeats);
                            if (!bookingCorrect)
                                continue;
                            if (paymentConfirmed)
                            {
                                foreach (var seat in selectedSeats)
                                    seat.IsReserved = true;

                                TicketHandler.Tickets.AddRange(tickets);
                                JSONMethods.WriteToJSON(TicketHandler.Tickets, TicketHandler.FileName);
                                JSONMethods.WriteToJSON(Theaters, FileName);

                                double totalPriceTickets = tickets.Sum(t => t.Price);
                                int totalAmountTickets = tickets.Count();

                                CheckOutHandler.AddToExistingRevenue(selectedShow.Id, totalPriceTickets);
                                CheckOutHandler.RevenueQuarterYearIfStatement(selectedShow, totalPriceTickets, totalAmountTickets);
                            }
                        }
                        JSONMethods.WriteToJSON(Theaters, FileName);
                        return;
                    }
                    else
                    {
                        break;
                    }
            }
        }
        while (keyInfo.Key != ConsoleKey.Escape);

        foreach (Seat seat in selectedSeats)
        {
            seat.UserId = -1;
            seat.IsAvailable = true;
        }
            
        return;
    }

    public static Theater? GetTheaterByShowId(int showId)
    {
        foreach (var theater in Theaters)
        {
            if (theater.ShowId == showId)
                return theater;
        }
        return null;
    }

    public static void DrawSeatOverview(Theater theater, int selectedRow, int selectedColumn, User user, List<Seat> selectedSeats)
    {
        int rows = theater.Seats.Max(seat => seat.Row);
        int columns = theater.Seats.Max(seat => seat.Column);
        for (int i = 0; i < rows + 1; i++)
        {
            Console.ResetColor();

            for (int j = 0; j < columns + 1; j++)
            {
                Seat seat = GetSeatByRowAndColumn(theater, i, j)!;
                // Change background color for the selected seat
                if (i == selectedRow && j == selectedColumn)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                    Console.ResetColor();

                if (seat.IsAvailable)
                {
                    // Not a seat
                    if (!seat.IsSeat)
                    {
                        Console.Write("  ");
                        Console.ResetColor();
                        continue;
                    }
                    // Seat has been reserved by current user in a previous reservation
                    if (seat.UserId == user.Id && selectedSeats.Contains(seat))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O ");
                        continue;
                    }
                    // Seat has been reserved by current user
                    if (seat.UserId == user.Id)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("O ");
                        continue;
                    }
                    // Seat has been reserverd by a different user
                    if (seat.UserId != -1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("O ");
                        continue;
                    }
                    // Seat is not taken
                    Console.ForegroundColor = seat.Price == Theater.RedSeatPrice ? ConsoleColor.DarkRed :
                        (seat.Price == Theater.YellowSeatPrice) ? ConsoleColor.DarkYellow : ConsoleColor.Blue;
                    Console.Write("■ ");
                }
                // Seat is Unavailable (X'ed out)
                else
                {
                    Console.ForegroundColor= ConsoleColor.DarkGray;
                    Console.Write("X ");
                    Console.ResetColor();
                }
                
            }
            Console.WriteLine();
        }
    }

    private static Seat? GetSeatByRowAndColumn(Theater theater, int row, int column)
    {
        foreach (var seat in theater.Seats)
            if (seat.Row == row && seat.Column == column)
                return seat;
        return null;
    }

    private static void DrawMovieScreen(Theater theater)
    {
        int screenLength = 27;
        int movieEmptySpace = 7;
        switch (theater.NumOfSeats)
        {
            case 150:
                break;
            case 300:
                screenLength = 43;
                movieEmptySpace = 14;
                break;
            case 500:
                screenLength = 66;
                movieEmptySpace = 26;
                break;
            default:
                break;

        }
        Console.WriteLine();
        for (int j = 0; j < screenLength; j++)
        {
            Console.Write("■");

        }
        Console.WriteLine();
        for (int i = 0; i < movieEmptySpace--; i++)
            Console.Write("  ");
        Console.WriteLine("Movie screen\n");
    }

    private static void DisplaySymbolsInfo(Seat selectedSeat, User user)
    {
        if (!user.IsAdmin)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"■: {Theater.RedSeatPrice} EUR\t");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"■: {Theater.YellowSeatPrice} EUR\t");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"■: {Theater.BlueSeatPrice} EUR");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nO: Your seat(s)\t");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("O: Others\t");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("X: Unavailable");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nO: Your seat(s) from previous reservations\n");

            Console.ResetColor();
            Console.Write($"Current seat: Row ");
            Console.ForegroundColor= ConsoleColor.Magenta;
            Console.Write($"[{selectedSeat.Row + 1}]");
            Console.ResetColor();
            Console.Write($", Seat ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"[{selectedSeat.Column + 1}]\n");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"X: Unavailable seat\t");

            Console.Write($"O: Reserved seat\n\n");
        }
    }

    public static bool ConfirmReservation(List<Ticket> tickets, bool isAdmin = false)
    {
        // No seats selected
        if (tickets.Count <= 0)
        {
            if (!isAdmin)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCannot checkout, you have not selected any seats.");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return false;
            }
        }
        
        if (isAdmin)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nYour selection has been confirmed.");
            Console.ResetColor();
            Console.WriteLine("Press any key to go back to the main menu");
            Console.ReadKey();
        }

        return true;
    }
}
