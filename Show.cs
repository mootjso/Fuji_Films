public class Show
{
    public int Id; // Id gets set in the ShowHandler Class
    public int MovieId;
    public int Room = 0; // TODO Add the specific cinema room
    public DateTime DateAndTime;
    public List<Seat> SeatList = new();

    public string DateString { get => DateAndTime.ToString("dd-MM-yyyy"); }
    public string StartTimeString { get => DateAndTime.ToString("HH:mm"); }
    public string EndTimeString { get; }

    public Show(Movie movie, DateTime startTime)
    {
        if (movie == null) return;
        MovieId = movie.Id;
        DateAndTime = startTime;
        EndTimeString = DateAndTime.AddMinutes(movie.Runtime).ToString("HH:mm");
    }
}