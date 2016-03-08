using Turquoise;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Turquoise.ParameterBinding
{
    public class DefaultBinder : ParameterBinder
    {
        //TODO: other primitive types
        private static readonly HashSet<Type> _primitiveTypes = new HashSet<Type>{
            typeof(int),
            typeof(double),
            typeof(float),
            typeof(long),
            typeof(short),
            typeof(int?),
            typeof(double?),
            typeof(float?),
            typeof(long?),
            typeof(short?),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(uint?),
            typeof(ulong?),
            typeof(ushort?),
            typeof(string),
            typeof(bool),
            typeof(bool?),
            typeof(Guid),
            typeof(Guid?),
            typeof(decimal),
            typeof(decimal?)
        };
        
        public override bool SupportsType(Type parameterType)
        {
            return true;
        }
        
        // TODO: I'm assuming this is safe to share
        private static readonly JsonSerializer _serializer = new JsonSerializer();
        
        public override object Bind(Request request, string parameterName, Type parameterType)
        {
            if (_primitiveTypes.Contains(parameterType))
            {
                return BindPrimitive(request, parameterName, parameterType);
            }
                        
            // TODO: content negotiation, assume JSON for this pass
            var requestBody = request.RequestBody;
            
            if (null == requestBody)
            {
                return null;
            }
            
            using (var streamReader = new StreamReader(requestBody))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize(jsonReader, parameterType);
                }
            }
            
        }
        public object BindPrimitive(Request request, string parameterName, Type parameterType)
        {
            //TODO: what if there are multiple values here?
            string stringValue = null;
            
            if (request.RouteTokens.ContainsKey(parameterName))
            {
                stringValue = request.RouteTokens[parameterName];
            }
            else if (request.QueryString.ContainsKey(parameterName))
            {
                stringValue = request.QueryString[parameterName][0];
            }
            
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
            
            //TODO: tons of types, I would like to think of a clever way to not have to handcode all of them
            
            return stringValue;
        }
    }
    
}
