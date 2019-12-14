using System;
using System.Collections.Generic;

namespace Poster
{
    public static class MessageBusStatic
    {
        private static MessageBus _busInstance;
        public static MessageBus Bus 
        {
            get
            {
                if(_busInstance == null)
                {
                    _busInstance = new MessageBus();
                }

                return _busInstance;
            }
        }
    }
}