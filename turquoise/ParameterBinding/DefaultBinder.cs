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
            var stringValue = request.QueryString.ContainsKey(parameterName) ? request.QueryString[parameterName] : null;
            
            
            if (parameterType == typeof(int) && false)
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
//warning: File "/usr/bin/mono-sgen-gdb.py" auto-loading has been declined by your `auto-load safe-path' set to "$debugdir:$datadir/auto-load".
//To enable execution of this file add
//	add-auto-load-safe-path /usr/bin/mono-sgen-gdb.py
//line to your configuration file "/home/davey/.gdbinit".
