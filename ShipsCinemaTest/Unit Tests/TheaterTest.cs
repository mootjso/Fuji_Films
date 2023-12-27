using System.Runtime.CompilerServices;

namespace TheaterTests;

[TestClass]
public class TheaterTests
{
    private List<Theater> _theatersCopy = new();

    private string _fileName = TheaterHandler.FileName;
    static Movie movieObject = new Movie(3, "testMovie", "en", "A Movie description", new List<string> { "Adventure", "Drama" }, 120, 12);

    [TestInitialize]
    public void TestInitialize()
    {
        _theatersCopy = JSONMethods.ReadJSON<Theater>(_fileName).ToList();
        JSONMethods.WriteToJSON(new List<Theater>(), _fileName);

    }

    [TestCleanup]
    public void TestCleanup()
    {
        JSONMethods.WriteToJSON(_theatersCopy, _fileName);
    }

    [DataTestMethod]
    [DataRow(1, 3, 1, "2023-12-14T12:20:40")]
    [DataRow(4, 7, 3, "2023-11-22T11:50:00")]
    [DataRow(0, 4, 2, "2020-6-16T14:48:05")]
    public void TestCreateTheater(int id, int movieid, int theaternumber, string datetimesstring)
    {
        DateTime dateTime = DateTime.Parse(datetimesstring);

        Show show = new Show(id, movieid, theaternumber, dateTime);
        Theater CreatedTheater = TheaterHandler.CreateOrGetTheater(show);

        Assert.IsNotNull(CreatedTheater);
    }

    [DataTestMethod]
    [DataRow(1, 150)]
    [DataRow(4, 500)]
    [DataRow(0, 300)]
    public void TestTheaterByShowId(int showid, int numofseats)
    {
        Theater foundTheater = TheaterHandler.GetTheaterByShowId(showid);

        Assert.AreEqual(foundTheater.NumOfSeats, numofseats);
    }
}