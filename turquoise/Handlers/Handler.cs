using System;
using System.Collections.Generic;

namespace Turquoise.Handlers
{
    public class Handler : IHandler
    {
        private readonly Delegate _handler;
        private readonly string _parameterName;
        private readonly Type _parameterType;
        
        public Handler(Delegate handler)
        {
            _handler = handler;
            //the compiler should guarantee there is exactly one parameter here
            var parameter = handler.Method.GetParameters()[0];
            _parameterName = parameter.Name;
            _parameterType = parameter.ParameterType;
        }
        
        public object HandleRequest(IDictionary<string, string[]> queryString)
        {
            //TODO: handle parameter not available
            //TODO: should repeating the key be valid for collection types?
            var stringValue = queryString[_parameterName][0];
            
            //TODO: let users register converts
            object castValue = Convert.ChangeType(stringValue, _parameterType);
            
            return _handler.DynamicInvoke(castValue);
        }
    }
}