public static class AppInitializer
{
    public const string MoviesFileName = MovieHandler.FileName;
    public const string ShowsFileName = ShowHandler.FileName;
    public const string TheatersFileName = TheaterHandler.FileName;
    public const string TicketsFileName = TicketHandler.FileName;


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

    public static List<Theater> GetTheaterObjects()
    {
        if (File.Exists(ShowsFileName))
        {
            return JSONMethods.ReadJSON<Theater>(TheatersFileName);
        }
        else
        {
            var theaters = new List<Theater>();
            JSONMethods.WriteToJSON(theaters, TheatersFileName);
            return theaters;
        }
    }

    public static List<Ticket> GetTicketObjects()
    {
        if (File.Exists(TicketsFileName))
        {
            return JSONMethods.ReadJSON<Ticket>(TicketsFileName);
        }
        else
        {
            var tickets = new List<Ticket>();
            JSONMethods.WriteToJSON(tickets, TicketsFileName);
            return tickets;
        }
    }
}