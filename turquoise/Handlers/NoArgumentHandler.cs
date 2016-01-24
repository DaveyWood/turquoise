using System;
using System.Collections.Generic;
using Turquoise;
using Turquoise.ParameterBinding;

namespace Turquoise.Handlers
{
    public class NoArgumentHandler : IHandler
    {
        private readonly Func<object> _handler;
        
        public NoArgumentHandler(Func<object> handler)
        {
            _handler = handler;
        }
        
        public object HandleRequest(Request request, List<ParameterBinder> parameterBinders)
        {
            return _handler();
        }
    }
}