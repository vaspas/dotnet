
namespace MessagesDistributor
{
    public interface IDistributor
    {
        void Connect(IMessageListener listener);

        void Distribute<T>(T command, object sender);

        void DistributeAsync<T>(T command, object sender);

        void DistributeExcludeSender<T>(T command, object sender);

        void DistributeAsyncExcludeSender<T>(T command, object sender);


        void Distribute<T>(T command, object sender, IMessageObserver observer);

        void DistributeAsync<T>(T command, object sender, IMessageObserver observer);

        void DistributeExcludeSender<T>(T command, object sender, IMessageObserver observer);

        void DistributeAsyncExcludeSender<T>(T command, object sender, IMessageObserver observer);
    }
}
