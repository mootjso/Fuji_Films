namespace ShowHandlerTest
{
[TestClass]
public class ShowHandlerTest
{
    private static string _filename = ShowHandler.FileName;
    private static List<Show> original_File = JSONMethods.ReadJSON<Show>(_filename).ToList();

    [TestInitialize]
    public void TestInitialize()
    {
        // Clear the test JSON file
        File.WriteAllText(_filename, "[]");

        Movie movieObject = new(2, "Movie 2", "en", "kaas", new List<string> { "Family" }, 120, 12);
        List<Show> testShows = new List<Show>
        {
            new Show(1, 5, 2, new DateTime(2023, 1, 1, 12, 0, 0)),
            new Show(movieObject, new DateTime(2023, 2, 24, 19, 0, 0), 1),
            new Show(null, new DateTime(2023, 7, 2, 14, 0, 0), 2)
        };
        // JSONMethods.WriteToJSON(testShows, _filename);
    }

    [ClassCleanup]
    public static void CleanupJSON()
    {
       JSONMethods.WriteToJSON(original_File, _filename); 
    }

    [TestMethod]
    public void TestWriteShowsFromJSON()
    {
        List<Show> shows = JSONMethods.ReadJSON<Show>(_filename).ToList();
        // Show show1 = shows[0];
        // Show show_movie_object = shows[1];
        // Show show_null_movie = shows[2];

    //     // Assert.AreEqual(1, show1.Id);
    //     // Assert.AreEqual(5, show1.MovieId);
    //     // Assert.AreEqual(2, show1.TheaterNumber);
    //     // Assert.AreEqual(new DateTime(2023, 1, 1, 12, 0, 0), show1.DateAndTime);

    //     // Assert.AreEqual(2, show_movie_object.MovieId);
    //     // Assert.AreEqual(1, show_movie_object.TheaterNumber);
    //     // Assert.AreEqual(null, show_null_movie);
    }
}
}
