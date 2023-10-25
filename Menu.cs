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

    public static int Start(string text, List<string> options)
    {
        selectedOption = 0;
        Console.CursorVisible = false;
        bool inMenu = true;
        while (inMenu)
        {
            Console.Clear();
            DisplayAsciiArt.Header();
            DisplayMenuLocation();

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
                case ConsoleKey.LeftArrow: // Added to let user go to previous screen without adding extra menu option
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
        Console.WriteLine("\n\nUse the up/down arrow keys to navigate, press Enter to select and left arrow to go back");
        Console.ResetColor();
    }

    private static void DisplayMenuLocation()
    {
        Console.ForegroundColor= ConsoleColor.DarkGray;
        Console.WriteLine();
        Console.ResetColor();
    }
}