public static class TheaterHandler
{
    public const string FileName = "theaters.json";
    const double BlueSeatPrice = 10;
    const double YellowSeatPrice = 15;
    const double RedSeatPrice = 20;

    public static List<Theater> Theaters;

    static TheaterHandler()
    {
        Theaters = AppInitializer.GetTheaterObjects();
    }

    public static Theater CreateTheater(Show show)
    {
        var theater = new Theater(show.Id);
        theater.Seats = CreateSeats(theater.SeatArrangement);
        Theaters.Add(theater);
        JSONMethods.WriteToJSON(Theaters, FileName);
        return theater;
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

    public static List<Seat> CreateSeats(int[,] seatArrangement)
    {
        List<Seat> seats = new();
        int rows = seatArrangement.GetLength(0);
        int columns = seatArrangement.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                switch (seatArrangement[i, j])
                {
                    case 0:
                        seats.Add(new Seat(i, j, -1, -1, false));
                        break;
                    case 1:
                        seats.Add(new Seat(i, j, BlueSeatPrice, -1, true));
                        break;
                    case 2:
                        seats.Add(new Seat(i, j, YellowSeatPrice, -1, true));
                        break;
                    case 3:
                        seats.Add(new Seat(i, j, RedSeatPrice, -1, true));
                        break;
                }
            }
        }
        return seats;
    }

    public static List<Ticket>? SelectSeats(User user, Theater theater)
    {
        Console.CursorVisible = false;

        List<Ticket> tickets = new();
        Show selectedShow = ShowHandler.GetShowById(theater.ShowId)!;
        List<Seat> selectedSeats = new();

        int selectedRow = 0;
        int selectedColumn = 0;
        double totalPrice;
        ConsoleKeyInfo keyInfo;

        LoadingBar.Start();

        do  // Do-while loop draws the seating overview, allows the user to select seats and checkout using the arrow keys and Enter
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("Use arrow keys to move. Press 'Enter' to select a seat. Press 'Q' to quit. Press 'C' to Checkout.");
            Console.WriteLine("\nChoose your Seat :\n");

            DrawSeatOverview(theater, selectedRow, selectedColumn);
            DrawMovieScreen();
            DisplayPriceInfo();

            totalPrice = GetTotalPrice(tickets);
            DisplaySelectedSeats(selectedSeats);
            Console.WriteLine($"\nTotal price of reservation: {totalPrice} EUR\n");

            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                // User navigates through the seats
                case ConsoleKey.UpArrow:
                    if (selectedRow > 0)
                        selectedRow--;
                    break;

                case ConsoleKey.DownArrow:
                    if (selectedRow < 13)
                        selectedRow++;
                    break;

                case ConsoleKey.LeftArrow:
                    if (selectedColumn > 0)
                        selectedColumn--;
                    break;

                case ConsoleKey.RightArrow:
                    if (selectedColumn < 11)
                        selectedColumn++;
                    break;

                // User selects a seat
                case ConsoleKey.Enter:
                    Seat selectedSeat = GetSeatByRowAndColumn(theater, selectedRow, selectedColumn)!;
                    List<int> validSelections = new() { 1, 2, 3 };
                    
                    // Valid seat selected
                    if (validSelections.Contains(theater.SeatArrangement[selectedRow, selectedColumn]))
                    {
                        double seatPrice = selectedSeat.Price;
                        string seatColor = seatPrice == RedSeatPrice ? "Red" : seatPrice == YellowSeatPrice ? "Red" : "Blue";

                        Ticket ticket = new(selectedShow.Id, user.Id, selectedRow, selectedColumn, seatPrice, seatColor);

                        Console.WriteLine($"You have selected seat {selectedSeat.PositionName}.");
                        Console.WriteLine($"Seat price: {seatPrice} EUR ({ticket.Color} Seat)");

                        // Ask user to confirm selected seat
                        if (ConfirmSeatSelection(ticket))
                        {
                            theater.SeatArrangement[selectedRow, selectedColumn] = 5;
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
                    LoadingBar.Start();

                    if (ConfirmReservation(tickets))
                    {
                        TurnSelectedSeatsIntoReserved(theater);
                        JSONMethods.WriteToJSON(Theaters, FileName);
                        TicketHandler.Tickets.AddRange(tickets);
                        JSONMethods.WriteToJSON(TicketHandler.Tickets, TicketHandler.FileName);
                        return tickets;
                    }
                    else
                    {
                        LoadingBar.Start();
                        break;
                    }
            }
        }
        while (keyInfo.Key != ConsoleKey.Q);

        // Reset the changes made to Seats and SeatArrangement array during seat selection if the user has selected seats but decides to quit
        TurnSelectedSeatsIntoReserved(theater);
        ResetSelectedSeats(theater, selectedSeats);

        return null;
    }

    public static void TurnSelectedSeatsIntoReserved(Theater theater)
    {
        // Change the 5's in the theater SeatArrangement array (which represent the current selected seats of a user) into 4's (which represent seats that are taken)
        // Method is ran when the user goes to checkout
        // Ensures that the next time a user wants to select a seat, the seats that are already taken are colored grey
        int rows = theater.SeatArrangement.GetLength(0);
        int columns = theater.SeatArrangement.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (theater.SeatArrangement[i, j] == 5)
                {
                    theater.SeatArrangement[i, j] = 4;
                }
            }
        }
    }
    
    public static void ResetSelectedSeats(Theater theater, List<Seat> seats)
    {
        // Reset the Seat objects and SeatArrangement array for when the user quits out of seat selection after having selected seats
        foreach (var seat in seats)
        {
            foreach (var _seat in theater.Seats)
            {
                if (seat.PositionName == _seat.PositionName)
                {
                    _seat.UserId = -1;
                    theater.SeatArrangement[seat.Row, seat.Column] = seat.Price == 10 ? 1 : (seat.Price == 15 ? 2 : 3);
                }
            }
        }
    }
    
    public static void DrawSeatOverview(Theater theater, int selectedRow, int selectedColumn)
    {
        int rows = theater.SeatArrangement.GetLength(0);
        int columns = theater.SeatArrangement.GetLength(1);

        // Write top line of numbers for the grid
        Console.Write("   ");
        for (int j = 1; j <= 12; j++)
        {
            Console.Write(j + " ");
        }
        Console.Write("\n");

        char rowLabel = 'A';
        for (int i = 0; i < rows; i++)
        {
            Console.Write(rowLabel + "  ");
            rowLabel++;

            for (int j = 0; j < columns; j++)
            {
                if (i == selectedRow && j == selectedColumn)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                
                switch (theater.SeatArrangement[i,j])
                {
                    case 0:
                        Console.Write("  ");
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("■ ");
                        Console.ResetColor();
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("■ ");
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("■ ");
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("O ");
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("O ");
                        break;
                    default: 
                        break;
                }
                Console.ResetColor();
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

    public static void DrawMovieScreen()
    {
        Console.WriteLine("");
        for (int j = 0; j < 30; j++)
        {
            Console.Write("■");

        }
        Console.WriteLine("\n        Movie screen\n");
    }

    public static void DisplayPriceInfo()
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($"■ : {RedSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"■ : {YellowSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"■ : {BlueSeatPrice} EUR");
        Console.WriteLine("\n");

        Console.ResetColor();
    }

    public static bool ConfirmReservation(List<Ticket> tickets)
    {
        // No seats selected
        if (tickets.Count <= 0)
        {
            Console.WriteLine("Cannot checkout, you have not selected any seats.");
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
            return false;
        }

        Console.WriteLine("Press 'Enter' to continue to checkout, press any other key to go back to seat selection.");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // User confirms their selection and continues to checkout
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return true;
        }

        return false;
    }

    public static bool ConfirmSeatSelection(Ticket ticket)
    {
        Console.WriteLine("\nAre you sure you want to choose this seat? (Press Enter to confirm, press any other key to cancel)");
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

    public static double GetTotalPrice(List<Ticket> tickets)
    {
        double totalPrice = 0;
        foreach (Ticket ticket in tickets)
        {
            totalPrice += ticket.Price;
        }

        return totalPrice;
    }
}