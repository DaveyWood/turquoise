using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Turquoise.Handlers
{
    public class Handler : IHandler
    {
        private readonly Delegate _handler;
        private readonly ParameterInfo[] _parameters;
        
        public Handler(Delegate handler)
        {
            _handler = handler;
            //the compiler should guarantee there is exactly one parameter here
            _parameters = handler.Method.GetParameters();
            //_parameterName = parameter.Name;
            //_parameterType = parameter.ParameterType;
        }
        
        public object HandleRequest(IDictionary<string, string[]> queryString)
        {
            //TODO: handle parameter not available
            //TODO: should repeating the key be valid for collection types?
            
            var callValues = _parameters.Select(p => Convert.ChangeType(queryString[p.Name][0], p.ParameterType)).ToArray();
            
            // var stringValue = queryString[_parameterName][0];
            
            // //TODO: let users register converts
            // object castValue = Convert.ChangeType(stringValue, _parameterType);
            
            return _handler.Method.Invoke(_handler.Target, callValues);
        }
    }
}