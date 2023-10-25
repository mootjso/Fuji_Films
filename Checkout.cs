public static class Checkout
{
    public static void Start(List<Ticket> tickets)
    {
        if (!(tickets.Count > 0))
        {
            Console.WriteLine("No tickets selected to checkout");
            Console.WriteLine("Press any key to continue");
            return;
        }

        // Show reservation information
        //Console.Clear();
        //DisplayAsciiArt.Header();
        //Console.WriteLine("Ticket checkout");
        //Console.Write("\nMovie: ");
        //Console.Write(tickets[0].ShowId.Movie.Title);
        //Console.WriteLine($"\nDate: {tickets[0].ShowId.StartTimeString.ToString("yyyy-MM-dd")}");
        //Console.WriteLine($"Time: {tickets[0].ShowId.StartTimeString.TimeOfDay}");

        Auditorium_1.DisplaySelectedSeats(tickets);
        
        
        Console.ReadKey();
        
    }
}