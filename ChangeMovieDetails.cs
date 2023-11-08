using Newtonsoft.Json;
public static class ChangeMovieDetails 
{
    //split code to reuse for other methods
    public static void EditMovieDescription()
{
    List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
    const int pageSize = 10;
    int currentPage = 0;

    while (true)
    {
        int startIndex = currentPage * pageSize;
        int endIndex = Math.Min(startIndex + pageSize, movies.Count);
        List<Movie> pageMovies = movies.GetRange(startIndex, endIndex - startIndex);
        List<string> movieTitles = ShowHandler.GetMovieTitles(pageMovies);
        
        string menuText = $"Select a movie to edit (Page {currentPage + 1}):\n";
        List<string> menuOptions = new List<string>(movieTitles);
        menuOptions.AddRange(new List<string> { "[Previous Page]", "[Next Page]" });

        int index = Menu.Start(menuText, menuOptions, true);

 
        if (index == menuOptions.Count - 2 && currentPage > 0) // next page
        {
            currentPage--;
        }
        if (index == menuOptions.Count - 1 && endIndex < movies.Count)  // previous page
        {
            currentPage++;
        }
        else if (index == menuOptions.Count)
        {
            break;
        }
        else if (index >= 0 && index < movieTitles.Count)
        {
            Movie selectedMovie = pageMovies[index];
            Console.Clear();
            DisplayAsciiArt.AdminHeader();

            Console.WriteLine("Edit Movie Description");
            Console.WriteLine($"Title: {selectedMovie.Title}");
            Console.WriteLine($"Current Description: {selectedMovie.Description}");
            Console.WriteLine("Enter the new description:");
            Console.CursorVisible = true;
            string? newDescription = Console.ReadLine();
            string oldDescription = selectedMovie.Description;

            Console.Clear();
            DisplayAsciiArt.AdminHeader();

            Console.WriteLine("Edit Movie Description");
            Console.WriteLine($"Title: {selectedMovie.Title}");
            Console.WriteLine("Current Description:");
            Console.WriteLine(oldDescription);
            Console.WriteLine("New Description:");
            Console.WriteLine(newDescription);
            Console.Write("Confirm the change? (Y/N): ");
            char? confirmChangeChoice = char.ToUpper(Console.ReadKey().KeyChar);
            Console.CursorVisible = false;

            if (confirmChangeChoice == 'Y')
            {
                selectedMovie.Description = newDescription;
                string updatedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);
                File.WriteAllText(JSONMethods.MovieFileName, updatedJson);
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine("Description updated successfully!");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine("Description not changed.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }
    }
}
}

