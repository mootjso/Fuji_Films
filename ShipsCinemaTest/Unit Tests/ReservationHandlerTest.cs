namespace ReservationTest;

[TestClass]
public class ReservationHandlerTests
{
    private static string FileName = ReservationHandler.FileName;
    private static List<Reservation> original_File = JSONMethods.ReadJSON<Reservation>(FileName).ToList();

    [ClassCleanup]
    public static void CleanupJSON()
    {
       JSONMethods.WriteToJSON(original_File, FileName); 
    }

    [TestMethod]
    public void TestReservationID()
    {
        File.WriteAllText(FileName, "[]");

        List<Reservation> testReservations = new List<Reservation>
        {
            new Reservation("c5e9-9f7e-1985", 1, 1, 2, 8, 5),
            new Reservation("abc1-def2-ghi3", 2, 3, 4, 10, 7),
            new Reservation("xyz9-uvw8-pqr7", 3, 5, 6, 15, 3),

        };
        JSONMethods.WriteToJSON(testReservations, FileName);

        string reservationId = ReservationHandler.GetReservationID();

        Assert.IsNotNull(reservationId);
        Assert.AreEqual(14, reservationId.Length); // Check if the generated ID has the correct length
    }

}