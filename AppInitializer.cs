public static class AppInitializer
{
    public const string MoviesFileName = MovieHandler.FileName;
    public const string ShowsFileName = ShowHandler.FileName;
    
    public static List<Movie> GetMovieObjects()
    {
        if (File.Exists(MoviesFileName))
        {
            return JSONMethods.ReadJSON<Movie>(MoviesFileName);
        }
        else
        {
            var movies = new List<Movie>();
            JSONMethods.WriteToJSON(movies, MoviesFileName);
            return movies;
        }
    }

    public static List<Show> GetShowObjects()
    {
        if (File.Exists(ShowsFileName))
        {
            return JSONMethods.ReadJSON<Show>(ShowsFileName);
        }
        else
        {
            var shows = new List<Show>();
            JSONMethods.WriteToJSON(shows, ShowsFileName);
            return shows;
        }
    }
}