using Newtonsoft.Json;
public static class ChangeMovieDetails 
{
    public static void EditMovieDescription()
{
    List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();
    
    if (movies.Count == 0)
    {
        Console.Clear();
        DisplayAsciiArt.AdminHeader();
        Console.WriteLine("Change movie details");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n\nCan't change movie details as there are no movies available");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n\nPress any key to go back");
        Console.ResetColor();
        Console.ReadKey();
        return;
    }
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
            string selectedTitle = movieTitles[index];
            Movie selectedMovieToEdit = movies.FirstOrDefault(movie => movie.Title == selectedTitle);
            EditMovieDetail(selectedMovieToEdit, movies);
    
        }
    }
}



public static void EditMovieDetail(Movie selectedMovieToEdit, List<Movie> movies)
{
    const int pageSize = 10;
    int currentPage = 0;

    while (true)
    {
        int startIndex = currentPage * pageSize;
        int endIndex = Math.Min(startIndex + pageSize, movies.Count);
        List<Movie> pageMovies = movies.GetRange(startIndex, endIndex - startIndex);

        string menuText = $"Select a movie detail to edit in {selectedMovieToEdit.Title}:\n";
        List<string> menuOptions = new List<string>
        {
            "Id", "Title", "Description", "Language", "Genres", "Runtime", "IsAdult"
        };

        int selectedIndex = Menu.Start(menuText, menuOptions);

        if (selectedIndex == menuOptions.Count - 3 && currentPage > 0) // previous page
        {
            currentPage--;
        }
        if (selectedIndex == menuOptions.Count - 2 && endIndex < movies.Count)  // next page
        {
            currentPage++;
        }
        else if (selectedIndex == menuOptions.Count) // return to the previous menu
        {
            break;
        }
        else
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();

            string selectedOption = menuOptions[selectedIndex];
            // Movie selectedMovie = selectedMovieToEdit;

            Console.WriteLine($"Editing Movie Detail for '{selectedMovieToEdit.Title}': {selectedOption}");
            Console.WriteLine($"Current {selectedOption}: {GetMovieDetail(selectedMovieToEdit, selectedOption)}");
            Console.Write($"Enter the new {selectedOption}: ");

            string newValue = Console.ReadLine();

            // Show confirmation
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Confirm Changes for '{selectedMovieToEdit.Title}':");
            Console.WriteLine($"Old {selectedOption}: {GetMovieDetail(selectedMovieToEdit, selectedOption)}");
            Console.WriteLine($"New {selectedOption}: {newValue}");
            Console.Write("Confirm changes (Y/N): ");
            char? confirmChangeChoice = char.ToUpper(Console.ReadKey().KeyChar);

            if (confirmChangeChoice == 'Y')
            {
                // Update the movie detail
                UpdateMovieDetail(selectedMovieToEdit, selectedOption, newValue);

                // Save changes to JSON file
                string updatedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);
                File.WriteAllText(JSONMethods.MovieFileName, updatedJson);

                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine($"Movie {selectedOption} for '{selectedMovieToEdit.Title}' updated successfully!");
                // Console.WriteLine("\nMake sure to refresh the movie menu to see the changes.");
                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine("Changes discarded. Press any key to continue.");
                Console.ReadKey();
            }
        }
    }
}



    private static string GetMovieDetail(Movie movie, string selectedOption)
    {
        switch (selectedOption)
        {
            case "Id": return movie.Id.ToString();
            case "Title": return movie.Title;
            case "Description": return movie.Description;
            case "Language": return movie.Language;
            case "Genres": return string.Join(", ", movie.Genres);
            case "Runtime": return movie.Runtime.ToString();
            case "IsAdult": return movie.IsAdult.ToString();
            default: return "";
        }
    }

    private static void UpdateMovieDetail(Movie movie, string selectedOption, string newValue)
    {
        switch (selectedOption)
        {
            case "Id": movie.Id = int.Parse(newValue); break;
            case "Title": movie.Title = newValue; break;
            case "Description": movie.Description = newValue; break;
            case "Language": movie.Language = newValue; break;
            case "Genres": movie.Genres = newValue.Split(',').ToList(); break;
            case "Runtime": movie.Runtime = int.Parse(newValue); break;
            case "IsAdult": movie.IsAdult = bool.Parse(newValue); break;
        }
    }
}



