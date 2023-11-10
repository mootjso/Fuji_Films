public static class TicketHandler
{
    public const string FileName = "tickets.json";
    public static List<Ticket> Tickets;

    static TicketHandler()
    {
        Tickets = AppInitializer.GetTicketObjects();
        
    }

    public static List<Ticket> GetTicketsByShowId(int showId)
    {
        var ticketsForShow = new List<Ticket>();
        foreach (var ticket in Tickets)
            if (ticket.ShowId == showId)
                ticketsForShow.Add(ticket);
        return ticketsForShow;
    }

    public static List<Ticket> GetTicketsByUser(User user)
    {
        var ticketsUser = new List<Ticket>();
        foreach (var ticket in Tickets)
            if (ticket.UserId == user.Id)
                ticketsUser.Add(ticket);
        return ticketsUser;
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