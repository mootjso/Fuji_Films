public abstract class MessageContainer<T>
{
    protected List<T> _messageContainer { get; set; } = new();

    public virtual void AddMessage(T message)
    {
        _messageContainer.Add(message);
    }
    public abstract void DisplayMessages();
}