public static class TestMethods
{
    public static void DisplayMovies()
    {
        List<Movie> movies = JSONMethods.ReadJSON(JSONMethods.MovieFileName);
        foreach (Movie movie in movies)
        {
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Language: {movie.Language}");
            Console.WriteLine($"Description: {movie.Description}");
            Console.Write("Genres: ");
            foreach (var genre in movie.Genres)
            {
                Console.Write($"{genre}; ");
            }
            Console.WriteLine();
            Console.WriteLine($"Runtime: {movie.Runtime}");
            Console.WriteLine($"IsAdult: {movie.IsAdult}");
            Console.WriteLine();
        }
    }
}