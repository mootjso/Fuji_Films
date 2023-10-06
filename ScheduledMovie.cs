public class ScheduledMovie
{
    public static int Counter = 0;
    public int Id;
    public DateTime StartTime;
    public DateTime EndTime;
    public int Room = 0; // TODO Add the specific cinema room
    public Movie Movie;

    public ScheduledMovie(Movie movie, DateTime startTime)
    {
        Counter++;
        Id = Counter;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(movie.Runtime);
        Movie = movie;
    }
}