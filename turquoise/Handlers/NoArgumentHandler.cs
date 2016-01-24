using System;

namespace Turquoise.Handlers
{
    public class NoArgumentHandler : IHandler
    {
        private readonly Func<object> _handler;
        
        public NoArgumentHandler(Func<object> handler)
        {
            _handler = handler;
        }
        
        public object HandleRequest(string queryString)
        {
            return _handler();
        }
    }
}