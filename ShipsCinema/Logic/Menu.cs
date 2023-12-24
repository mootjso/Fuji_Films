/*
 * HOW TO USE MENU
 * You need 2 parameters
 *      string text: the text to be displayed above the menu
 *      List<string> options: a list with the options the user can select
 * 
 * Call Menu.Start(text, options).
 * When the user selects an option the method returns an integer that corresponds to the position in the list.
 * NOTE: The Menu Automatically displays the header at the top.
 */
public static class Menu
{
    static int selectedOption = 0;
    static ConsoleKeyInfo keyInfo;

    public static int Start(string text, List<string> options, bool IsAdmin = false)
    {
        selectedOption = 0;
        Console.CursorVisible = false;
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            if (IsAdmin)
            {
                DisplayAsciiArt.AdminHeader();
            }
            else
            {
                DisplayAsciiArt.Header();
                AdHandler.DisplaySnacks();
            }            
          
            DisplayOptions(text, options);
            keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOption > 0)
                        selectedOption--;
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOption < options.Count - 1)
                        selectedOption++;
                    break;
                case ConsoleKey.Enter:
                    inMenu = false;
                    return selectedOption;
                case ConsoleKey.Escape: // Added to let user go to previous screen without adding extra menu option
                    inMenu = false;
                    return options.Count;
            }
            Console.Clear();
        }
        return -1;
    }

    private static void DisplayOptions(string text, List<string> options)
    {
        Console.WriteLine(text);

        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedOption)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"  {options[i].PadRight(20)}");
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("\nControls:\n[Up/down arrow keys] Navigation\n[Enter] Select an option\n[Esc] Go back");
        Console.ResetColor();
    }

    public static bool MenuPagination<T>(List<string> menuOptionsFull, string menuText, string messageWhenEmpty, Func<T, bool> func, List<T>? menuOptionsFullObjects = null, bool isAdmin = false)
    {
        if (menuOptionsFullObjects is null)
            menuOptionsFullObjects = menuOptionsFull.Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
        List<string> menuOptions;
        if (menuOptionsFull.Count >= 10)
            menuOptions = menuOptionsFull.GetRange(0, 10);
        else
            menuOptions = menuOptionsFull.GetRange(0, menuOptionsFull.Count);

        if (menuOptions.Count <= 0)
        {
            List<string> menuOption = new() { "Back" };
            Start(messageWhenEmpty, menuOption, isAdmin);
            return true;
        }
        menuOptions.AddRange(new List<string> { "  Previous Page", "  Next Page", "  Back" });
        int pageNumber = 0;
        int pageSize = 10;
        int maxPages = Convert.ToInt32(Math.Ceiling((double)menuOptionsFull.Count / pageSize));
        int firstOptionIndex;
        int endIndex;

        while (true)
        {
            int selection = Start(menuText, menuOptions, isAdmin);
            if (selection == menuOptions.Count)
                break; // Go back to main menu
            else if (selection == menuOptions.Count - 2 && pageNumber < (maxPages - 1)) // Next page
                pageNumber++;
            else if (selection == menuOptions.Count - 3 && pageNumber != 0) // Previous page
                pageNumber--;
            else if (selection == menuOptions.Count - 1)
                return true;
            else if (selection >= 0 && selection < menuOptions.Count - 3)
            {
                selection += (pageNumber * 10);
                T obj = menuOptionsFullObjects[selection];
                bool isRemoved = func(obj);
                // Check if object has been deleted, return if yes
                // That way you don't go back to the  menu, deleted object would still be visible there
                if (isRemoved)
                {
                    return false;
                }
            }
            firstOptionIndex = pageSize * pageNumber;
            // Prevent Error when page has less than 10 entries
            endIndex = menuOptionsFull.Count % 10;
            if (endIndex != 0 && pageNumber == maxPages - 1)
                menuOptions = menuOptionsFull.GetRange(firstOptionIndex, endIndex);
            else
                menuOptions = menuOptionsFull.GetRange(firstOptionIndex, pageSize);
            menuOptions.AddRange(new List<string> { "  Previous Page", "  Next Page", "  Back" });
        }
        return false;
    }
}