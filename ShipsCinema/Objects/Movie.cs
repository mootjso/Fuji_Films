public class Movie
{
    public int Id { get; set; }
    public static int HighestId { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public List<string> Genres { get; set; }
    public int Runtime { get; set; }
    public int AgeRating { get; set; }

    public Movie(int highestId, string title, string language, string description, List<string> genres, int runtime, int ageRating)
    {
        Id = ++highestId;
        Title = title;
        Language = language;
        Description = description;
        Genres = genres;
        Runtime = runtime;
        AgeRating = ageRating;
    }
}