using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;
using static LoadingBar;

public class Auditorium_1
{
    public static void DisplaySelectedSeats()
    {
        Console.Clear();
        Console.WriteLine("Selected Seats:");
        foreach (var seat in selectedSeats)
        {
            Console.WriteLine(seat);
        }
    }

    public static void DisplayLoadingBar()
    {
        Console.Clear();
        LoadingBar.Start();
        Console.ResetColor();
        
    }


    
    class SeatDatabase
    {
        public char[,] Seats { get; set; }
        public double RedSeatPrice { get; set; }
        public double YellowSeatPrice { get; set; }
        public double BlueSeatPrice { get; set; }
    }

    static List<string> selectedSeats = new List<string>();
    static double totalAmount = 0.0;
    private static ConsoleKeyInfo key;

    static void Start()
    {
        string jsonFilePath = @"SaveFileSeatSelectionAuditorium_1.json";

        SeatDatabase seatDatabase;

        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
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

            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    seatDatabase.Seats[i, j] = '■';
                }
            }

            seatDatabase.Seats[0, 0] = seatDatabase.Seats[0, 1] = ' ';
            seatDatabase.Seats[0, 10] = seatDatabase.Seats[0, 11] = ' ';
            seatDatabase.Seats[1, 0] = seatDatabase.Seats[1, 11] = ' ';
            seatDatabase.Seats[2, 0] = seatDatabase.Seats[2, 11] = ' ';
            seatDatabase.Seats[11, 0] = seatDatabase.Seats[11, 11] = ' ';
            seatDatabase.Seats[12, 0] = seatDatabase.Seats[12, 1] = ' ';
            seatDatabase.Seats[12, 10] = seatDatabase.Seats[12, 11] = ' ';
            seatDatabase.Seats[13, 0] = seatDatabase.Seats[13, 1] = ' ';
            seatDatabase.Seats[13, 10] = seatDatabase.Seats[13, 11] = ' ';
        }

        int selectedRow = 0;
        int selectedColumn = 0;


        DisplayLoadingBar();
        do
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("Use arrow keys to move. Press Enter to select a seat. Press 'Q' to quit. Press 'C' for Checkout.");
            Console.WriteLine("\nChoose your Seat :\n");

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

                    else if (seatDatabase.Seats[i, j] == '■')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.Write(seatDatabase.Seats[i, j] + " ");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.WriteLine("");
            for (int j = 0; j < 30; j++)
            {
                Console.Write("■");
            }

            Console.WriteLine("\n\n\t-Prices-");
            Console.WriteLine($"Red    : {seatDatabase.RedSeatPrice} EUR");
            Console.WriteLine($"Yellow : {seatDatabase.YellowSeatPrice} EUR");
            Console.WriteLine($"Blue   : {seatDatabase.BlueSeatPrice} EUR");

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedRow > 0)
                    {
                        selectedRow--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedRow < 13)
                    {
                        selectedRow++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (selectedColumn > 0)
                    {
                        selectedColumn--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (selectedColumn < 11)
                    {
                        selectedColumn++;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (seatDatabase.Seats[selectedRow, selectedColumn] == '■' &&
                        !(selectedRow == 0 && (selectedColumn == 0 || selectedColumn == 1 || selectedColumn == 10 || selectedColumn == 11)) &&
                        !(selectedRow == 1 && (selectedColumn == 0 || selectedColumn == 11)) &&
                        !(selectedRow == 2 && (selectedColumn == 0 || selectedColumn == 11)) &&
                        !(selectedRow == 11 && (selectedColumn == 0 || selectedColumn == 11)) &&
                        !(selectedRow == 12 && (selectedColumn == 0 || selectedColumn == 1 || selectedColumn == 10 || selectedColumn == 11)) &&
                        !(selectedRow == 13 && (selectedColumn == 0 || selectedColumn == 1 || selectedColumn == 10 || selectedColumn == 11)))
                    {
                        seatDatabase.Seats[selectedRow, selectedColumn] = 'X';
                        Console.WriteLine($"\nYou have selected seat {Convert.ToChar(selectedRow + 'A')}{selectedColumn + 1}.");

                        double seatPrice = 0.0;
                        if (selectedRow >= 5 && selectedRow <= 8 && (selectedColumn == 5 || selectedColumn == 6))
                        {
                            seatPrice = seatDatabase.RedSeatPrice;
                            Console.WriteLine($"Price: {seatPrice} EUR (Red Seat)");
                        }
                        else if ((selectedRow == 3 || selectedRow == 4 || selectedRow == 9 || selectedRow == 10) && (selectedColumn == 5 || selectedColumn == 6))
                        {
                            seatPrice = seatDatabase.YellowSeatPrice;
                            Console.WriteLine($"Price: {seatPrice} EUR (Yellow Seat)");
                        }
                        else if ((selectedRow >= 4 && selectedRow <= 9) && (selectedColumn == 4 || selectedColumn == 7))
                        {
                            seatPrice = seatDatabase.YellowSeatPrice;
                            Console.WriteLine($"Price: {seatPrice} EUR (Yellow Seat)");
                        }
                        else if ((selectedRow >= 5 && selectedRow <= 8) && (selectedColumn == 3 || selectedColumn == 8))
                        {
                            seatPrice = seatDatabase.YellowSeatPrice;
                            Console.WriteLine($"Price: {seatPrice} EUR (Yellow Seat)");
                        }
                        else
                        {
                            seatPrice = seatDatabase.BlueSeatPrice;
                            Console.WriteLine($"Price: {seatPrice} EUR (Blue Seat)");
                        }

                        Console.WriteLine("Are you sure you want to choose this seat? (Press Enter to confirm, any other key to cancel)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine("Seat selected.");
                            selectedSeats.Add($"{Convert.ToChar(selectedRow + 'A')}{selectedColumn + 1}");
                            totalAmount += seatPrice;
                            Console.WriteLine($"\nTotal Amount: {totalAmount} EUR");
                        }
                        else
                        {
                            Console.WriteLine("Seat selection cancelled.");
                            seatDatabase.Seats[selectedRow, selectedColumn] = '■'; 
                            Console.ReadKey(true);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Seat {Convert.ToChar(selectedRow + 'A')}{selectedColumn + 1} is already taken or cannot be chosen.");
                        Console.WriteLine("Press any key to choose another seat.");
                        Console.ReadKey(true);
                    }
                    break;
                case ConsoleKey.C:
                    DisplayLoadingBar();
                    Console.Clear();
                    string updatedJson = JsonConvert.SerializeObject(seatDatabase);
                    File.WriteAllText(jsonFilePath, updatedJson);
                    Console.WriteLine($"\nTotal Amount: {totalAmount} EUR");
                    Console.WriteLine("Press 'Enter' to continue or 'Backspace' to go back to seat selection.");

                    key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        // Check if totalAmount is greater than 0
                        if (totalAmount > 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Ending program.");
                            return; // End the program
                        }
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        // User wants to go back to seat selection
                        Console.Clear();
                        LoadingBar.Start();
                    }
                    DisplayLoadingBar();
                    break;
            }

        } while (key.Key != ConsoleKey.Q);

        Console.Clear();
        Console.WriteLine("Quitted");
    }

    
}
