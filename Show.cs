public class Show
{
    public int Id;
    public int MovieId;
    public int Room = 0; // TODO Add the specific cinema room
    public DateTime DateAndTime;
    // TODO Add a list of seats

    public string DateString { get => DateAndTime.ToString("dd-MM-yyyy"); }
    public string StartTimeString { get => DateAndTime.ToString("HH:mm"); }
    public string EndTimeString { get => DateAndTime.AddMinutes(MovieHandler.GetMovieById(MovieId)!.Runtime).ToString("HH:mm"); }

    public Show(Movie movie, DateTime startTime)
    {
        if (movie == null) return;
        MovieId = movie.Id;
        DateAndTime = startTime;
    }
}