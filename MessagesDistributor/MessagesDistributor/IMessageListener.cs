

namespace MessagesDistributor
{
    public interface IMessageListener
    {
    }

    public interface IMessageListener<in T> : IMessageListener
    {
        
    }

    public interface ISyncMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IStaAsyncMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IAsyncMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IAsyncUiMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface ISyncUiMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IQueueMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IQueueLastMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IQueueUiMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }

    public interface IQueueUiLastMessageListener<in T> : IMessageListener<T>
    {
        void Listen(T message, IMessageObserver observer);
    }
}
