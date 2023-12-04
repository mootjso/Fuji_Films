
namespace TicketPurchaseTests;

[TestClass]
public class ReservationHandlerTests
{
    private static string FileName = ReservationHandler.FileName;
    private static List<Reservation> original_FileName2 = JSONMethods.ReadJSON<Reservation>(FileName).ToList();


    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        // Clear the  JSON file
        File.WriteAllText(FileName, "[]");

        List<Reservation> testReservations = new List<Reservation>
        {
            new Reservation("c5e9-9f7e-1985", 1, 1, 2, 8, 5),
            new Reservation("abc1-def2-ghi3", 2, 3, 4, 10, 7),
            new Reservation("xyz9-uvw8-pqr7", 3, 5, 6, 15, 3),

        };
        JSONMethods.WriteToJSON(testReservations, FileName);

    }

    [ClassCleanup]
    public static void CleanupJSON()
    {
       JSONMethods.WriteToJSON(original_FileName2, FileName); 
    }

    [TestMethod]
    public void TestReservationID()
    {
        string reservationId = ReservationHandler.GetReservationID();

        Assert.IsNotNull(reservationId);
        Assert.AreEqual(14, reservationId.Length); // Check if the generated ID has the correct length
    }

    [TestMethod]
    public void TestWriteReservationsToJSON()
    {
        List<Reservation> reservations = JSONMethods.ReadJSON<Reservation>(FileName).ToList();
        Reservation reservationTest = reservations[2];

        Assert.AreEqual(3, reservations.Count); // Check if the number of reservations is correct
        
        Assert.AreEqual("xyz9-uvw8-pqr7", reservationTest.ReservationId);
        Assert.AreEqual(3, reservationTest.UserId);
        Assert.AreEqual(5, reservationTest.ShowId);
        Assert.AreEqual(6, reservationTest.MovieId);
        Assert.AreEqual(15, reservationTest.Row);
        Assert.AreNotEqual(4, reservationTest.Column);

    }
}