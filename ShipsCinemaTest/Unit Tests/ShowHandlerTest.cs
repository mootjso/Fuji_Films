namespace ShowHandlerTest
{
[TestClass]
public class ShowHandlerTest
{
    private List<Show> _showsCopy = new();
    private string _fileName = ShowHandler.FileName;
    static Movie movieObject = new Movie(3, "Movie", "en", "A Movie", new List<string> { "Adventure", "Horror" }, 105, 18);
    List<Show> testshows = new() 
    {
        new Show(1, 1, 1, new DateTime(2023, 12, 14, 12, 20, 40)),
        new Show(9, 5, 2, new DateTime(2023, 11, 26, 17, 2, 12)),
        new Show(movieObject, new DateTime(2020, 6, 16, 14, 48, 5), 1),
    };

    [TestInitialize]
    public void TestInitialize()
    {   
        _showsCopy = JSONMethods.ReadJSON<Show>(_fileName).ToList();
        JSONMethods.WriteToJSON(new List<Show>(), _fileName);

        JSONMethods.WriteToJSON(testshows, _fileName);
    }

    [TestCleanup]
    public void TestCleanUp()
    {
        JSONMethods.WriteToJSON(_showsCopy, _fileName);
    }

    [DataTestMethod]
    [DataRow(9, 5, 2, "2023-11-26T17:02:12")]
    [DataRow(1, 1, 1, "2023-12-14T12:20:40")]
    [DataRow(0, 4, 1, "2020-06-16T14:48:05")]
    public void TestGetShowById(int getById, int movieid, int theaternumber, string dateAndTimeString)
    {   
        Show? showResult = ShowHandler.GetShowById(getById);
        DateTime dateAndTime = DateTime.Parse(dateAndTimeString);
        Show showExpected = new Show(getById, movieid, theaternumber, dateAndTime);
        Assert.AreEqual(showExpected.MovieId, showResult.MovieId);
        Assert.AreEqual(showExpected.Id, showResult.Id);
        Assert.AreEqual(showExpected.TheaterNumber, showResult.TheaterNumber);
        Assert.AreEqual(showExpected.DateAndTime, showResult.DateAndTime);
    }

    [DataTestMethod]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(5)]
    public void TestGetShowByIdNotFound(int getById)
    {   
        Show? showResult = ShowHandler.GetShowById(getById);
     
        Assert.IsNull(showResult);
    }

    [DataTestMethod]
    [DataRow("2023-11-26", 1)]
    [DataRow("2023-12-14", 1)]
    [DataRow("2020-6-16", 1)]
    public void TestGetMoviebyDate(string dateString, int expectedShowCount)
    {
        DateTime targetDate = DateTime.Parse(dateString);
        List<Show> result = ShowHandler.GetShowsByDate(targetDate);

        Assert.IsNotNull(result);
        Assert.AreEqual(expectedShowCount, result.Count);

        List<Show> expectedShows = ShowHandler.Shows.Where(show => show.DateAndTime.Date == targetDate.Date).ToList();
        CollectionAssert.AreEqual(expectedShows, result);
    }

    [TestMethod]
    public void TestGetMovieTitles()
    {
        List<Movie> movies = new List<Movie>
        {
            new Movie(1, "Movie1", "ne", "Description1", new List<string> { "Action, Romance" }, 120, 16),
            new Movie(2, "Movie2", "cn", "Description2", new List<string> { "Drama" }, 110, 18),
            new Movie(3, "Movie3", "ru", "Description3", new List<string> { "Fantasy" }, 130, 12),
        };

        List<string> result = MovieHandler.GetMovieTitles(movies).Select(movie => movie.Split("|")[0].Trim()).ToList();

        List<string> expectedTitles = movies.Select(movie => movie.Title).ToList();
        CollectionAssert.AreEqual(expectedTitles, result);
    }
    }

}

