public class ScheduledMovie
{
    public int Id = 0;
    public DateTime StartTime;
    public DateTime EndTime; // TODO Automatically set Endtime to the starttime + runtime
    public int Room = 0; // TODO Add the specific cinema room
    public Movie Movie;

    public ScheduledMovie(Movie movie, DateTime startTime)
    {
        Id++;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(movie.Runtime);
        Movie = movie;
    }
}