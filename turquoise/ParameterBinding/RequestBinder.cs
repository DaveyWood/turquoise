using Turquoise;
using System.Linq;
using System;

namespace Turquoise.ParameterBinding
{
    public class RequestBinder : ParameterBinder
    {
        
        public override bool SupportsType(Type parameterType)
        {
            return typeof(Request) == parameterType;
        }
        
        public override object Bind(Request request, string parameterName, Type parameterType)
        {
            return request;
        }
    }
    
}
