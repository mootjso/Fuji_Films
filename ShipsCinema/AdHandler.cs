public static class AdHandler
{
    private static bool _picked = false;
    public static List<string> Ads = JSONMethods.ReadJSON<string>("ads.json").ToList();
    public static List<string> selectedSnacks = new();

    private static void PickSnacks()
    {   
        if (Ads.Count == 0)
        {
            selectedSnacks.Add("Stay tuned, snack enthusiasts! Exciting flavors and mouthwatering deals are opening soon!");
            return;
        }

        if (_picked)
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