using Newtonsoft.Json;

public class Show : HasID
{
    public int Id { get; set; }
    public int MovieId;
    public int TheaterNumber;
    public DateTime DateAndTime;

    public string DateString { get => DateAndTime.ToString("dd-MM-yyyy"); }
    public string StartTimeString { get => DateAndTime.ToString("HH:mm"); }
    public string EndTimeString { get => DateAndTime.AddMinutes(MovieHandler.GetMovieById(MovieId)!.Runtime).ToString("HH:mm"); }

    [JsonConstructor]
    public Show(int id, int movieId, int theaterNumber, DateTime dateAndTime)
    {
        Id = id;
        MovieId = movieId;
        TheaterNumber = theaterNumber;
        DateAndTime = dateAndTime;
    }

    public Show(Movie movie, DateTime startTime, int theaterNumber)
    {
        if (movie == null) 
            return;
        MovieId = movie.Id;
        DateAndTime = startTime;
        TheaterNumber = theaterNumber;
    }
}