using System;
using System.Collections.Generic;

namespace Turquoise.Handlers
{
    public class NoArgumentHandler : IHandler
    {
        private readonly Func<object> _handler;
        
        public NoArgumentHandler(Func<object> handler)
        {
            _handler = handler;
        }
        
        public object HandleRequest(IDictionary<string, string[]> queryString)
        {
            return _handler();
        }
    }
}