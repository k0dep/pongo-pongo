using System;

namespace Poster
{
    public interface IMessageBinder
    {
        void Bind<TMessage>(Action<TMessage> Handler);
        void UnBind<TMessage>(Action<TMessage> Handler);
    }
}