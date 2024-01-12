
﻿public static class DisplayAsciiArt

{
    public static void OpeningLogo()
    {
        FadeInEffect(AsciiArt.logo);

        Thread.Sleep(1800);
        Console.Clear();
    }

    public static void Header()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(AsciiArt.header);
        Console.WriteLine("---------------------------------------------------------------------");
        Console.ResetColor();
    }


    public static void AdminHeader()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(AsciiArt.AdminHeader);
        Console.WriteLine("---------------------------------------------------------------------");
        Console.ResetColor();
    }

    public static void Standby()
    {
        Console.CursorVisible = false;
        BlinkingEffect("\n\n\n\n         Press any key to start");
    }

    private static void FadeInEffect(string text)
    {
        ConsoleColor[] colors = { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White };

        foreach (ConsoleColor color in colors)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Thread.Sleep(400);
        }
        Thread.Sleep(300);
        Console.WriteLine("\n                    --An Ocean of Blockbusters Awaits--");
        Console.ResetColor();
    }

    private static void BlinkingEffect(string text)
    {
        bool isVisible = true;
        while (!Console.KeyAvailable)
        {
            Console.Clear();
            Header();

            if (isVisible)
            {
                Console.WriteLine(text);
                Thread.Sleep(600);
            }
            else
                Thread.Sleep(500);

            isVisible = !isVisible;
        }
        Console.ReadKey(true);
    }
}