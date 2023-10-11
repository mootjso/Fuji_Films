using Newtonsoft.Json;

public static class JSONMethods
{
    public static string MovieFileName = "movies.json";
    public static List<T> ReadJSON<T>(string fileName)
    {
        if (File.Exists(fileName))
        {
            long fileSize = new FileInfo(fileName).Length;
            if (fileSize > 0)
            {
                StreamReader reader = new(fileName);
                string content = reader.ReadToEnd();
                List<T> movies = JsonConvert.DeserializeObject<List<T>>(content)!;
                reader.Close();

                return movies;
            }
        }
        else
        {
            File.WriteAllText(fileName, "[]");
        }

        return new List<T>();
    }

    public static void WriteToJSONSchedule(List<ScheduledMovie> movieList, string fileName)
    {
        StreamWriter writer = new(fileName);
        string ListToJson = JsonConvert.SerializeObject(movieList);
        writer.Write(ListToJson);
        writer.Close();
    }
}