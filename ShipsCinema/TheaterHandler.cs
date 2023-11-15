public static class TheaterHandler
{
    public const string FileName = "theaters.json";

    public static List<Theater> Theaters;

    static TheaterHandler()
    {
        Theaters = AppInitializer.GetTheaterObjects();
    }

    public static Theater CreateTheater(Show show)
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

    public static List<Ticket>? SelectSeats(User user, Theater theater)
    {
        Console.CursorVisible = false;

        List<Ticket> tickets = new();
        Show selectedShow = ShowHandler.GetShowById(theater.ShowId)!;
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
                Console.WriteLine("Use arrow keys to move. Press 'Enter' to select a seat. Press 'Q' to quit. Press 'C' to Checkout.");
                Console.WriteLine("\nChoose your Seat:\n");
            }
            else if (user.IsAdmin)
            {
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine("Use arrow keys to move. Press 'Enter' to select a seat. Press 'Q' to quit. Press 'C' to apply your selection.");
                Console.WriteLine("\nSelect a seat to make it unavailable:\n");
            }
            

            DrawSeatOverview(theater, selectedRow, selectedColumn, user);
            DrawMovieScreen(theater);
            if (!user.IsAdmin)
            {
                DisplayPriceInfo();
                totalPrice = TicketHandler.GetTotalPrice(tickets);
                DisplaySelectedSeats(selectedSeats);
                Console.WriteLine($"\nTotal price of reservation: {totalPrice} EUR\n");
            }

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
                    Seat selectedSeat = GetSeatByRowAndColumn(theater, selectedRow, selectedColumn)!;
                    
                    // Valid seat selected
                    if (selectedSeat.IsSeat && selectedSeat.UserId == -1 && selectedSeat.IsAvailable)
                    {
                        double seatPrice = selectedSeat.Price;
                        string seatColor = seatPrice == Theater.RedSeatPrice ? "Red" : seatPrice == Theater.YellowSeatPrice ? "Yellow" : "Blue";

                        Ticket ticket = new(selectedShow.Id, user.Id, selectedRow, selectedColumn, seatPrice, seatColor);

                        Console.WriteLine($"You have selected seat {selectedSeat.PositionName}.");
                        if (!user.IsAdmin)
                            Console.WriteLine($"Seat price: {seatPrice} EUR ({ticket.Color} Seat)");

                        // Ask user to confirm selected seat
                        if (ConfirmSeatSelection(user))
                        {
                            if (user.IsAdmin)
                                selectedSeat.IsAvailable = false;

                            selectedSeat.UserId = user.Id;
                            selectedSeats.Add(selectedSeat);
                            tickets.Add(ticket);
                        }
                    }
                    // Invalid seat selected
                    else
                    {
                        Console.WriteLine($"Seat {selectedSeat.PositionName} is already taken or cannot be chosen.");
                        Console.WriteLine("Press any key to choose another seat.");
                        Console.ReadKey();
                    }
                    break;

                // Go to checkout
                case ConsoleKey.C:

                    if (ConfirmReservation(tickets, user.IsAdmin))
                    {
                        JSONMethods.WriteToJSON(Theaters, FileName);
                        TicketHandler.Tickets.AddRange(tickets);
                        JSONMethods.WriteToJSON(TicketHandler.Tickets, TicketHandler.FileName);
                        return tickets;
                    }
                    else
                    {
                        break;
                    }
            }
        }
        while (keyInfo.Key != ConsoleKey.Q);

        foreach (Seat seat in selectedSeats)
        {
            seat.UserId = -1;
            seat.IsAvailable = true;
        }
            
        return null;
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

    public static void DrawSeatOverview(Theater theater, int selectedRow, int selectedColumn, User user)
    {
        int rows = theater.Seats.Max(seat => seat.Row);
        int columns = theater.Seats.Max(seat => seat.Column);

        // Write top line of numbers for the grid
        Console.Write("  ");
        for (int j = 1; j <= columns + 1; j++)
        {
            Console.Write(j + " ");
        }
        Console.Write("\n");

        char rowLabel = 'A';
        for (int i = 0; i < rows + 1; i++)
        {
            // Write the Letter-labels on the side of the grid
            Console.ResetColor();
            Console.Write(rowLabel + " ");
            rowLabel++;

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

    public static Seat? GetSeatByRowAndColumn(Theater theater, int row, int column)
    {
        foreach (var seat in theater.Seats)
            if (seat.Row == row && seat.Column == column)
                return seat;
        return null;
    }

    public static void DrawMovieScreen(Theater theater)
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

    public static void DisplayPriceInfo()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($"■ : {Theater.RedSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"■ : {Theater.YellowSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"■ : {Theater.BlueSeatPrice} EUR");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("\nO : Your seat\t");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("O : Others\t");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("X : Unavailable");
        Console.WriteLine("\n");

        Console.ResetColor();
    }

    public static bool ConfirmReservation(List<Ticket> tickets, bool isAdmin = false)
    {
        // No seats selected
        if (tickets.Count <= 0)
        {
            if (!isAdmin)
                Console.WriteLine("Cannot checkout, you have not selected any seats.");
            else
                Console.WriteLine("You have not selected any seats to take out.");

            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
            return false;
        }

        if (!isAdmin)
            Console.WriteLine("Press 'Enter' to continue to checkout, press any other key to go back to seat selection.");
        else
            Console.WriteLine("Press 'Enter' to confirm and take out the selected seats, press any other key to go back to seat selection.");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // User confirms their selection and continues to checkout
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return true;
        }

        return false;
    }

    public static bool ConfirmSeatSelection(User user)
    {
        if (!user.IsAdmin)
        {
            Console.WriteLine("\nAre you sure you want to choose this seat? (Press Enter to confirm, press any other key to cancel)");
        }
        else if (user.IsAdmin)
            Console.WriteLine("\nAre you sure you want to take out this seat? (Press Enter to confirm, press any other key to cancel)");

        ConsoleKeyInfo keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return true;
        }
        return false;
    }

    public static void DisplaySelectedSeats(List<Seat> seats)
    {
        Console.Write("Selected seat(s): ");
        List<string> seatPositions = new();
        foreach (var seat in seats)
            seatPositions.Add(seat.PositionName);

        Console.Write($"{string.Join(", ", seatPositions)}");
    }
}