using System;
using System.Collections.Generic;

namespace Turquoise.Handlers
{
    public class Handler<T> : IHandler
    {
        private readonly Func<T, object> _handler;
        private readonly string _parameterName;
        
        public Handler(Func<T, object> handler)
        {
            _handler = handler;
            //the compiler should guarantee there is exactly one parameter here
            var parameter = handler.Method.GetParameters()[0];
            _parameterName = parameter.Name;
        }
        
        public object HandleRequest(IDictionary<string, string[]> queryString)
        {
            //TODO: handle parameter not available
            //TODO: should repeating the key be valid for collection types?
            var stringValue = queryString[_parameterName][0];
            
            //TODO: let users register converts
            T castValue = (T)Convert.ChangeType(stringValue, typeof(T));
            
            return _handler(castValue);
        }
    }
}