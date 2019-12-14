using System;
using System.Collections.Generic;

namespace Poster
{
    public class MessageBus : IMessageBinder, IMessageSender
    {
        private readonly Dictionary<Type, List<(object handler, Action<object> list)>> _handlers;

        public event Action<Exception> OnHandleMessageException;

        public MessageBus(Dictionary<Type, List<(object handler, Action<object> list)>> handlers)
        {
            _handlers = handlers;
        }

        public MessageBus() : this(new Dictionary<Type, List<(object handler, Action<object> list)>>())
        {
        }

        public void Bind<TMessage>(Action<TMessage> Handler)
        {
            var type = typeof(TMessage);

            List<(object, Action<object>)> list = null;
            if(_handlers.TryGetValue(type, out list))
            {
            }
            else
            {
                list = new List<(object, Action<object>)>();
                _handlers.Add(type, list);
            }

            list.Add((Handler, (object parameter) => Handler((TMessage) parameter)));
        }

        public void UnBind<TMessage>(Action<TMessage> Handler)
        {
            var type = typeof(TMessage);

            if(_handlers.TryGetValue(type, out var list))
            {
                list.RemoveAll(t => Delegate.Equals(t.handler, Handler));
            }
        }

        public void Send<TMessage>(TMessage message)
        {
            var type = typeof(TMessage);
            if(!_handlers.TryGetValue(type, out var list))
            {
                return;
            }

            foreach(var handler in list)
            {
                try
                {
                    handler.list(message);
                }
                catch(Exception e)
                {
                    OnHandleMessageException?.Invoke(e);
                }
            }
        }
    }
}