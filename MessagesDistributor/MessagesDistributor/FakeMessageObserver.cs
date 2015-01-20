using System;

namespace MessagesDistributor
{
    class FakeMessageObserver:IMessageObserver
    {
        private static readonly Action _fakeAction = () => { };

        public Action RegisterAction(object sender, string name)
        {
            return _fakeAction;
        }

        /// <summary>
        /// Создает объект наблюдателя для нового сообщения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IMessageObserver CreateSubObserver(object message, object sender)
        {
            return new FakeMessageObserver
                       {
                           Sender = sender
                       };
        }

        public object Sender { get; private set; }
    }
}
