using Newtonsoft.Json;

public class Auditorium_1
{
    private SeatDatabase SeatDb;
    private static List<string> SelectedSeats = new List<string>();
    public string Filename;
    public List<Ticket> Tickets = new();

    public Auditorium_1(ScheduledMovie selectedMovie)
    {
        Filename = $"seat_reservations_{selectedMovie.Id}.json";
        SeatDb = GetSeatDatabase();
    }

    public List<Ticket> SelectSeats()
    {
        Console.CursorVisible = false;

        int selectedRow = 0;
        int selectedColumn = 0;
        double totalPrice;
        ConsoleKeyInfo keyInfo;

        DisplayLoadingBar();

        do  // Do-while loop draws the seating overview, allows the user to select seats and checkout using the arrow keys and Enter
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            Console.WriteLine("Use arrow keys to move. Press 'Enter' to select a seat. Press 'Q' to quit. Press 'C' to Checkout.");
            Console.WriteLine("\nChoose your Seat :\n");

            DrawSeats(selectedRow, selectedColumn);
            DrawMovieScreen();
            DisplayPriceInfo();

            totalPrice = GetTotalPrice();
            DisplaySelectedSeats();
            Console.WriteLine($"\nTotal price of reservation: {totalPrice}\n");

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
                    string selectedSeat = $"{Convert.ToChar(selectedRow + 'A')}{selectedColumn + 1}";
                    if (SeatDb.Seats[selectedRow, selectedColumn] == '■')
                    {
                        double seatPrice = GetSelectedSeatPrice(selectedRow, selectedColumn);
                        string seatColor = seatPrice == SeatDb.RedSeatPrice ? "Red" : seatPrice == SeatDb.YellowSeatPrice ? "Red" : "Blue";

                        Ticket ticket = new(selectedSeat, seatPrice, seatColor);

                        Console.WriteLine($"You have selected seat {ticket.Position}.");
                        Console.WriteLine($"Seat price: {ticket.Price} EUR ({ticket.Color} Seat)");

                        if (ConfirmSelection(ticket))
                        {
                            SeatDb.Seats[selectedRow, selectedColumn] = 'X';
                            Tickets.Add(ticket);
                        }
                    }
                    // Invalid seat selected
                    else
                    {
                        Console.WriteLine($"Seat {selectedSeat} is already taken or cannot be chosen.");
                        Console.WriteLine("Press any key to choose another seat.");
                        Console.ReadKey();
                    }
                    break;

                // Go to checkout
                case ConsoleKey.C:
                    DisplayLoadingBar();

                    if (ConfirmCheckout())
                        return Tickets;
                    else
                    {
                        DisplayLoadingBar();
                        break;
                    }
            }
        }
        while (keyInfo.Key != ConsoleKey.Q);

        return Tickets;
    }

    // TODO Uncomment method when done with testing
    public void DisplayLoadingBar()
    {
        //Console.Clear();
        //LoadingBar.Start();
        //Console.ResetColor();
    }

    public void DisplaySelectedSeats()
    {
        Console.Write("Selected seat(s): ");
        List<string> seatPositions = new();
        foreach (Ticket ticket in Tickets)
            seatPositions.Add(ticket.Position);

        Console.Write($"{string.Join(", ", seatPositions)}");
    }

    public void DrawSeats(int selectedRow, int selectedColumn)
    {
        // Write top line of numbers for the grid
        Console.Write("   ");
        for (int j = 1; j <= 12; j++)
        {
            Console.Write(j + " ");
        }
        Console.Write("\n");

        char rowLabel = 'A';
        for (int i = 0; i < 14; i++)
        {
            Console.Write(rowLabel + "  ");
            rowLabel++;

            // Change the font color for seats based on their position
            for (int j = 0; j < 12; j++)
            {
                if (i == selectedRow && j == selectedColumn)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else if ((i >= 5 && i <= 8 && (j == 5 || j == 6)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if ((i == 3 || i == 4 || i == 9 || i == 10) && (j == 5 || j == 6))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else if ((i >= 4 && i <= 9) && (j == 4 || j == 7))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else if ((i >= 5 && i <= 8) && (j == 3 || j == 8))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else if ((i <= 13) && (j <= 11))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                else if (SeatDb.Seats[i, j] == '■')
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write(SeatDb.Seats[i, j] + " ");
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }

    public void DrawMovieScreen()
    {
        Console.WriteLine("");
        for (int j = 0; j < 30; j++)
        {
            Console.Write("■");

        }
        Console.WriteLine("\n        Movie screen");
    }

    public double GetSelectedSeatPrice(int selectedRow, int selectedColumn)
    {
        double seatPrice;
        if (selectedRow >= 5 && selectedRow <= 8 && (selectedColumn == 5 || selectedColumn == 6))
        {
            seatPrice = SeatDb.RedSeatPrice;
        }
        else if ((selectedRow == 3 || selectedRow == 4 || selectedRow == 9 || selectedRow == 10) && (selectedColumn == 5 || selectedColumn == 6))
        {
            seatPrice = SeatDb.YellowSeatPrice;
        }
        else if ((selectedRow >= 4 && selectedRow <= 9) && (selectedColumn == 4 || selectedColumn == 7))
        {
            seatPrice = SeatDb.YellowSeatPrice;
        }
        else if ((selectedRow >= 5 && selectedRow <= 8) && (selectedColumn == 3 || selectedColumn == 8))
        {
            seatPrice = SeatDb.YellowSeatPrice;
        }
        else
        {
            seatPrice = SeatDb.BlueSeatPrice;
        }

        return seatPrice;
    }

    public void DisplayPriceInfo()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"■ : {SeatDb.RedSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"■ : {SeatDb.YellowSeatPrice} EUR\t");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"■ : {SeatDb.BlueSeatPrice} EUR");
        Console.WriteLine("\n");

        Console.ResetColor();
    }

    public SeatDatabase GetSeatDatabase()
    {
        SeatDatabase? seatDatabase;
        if (File.Exists(Filename))
        {
            string json = File.ReadAllText(Filename);
            seatDatabase = JsonConvert.DeserializeObject<SeatDatabase>(json);
        }
        else
        {
            seatDatabase = new SeatDatabase
            {
                Seats = new char[14, 12],
                RedSeatPrice = 20.0,
                YellowSeatPrice = 15.0,
                BlueSeatPrice = 10.0
            };
        }

        // Replace all characters in 2d array with squares, except for 'X's which indicate reservered seats
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if (!(seatDatabase.Seats[i, j] == 'X'))
                {
                    seatDatabase.Seats[i, j] = '■';
                }
            }
        }

        // Replace the positions where there is no chair with empty spaces
        seatDatabase.Seats[0, 0] = seatDatabase.Seats[0, 1] = ' ';
        seatDatabase.Seats[0, 10] = seatDatabase.Seats[0, 11] = ' ';
        seatDatabase.Seats[1, 0] = seatDatabase.Seats[1, 11] = ' ';
        seatDatabase.Seats[2, 0] = seatDatabase.Seats[2, 11] = ' ';
        seatDatabase.Seats[11, 0] = seatDatabase.Seats[11, 11] = ' ';
        seatDatabase.Seats[12, 0] = seatDatabase.Seats[12, 1] = ' ';
        seatDatabase.Seats[12, 10] = seatDatabase.Seats[12, 11] = ' ';
        seatDatabase.Seats[13, 0] = seatDatabase.Seats[13, 1] = ' ';
        seatDatabase.Seats[13, 10] = seatDatabase.Seats[13, 11] = ' ';

        return seatDatabase;
    }

    public bool ConfirmSelection(Ticket ticket)
    {
        Console.WriteLine("\nAre you sure you want to choose this seat? (Press Enter to confirm, press any other key to cancel)");
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("Seat selected.");
            SelectedSeats.Add($"{ticket.Position}");

            return true;
        }
        return false;
    }

    public double GetTotalPrice()
    {
        double totalPrice = 0;
        foreach (Ticket ticket in Tickets)
        {
            totalPrice += ticket.Price;
        }

        return totalPrice;
    }

    public void WriteToJson(SeatDatabase seatDb)
    {
        string updatedJson = JsonConvert.SerializeObject(seatDb);
        File.WriteAllText(Filename, updatedJson);
    }

    public bool ConfirmCheckout()
    {
        // No seats selected
        if (Tickets.Count <= 0)
        {
            Console.WriteLine("Cannot checkout, you have not selected any seats.");
            Console.WriteLine("Press any key to continue");

            Console.ReadKey();
            return false;
        }

        DisplaySelectedSeats();

        double totalPrice = GetTotalPrice();
        Console.WriteLine($"\nTotal price: {totalPrice} EUR");

        Console.WriteLine("\nPress 'Enter' to continue to checkout, press any other key to go back to seat selection.");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // User confirms their selection and continues to checkout
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            Console.Clear();
            Console.WriteLine("Writing to json");
            Console.ReadKey();
            WriteToJson(SeatDb);

            return true;
        }

        return false;
    }
}