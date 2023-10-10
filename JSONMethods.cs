using Newtonsoft.Json;

public static class JSONMethods
{
    public static string MovieFileName = "movies.json";
    public static List<Movie> ReadJSON(string fileName)
    {
        StreamReader reader = new(fileName);
        string content = reader.ReadToEnd();
        List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        return movies;
    }
}