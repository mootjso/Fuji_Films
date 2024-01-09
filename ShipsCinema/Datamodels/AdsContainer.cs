public class AdsContainer : MessageContainer<string>
{
    public int AdsCount { get =>  _messageContainer.Count; }

    public AdsContainer()
    {
        _messageContainer = new();
    }

    public override void DisplayMessages()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("------- TASTY OFFERINGS -------");
        foreach (string ad in _messageContainer)
        {
            Console.WriteLine(ad);
        }
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("---------------------------------------------------------------------");
        Console.ResetColor();
    }

    public bool ContainsAd(string ad) => _messageContainer.Contains(ad);
}