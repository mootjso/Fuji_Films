public static class AdHandler
{
    private static bool _picked = false;
    public static List<string> Ads = JSONMethods.ReadJSON<string>("Datasources/ads.json").ToList();
    public static AdsContainer AdsContainer = new AdsContainer();

    private static void PickSnacks()
    {   
        if (Ads.Count == 0 && AdsContainer.AdsCount == 0)
        {
            AdsContainer.AddMessage("Stay tuned, snack enthusiasts! Exciting flavors and mouthwatering deals are opening soon!");
            _picked = true;
            return;
        }

        if (_picked)
            return;
        // Choose 3 random sentences and display them
        
        string Ad;
        int i = 0;
        Random rand = new();
        while (i < 3)
        {
            Ad = Ads[rand.Next(Ads.Count)];
            if (AdsContainer.ContainsAd(Ad))
                continue;
            AdsContainer.AddMessage(Ad);
            i++;
        }
        _picked = true;
    }

    public static void DisplaySnacks()
    {
        PickSnacks();
        AdsContainer.DisplayMessages();
    }
}