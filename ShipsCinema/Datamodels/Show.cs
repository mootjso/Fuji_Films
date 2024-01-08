using Newtonsoft.Json;

public class Show
{
    public int Id;
    public int MovieId;
    public int TheaterNumber;
    public bool Removed;
    public DateTime DateAndTime;

    public string DateString { get => DateAndTime.ToString("dd-MM-yyyy"); }
    public string StartTimeString { get => DateAndTime.ToString("HH:mm"); }
    public string EndTimeString { get => DateAndTime.AddMinutes(MovieHandler.GetMovieById(MovieId)!.Runtime).ToString("HH:mm"); }

    [JsonConstructor]
    public Show(int id, int movieId, int theaterNumber, DateTime dateAndTime, bool removed = false)
    {
        Id = id;
        MovieId = movieId;
        TheaterNumber = theaterNumber;
        DateAndTime = dateAndTime;
        Removed = removed;
    }

    public Show(Movie movie, DateTime startTime, int theaterNumber, bool removed = false)
    {
        if (movie == null) 
            return;
        MovieId = movie.Id;
        DateAndTime = startTime;
        TheaterNumber = theaterNumber;
        Removed = removed;
    }
}