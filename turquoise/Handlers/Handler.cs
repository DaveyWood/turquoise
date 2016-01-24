using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Turquoise;
using Turquoise.ParameterBinding;

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
        
        public object HandleRequest(Request request, List<ParameterBinder> parameterBinders)
        {
            //TODO: handle parameter not available
            //TODO: should repeating the key be valid for collection types?
            
            //var callValues = _parameters.Select(p => Convert.ChangeType(queryString[p.Name][0], p.ParameterType)).ToArray();
            var callValues = _parameters.Select(p => BindParameter(p, request, parameterBinders)).ToArray();
            // var stringValue = queryString[_parameterName][0];
            
            // //TODO: let users register converts
            // object castValue = Convert.ChangeType(stringValue, _parameterType);
            
            
            return _handler.Method.Invoke(_handler.Target, callValues);
        }
        
        private object BindParameter(ParameterInfo parameter, Request request, List<ParameterBinder> parameterBinders)
        {
            var binder = parameterBinders.FirstOrDefault(b => b.SupportsType(parameter.ParameterType));
            
            if (null == binder)
            {
                //TODO: figure out how to get this error to the developer
                return null;
            }
            
            return binder.Bind(request, parameter.Name, parameter.ParameterType);
            
            
        }
    }
}