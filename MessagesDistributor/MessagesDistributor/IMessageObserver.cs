
using System;

namespace MessagesDistributor
{
    public interface IMessageObserver
    {
        /// <summary>
        /// Регистрирует новое действие над сообщением.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Action RegisterAction(object owner, string name);

        /// <summary>
        /// Создает объект наблюдателя для нового сообщения.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        IMessageObserver CreateSubObserver(object message, object sender);

        /// <summary>
        /// Объект создавший сообщение.
        /// </summary>
        object Sender { get; }
    }
}
