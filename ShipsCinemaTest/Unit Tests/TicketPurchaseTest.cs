namespace TicketPurchase
{
[TestClass]
public class TicketHandlerTests
{
    private static string FileName = TicketHandler.FileName;
    private static List<Ticket> original_File = JSONMethods.ReadJSON<Ticket>(FileName).ToList();

    [TestInitialize]
    public void TestInitialize()
    {
        // Clear the test JSON file
        File.WriteAllText(FileName, "[]");

        List<Ticket> testTickets = new List<Ticket>
        {
            new Ticket(1, 1, 1, 1, 10.0, "Red"),
            new Ticket(1, 2, 8, 5, 15.0, "Blue"),
            new Ticket(2, 1, 3, 3, 20.0, "Yellow"),
        };
        JSONMethods.WriteToJSON(testTickets, FileName);

    }
    [ClassCleanup]
    public static void CleanupJSONFile()
    {
       JSONMethods.WriteToJSON(original_File, FileName); 
    }

    [TestMethod]
    public void TestTicketToJSON()
    {
        Ticket ticket = TicketHandler.Tickets[1];

        Assert.IsNotNull(ticket);
        Assert.AreEqual(1, ticket.ShowId);
        Assert.AreEqual(2, ticket.UserId);
        Assert.AreEqual(8, ticket.Row);
        Assert.AreEqual(5, ticket.Column);
        Assert.AreEqual(15.0, ticket.Price);
        Assert.AreEqual("Blue", ticket.Color);
        Assert.AreNotEqual(6, ticket.Row);
        Assert.AreNotEqual(14.0, ticket.Price);
        Assert.AreNotEqual("Yellow", ticket.ShowId);
    }

    [TestMethod]
    public void TestGetTicketsByShowId()
    {
        int showId = 1;
        List<Ticket> expectedTickets = TicketHandler.Tickets.Where(t => t.ShowId == showId).ToList();

        List<Ticket> actualTickets = TicketHandler.GetTicketsByShowId(showId);

        CollectionAssert.AreEqual(expectedTickets, actualTickets);
        Assert.AreEqual(2, actualTickets.Count);
    }

    [TestMethod]
    public void TestGetTicketsByUser()
    {
        User user = new User(1, "John", "Doe", "john.doe@example.com", "password123", "1234567890");
        List<Ticket> expectedTickets = TicketHandler.Tickets.Where(t => t.UserId == user.Id).ToList();

        List<Ticket> actualTickets = TicketHandler.GetTicketsByUser(user);

        CollectionAssert.AreEqual(expectedTickets, actualTickets);
        Assert.AreEqual(2, actualTickets.Count);
        Assert.IsTrue(actualTickets.All(t => t.UserId == user.Id));
        Assert.IsFalse(actualTickets.Any(t => t.Price == 0));
    }

    [TestMethod]
    public void TestGetTotalPrice()
    {
        List<Ticket> tickets = TicketHandler.Tickets;
        double expectedTotalPrice = tickets.Sum(t => t.Price);

        double actualTotalPrice = TicketHandler.GetTotalPrice(tickets);

        Assert.AreEqual(expectedTotalPrice, actualTotalPrice);
        Assert.AreEqual(45, actualTotalPrice);
        Assert.AreNotEqual(50, actualTotalPrice);
        Assert.IsTrue(actualTotalPrice > 0);
    }
}
}