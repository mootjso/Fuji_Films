using Newtonsoft.Json;

public static class JSONMethods
{
    public static string MovieFileName = "movies.json";
    public static IEnumerable<T> ReadJSON<T>(string fileName)
    {
        if (File.Exists(fileName))
        {
            long fileSize = new FileInfo(fileName).Length;
            if (fileSize > 0)
            {
                StreamReader reader = new(fileName);
                string content = reader.ReadToEnd();
                IEnumerable<T> objList = JsonConvert.DeserializeObject<List<T>>(content)!;
                reader.Close();

                return objList;
            }
        }
        else
        {
            File.WriteAllText(fileName, "[]");
        }

        return Enumerable.Empty<T>();
    }

    public static void WriteToJSON<T>(List<T> objList, string fileName)
    {
        StreamWriter writer = new(fileName);
        string ListToJson = JsonConvert.SerializeObject(objList);
        writer.Write(ListToJson);
        writer.Close();
    }
}