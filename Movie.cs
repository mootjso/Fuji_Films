public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public List<string> Genres { get; set; }
    public int Runtime { get; set; }
    public bool IsAdult { get; set; }

    public Movie(int id, string title, string language, string description, List<string> genres, int runtime, bool isAdult)
    {
        Id = id;
        Title = title;
        Language = language;
        Description = description;
        Genres = genres;
        Runtime = runtime;
        IsAdult = isAdult;
    }
}