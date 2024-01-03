using Newtonsoft.Json;

public static class ChangeMovieDetails
{
    public static void EditMovieInfo()
    {
        List<Movie> movies = JSONMethods.ReadJSON<Movie>(JSONMethods.MovieFileName).ToList();

        if (movies.Count == 0)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine("Change movie details");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nCan't change movie details as there are no movies available");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to go back");
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
            List<string> movieTitles = MovieHandler.GetMovieTitles(pageMovies);

            string menuText = $"Select a movie to edit (Page {currentPage + 1}):\n\n" +
                $"  {"Title",-30} | {"Language",-10} | {"Genres",-30} | {"Runtime",-8} | Age\n" +
                $"  {new string('-', 93)}";
            List<string> menuOptions = new List<string>(movieTitles);
            menuOptions.AddRange(new List<string> { "  Previous Page", "  Next Page", "  Back" });

            int index = Menu.Start(menuText, menuOptions, true, true);


            if (index == menuOptions.Count - 3 && currentPage > 0) // next page
            {
                currentPage--;
            }
            if (index == menuOptions.Count - 2 && endIndex < movies.Count)  // previous page
            {
                currentPage++;
            }

            else if (index == menuOptions.Count || index == menuOptions.Count - 1)
            {
                break;
            }
            else if (index >= 0 && index < movieTitles.Count)
            {
                string selectedTitle = movieTitles[index].Split("|")[0].Trim().Replace("...", "");
                Movie selectedMovieToEdit = null;

                foreach (Movie movie in movies)
                {
                    if (movie.Title.Contains(selectedTitle))
                    {
                        selectedMovieToEdit = movie;
                        break; // Exit the loop once a matching movie is found
                    }
                }

                EditMovieDetail(selectedMovieToEdit, movies);

            }
        }
    }



    public static void EditMovieDetail(Movie selectedMovieToEdit, List<Movie> movies)
    {
        while (true)
        {
            string menuText = $"Select a movie detail to edit in {selectedMovieToEdit.Title}:";
            string movieDescription = selectedMovieToEdit.Description;
            string formattedMovieDescription = movieDescription.Length >= 13 ? movieDescription.Substring(0, 10) + "..." : movieDescription;

            List<string> menuOptions = new List<string>
        {
            $"Title: {selectedMovieToEdit.Title}",
            $"Description: {formattedMovieDescription}",
            $"Language: {selectedMovieToEdit.Language}",
            $"Genres: {string.Join(", ", selectedMovieToEdit.Genres)}",
            $"Runtime: {selectedMovieToEdit.Runtime} minutes",
            $"AgeRating: {selectedMovieToEdit.AgeRating}+",
            " Back"
        };

            int selectedIndex = Menu.Start(menuText, menuOptions, true);

            if (selectedIndex == menuOptions.Count || selectedIndex == menuOptions.Count - 1) // return to the previous menu
            {
                break;
            }
            else
            {
                bool rightInput = false;
                string newValue;
                string selectedOption = menuOptions[selectedIndex].Substring(0, menuOptions[selectedIndex].IndexOf(':'));

                do
                {
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine($"Editing the {selectedOption} for '{selectedMovieToEdit.Title}'\n");
                    Console.WriteLine($"Current {selectedOption}: {GetMovieDetail(selectedMovieToEdit, selectedOption)}");
                    if (selectedIndex == 1) // Change description
                        newValue = EditMovieDescription(selectedMovieToEdit.Description);
                    else
                    {
                        Console.Write($"\nEnter the new {selectedOption}: ");
                        Console.CursorVisible = true;
                        newValue = Console.ReadLine();
                    } 
                    Console.CursorVisible = false;

                    if (ValidateInput(selectedOption, newValue))
                    {
                        rightInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        DisplayAsciiArt.AdminHeader();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Invalid input for {selectedOption}. Please enter a valid value.");
                        Console.ResetColor();
                        Console.WriteLine("\n\nPress any key to continue");
                        Console.ReadKey();
                    }
                } while (!rightInput);

                Console.Clear();
                DisplayAsciiArt.AdminHeader();
                Console.WriteLine($"Confirm Changes for '{selectedMovieToEdit.Title}':\n");
                Console.WriteLine($"Old {selectedOption}: \n{GetMovieDetail(selectedMovieToEdit, selectedOption)}");
                Console.WriteLine($"\nNew {selectedOption}: \n{newValue}");
                Console.CursorVisible = false;

                Console.Write("\n[Y] Confirm Changes\n[N] Cancel Changes");
                char? confirmChangeChoice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.CursorVisible = false;

                if (confirmChangeChoice == 'Y')
                {
                    // Update the movie detail
                    UpdateMovieDetail(selectedMovieToEdit, selectedOption, newValue);

                    // Save changes to JSON file
                    string updatedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);
                    File.WriteAllText(JSONMethods.MovieFileName, updatedJson);

                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Movie {selectedOption} for '{selectedMovieToEdit.Title}' is updated successfully!");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    DisplayAsciiArt.AdminHeader();
                    Console.WriteLine($"Editing the {selectedOption} for '{selectedMovieToEdit.Title}'\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Changes discarded");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadKey();
                }
            }
        }
    }

    private static string GetMovieDetail(Movie movie, string selectedOption)
    {
        switch (selectedOption)
        {
            case "Title": return movie.Title;
            case "Description": return movie.Description;
            case "Language": return movie.Language;
            case "Genres": return string.Join(", ", movie.Genres);
            case "Runtime": return movie.Runtime.ToString();
            case "AgeRating": return movie.AgeRating.ToString();
            default: return "";
        }
    }

    private static void UpdateMovieDetail(Movie movie, string selectedOption, string newValue)
    {
        switch (selectedOption)
        {
            case "Title": movie.Title = newValue; break;
            case "Description": movie.Description = newValue; break;
            case "Language": movie.Language = newValue; break;
            case "Genres": movie.Genres = newValue.Split(", ").ToList(); break;
            case "Runtime": movie.Runtime = int.Parse(newValue); break;
            case "AgeRating": movie.AgeRating = int.Parse(newValue); break;

        }
    }

    private static bool ValidateInput(string selectedOption, string newValue)
    {
        switch (selectedOption)
        {
            case "Runtime":
            case "AgeRating":
                return int.TryParse(newValue, out _);
            case "Language":
                return !newValue.Any(char.IsDigit); // check whether the new value contains any digits
            default:
                return true; // No specific validation for other options
        }
    }

    public static string EditMovieDescription(string description)
    {
        const int maxCharactersPerLine = 100;
        string controls = "\nType to add new information\n[Arrow keys] Navigate\n[Backspace] Remove text\n[Enter] Confirm";
        int cursorPosition = 0;
        Console.CursorVisible = true;

        while (true)
        {
            Console.Clear();
            DisplayAsciiArt.AdminHeader();
            Console.WriteLine($"Edit description:\n");
            for (int i = 0; i < description.Length; i += maxCharactersPerLine)
            {
                int remainingChars = Math.Min(maxCharactersPerLine, description.Length - i);
                Console.WriteLine(description.Substring(i, remainingChars));
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine(controls);
            Console.ResetColor();
            Console.SetCursorPosition(cursorPosition % maxCharactersPerLine, 8 + (cursorPosition / maxCharactersPerLine));

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    cursorPosition = Math.Max(0, cursorPosition - 1);
                    break;

                case ConsoleKey.RightArrow:
                    cursorPosition = Math.Min(description.Length, cursorPosition + 1);
                    break;

                case ConsoleKey.UpArrow:
                    cursorPosition = Math.Max(0, cursorPosition - maxCharactersPerLine);
                    break;

                case ConsoleKey.DownArrow:
                    cursorPosition = Math.Min(description.Length, cursorPosition + maxCharactersPerLine);
                    break;

                case ConsoleKey.Backspace:
                    if (cursorPosition > 0)
                    {
                        description = description.Remove(cursorPosition - 1, 1);
                        cursorPosition--;
                    }
                    break;

                case ConsoleKey.Enter:
                    Console.CursorVisible = false;
                    return description;

                default:
                    description = description.Insert(cursorPosition, keyInfo.KeyChar.ToString());
                    cursorPosition++;
                    break;
            }
        }
    }

}