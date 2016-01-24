using Turquoise;
using System;

namespace Turquoise.ParameterBinding
{
    public abstract class ParameterBinder
    {
        public abstract bool SupportsType(Type parameterType);
        
        public abstract object Bind(Request request, string parameterName, Type parameterType);
    }
    
}