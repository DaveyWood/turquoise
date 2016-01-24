using Turquoise;
using System.Linq;
using System;

namespace Turquoise.ParameterBinding
{
    public class DefaultBinder : ParameterBinder
    {
        //TODO: other primitive types
        private static readonly Type[] _supportedTypes = new Type[]{ typeof(string), typeof(int) };
        
        public override bool SupportsType(Type parameterType)
        {
            return _supportedTypes.Contains(parameterType);
        }
        
        public override object Bind(Request request, string parameterName, Type parameterType)
        {
            var stringValue = request.QueryString.ContainsKey(parameterName) ? parameterName : null;
            
            
            if (parameterType == typeof(int))
            {
                int x = 0;
                var parsed = null != stringValue && int.TryParse(stringValue, out x);
                if(parsed)
                {
                    return x;
                }
                
                //TODO: indicate this to the developer
                //I don't like returning 4xx here without calling the action, because it prevents further validations
                return 0;
            }
            
            return stringValue;
        }
    }
    
}