using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

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
        File.WriteAllText(ShowHandler.FileName, "[]");
        ShowHandler.Shows.Clear();
        foreach (Show show in testshows)
        {
            ShowHandler.Shows.Add(show);
        }
    }
    [TestCleanup]
    public void TestCleanUp()
    {
        JSONMethods.WriteToJSON(_showsCopy, _fileName);

    }

    [DataTestMethod]
    [DataRow(0, "14-12-2023")]
    [DataRow(1, "26-11-2023")]
    [DataRow(2, "16-06-2020")]
    public void TestDateStringCorrect(int indexshow, string expectedOutput)
    {
        string DateStringShow = testshows[indexshow].DateString;

        Assert.AreEqual(expectedOutput, DateStringShow);
    }

    [DataTestMethod]
    [DataRow(0, "14-12-2021")]
    [DataRow(1, "26-07-2023")]
    [DataRow(2, "25-12-2021")]
    public void TestDateStringIncorrect(int indexshow, string expectedOutput)
    {
        string DateStringShow = testshows[indexshow].DateString;

        Assert.AreNotEqual(expectedOutput, DateStringShow);
    }

    [DataTestMethod]
    [DataRow(0, "12:20")]
    [DataRow(1, "17:02")]
    [DataRow(2, "14:48")]
    public void TestStartTimeStringCorrect(int indexshow, string expectedOutput)
    {
        string startTimeShow = testshows[indexshow].StartTimeString;

        Assert.AreEqual(expectedOutput, startTimeShow);
    }

    [DataTestMethod]
    [DataRow(0, "11:23")]
    [DataRow(1, "14:35")]
    [DataRow(2, "18:18")]
    public void TestStartTimeStringIncorrect(int indexshow, string expectedOutput)
    {
        string startTimeShow = testshows[indexshow].StartTimeString;

        Assert.AreNotEqual(expectedOutput, startTimeShow);
    }

    [DataTestMethod]
    [DataRow(0, 1, 1, 1, "2023-12-14T12:20:40")]
    [DataRow(1, 9, 5, 2, "2023-11-26T17:02:12")]
    [DataRow(2, 0, 3, 1, "2020-06-16T14:48:05")]
    public void TestConstructorCorrectProperties(int indexshow, int expectedId, int expectedMovieId, int expectedTheaterNumber, string expectedDateAndTime)
    {
        Show show = testshows[indexshow];
        DateTime expectedDateTime = DateTime.Parse(expectedDateAndTime);

        Assert.AreEqual(expectedId, show.Id);
        Assert.AreEqual(expectedMovieId, show.MovieId);
        Assert.AreEqual(expectedTheaterNumber, show.TheaterNumber);
        Assert.AreEqual(expectedDateTime, show.DateAndTime);
    }

    [DataTestMethod]
    [DataRow(0, 2, 6, 4, "2023-12-16T12:25:40")]
    [DataRow(1, 4, 2, 6, "2023-11-22T17:02:12")]
    [DataRow(2, 4, 2, 6, "2020-06-19T14:48:05")]
    public void TestConstructorIncorrectProperties(int indexshow, int expectedId, int expectedMovieId, int expectedTheaterNumber, string expectedDateAndTime)
    {
        Show show = testshows[indexshow];
        DateTime expectedDateTime = DateTime.Parse(expectedDateAndTime);

        Assert.AreNotEqual(expectedId, show.Id);
        Assert.AreNotEqual(expectedMovieId, show.MovieId);
        Assert.AreNotEqual(expectedTheaterNumber, show.TheaterNumber);
        Assert.AreNotEqual(expectedDateTime, show.DateAndTime);
    }

    [DataTestMethod]
    [DataRow(9, 5, 2, "2023-11-26T17:02:12")]
    [DataRow(1, 1, 1, "2023-12-14T12:20:40")]
    [DataRow(0, 3, 1, "2020-06-16T14:48:05")]
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

        List<string> result = ShowHandler.GetMovieTitles(movies);

        List<string> expectedTitles = movies.Select(movie => movie.Title).ToList();
        CollectionAssert.AreEqual(expectedTitles, result);
    }

    }

}

