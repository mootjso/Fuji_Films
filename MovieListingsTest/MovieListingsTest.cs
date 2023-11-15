namespace MovieListingsTest
{
    [TestClass]
    public class MovieListingsTest
    {
        private List<Movie> _moviesCopy = new();
        private string _fileName = MovieHandler.FileName;

        [TestInitialize]
        public void TestInitialize()
        {
            _moviesCopy = JSONMethods.ReadJSON<Movie>(_fileName).ToList();
            JSONMethods.WriteToJSON(new List<Movie>(), _fileName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            JSONMethods.WriteToJSON(_moviesCopy, _fileName);
        }

        [TestMethod]
        public void TestReadMoviesFromJSON()
        {
            List<Movie> movieToWrite = new()
        {
            new Movie(1, "Movie1", "en", "First Movie", new List<string> { "Adventure", "Horror" }, 105, 18)
        };
            JSONMethods.WriteToJSON(movieToWrite, _fileName);
            List<Movie> readMovie = JSONMethods.ReadJSON<Movie>(_fileName).ToList();
            Movie movie = readMovie[0];
            Assert.AreEqual(1, movie.Id);
            Assert.AreEqual("Movie1", movie.Title);
            Assert.AreEqual("en", movie.Language);
            Assert.AreEqual("First Movie", movie.Description);
            CollectionAssert.AreEqual(new List<string> { "Adventure", "Horror" }, movie.Genres);
            Assert.AreEqual(105, movie.Runtime);
            Assert.AreEqual(18, movie.AgeRating);
        }
    }

}
