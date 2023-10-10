public static class AdminMenu
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
            }
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
            Console.WriteLine($"  {options[i].PadRight(25)}");
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("\n\nUse the up/down arrow keys to navigate, press Enter to select");
        Console.ResetColor();
    }

}
