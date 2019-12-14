namespace Poster
{
    public interface IMessageSender
    {
        void Send<TMessage>(TMessage message);
    }
}