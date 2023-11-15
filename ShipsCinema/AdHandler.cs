public static class AdHandler
{
    private static bool _picked = false;
    public static List<string> Ads = new()
    {
        "Savor the reel deal – popcorn perfection and drinks that truly quench your cinematic thirst!",
        "Unbox the blockbuster flavor: where popcorn crunch meets the refreshment rush!",
        "Movie magic in every bite and sip! Grab our popcorn and drinks for a taste of the cinema!",
        "Snacking is an art form here: our snacks and drinks are the masterpiece you've been craving.",
        "Bite-sized happiness, fizzy fun, and cinema-ready flavor – it's showtime for your taste buds!",
        "Get your taste buds ready for an epic adventure with our snack stars and thirst-quenching co-stars.",
        "From the red carpet to your cupholder, our snacks and drinks take you on a taste-filled journey.",
        "Let your cravings meet their match with our top-notch snacks and drinks, perfectly cast for your movie night.",
        "Snack spectacular: where popcorn dreams and drink delights take center stage.",
        "Elevate your movie experience with snacks that pop and drinks that fizz – the perfect cinematic duo!"
    };
    public static List<string> selectedSnacks = new();

    private static void PickSnacks()
    {   if (_picked)
            return;
        // Choose 3 random sentences and display them
        
        string Snack;
        int i = 0;
        Random rand = new();
        while (i < 3)
        {
            Snack = Ads[rand.Next(Ads.Count)];
            if (selectedSnacks.Contains(Snack))
                continue;
            selectedSnacks.Add(Snack);
            i++;
        }
        _picked = true;
    }

    public static void DisplaySnacks()
    {
        PickSnacks();
        Console.WriteLine("------- TASTY OFFERINGS -------");
        foreach (string Snack in selectedSnacks)
            Console.WriteLine(Snack);
        Console.WriteLine("-------------------------------");
        Console.WriteLine();
    }
}