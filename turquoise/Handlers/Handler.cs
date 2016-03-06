using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Turquoise;
using Turquoise.ParameterBinding;

namespace Turquoise.Handlers
{
    public class Handler : IHandler
    {
        private readonly Delegate _handler;
        private readonly ReadOnlyCollection<ParameterExpression> _parameters;
        
        public Handler(LambdaExpression handler)
        {
            _handler = handler.Compile();
            _parameters = handler.Parameters;
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
            
            
            return _handler.DynamicInvoke(callValues);
        }
        
        private object BindParameter(ParameterExpression parameter, Request request, List<ParameterBinder> parameterBinders)
        {
            var binder = parameterBinders.FirstOrDefault(b => b.SupportsType(parameter.Type));
            
            if (null == binder)
            {
                //TODO: figure out how to get this error to the developer
                return null;
            }
            
            return binder.Bind(request, parameter.Name, parameter.Type);
            
            
        }
    }
}