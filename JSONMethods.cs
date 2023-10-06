using Newtonsoft.Json;

public static class JSONMethods
{
    public static string MovieFileName = "movies.json";
    public static List<Movie> ReadJSON(string fileName)
    {
        StreamReader reader = new(fileName);
        string content = reader.ReadToEnd();
        List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        reader.Close();
        return movies;
    }

    public static List<ScheduledMovie> ReadJSONSchedule(string fileName)
    {
        StreamReader reader = new(fileName);
        string content = reader.ReadToEnd();
        List<ScheduledMovie> movies = JsonConvert.DeserializeObject<List<ScheduledMovie>>(content);
        reader.Close();
        return movies;
    }

    public static void WriteToJSONSchedule(List<ScheduledMovie> movieList, string fileName)
    {
        StreamWriter writer = new(fileName);
        string ListToJson = JsonConvert.SerializeObject(movieList);
        writer.Write(ListToJson);
        writer.Close();
    }
}