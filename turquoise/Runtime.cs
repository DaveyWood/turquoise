using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Globalization;
using Turquoise.Routing;
using Turquoise.Owin;
using Turquoise.ParameterBinding;

namespace Turquoise
{
    public class Runtime
    {
        //TODO: separate the Owin integration from the request handling
        private readonly Router _router = new Router();
        
        //TODO: this will probably be refactored out into a class once binding is based on more than type
        private readonly List<ParameterBinder> _binders = new List<ParameterBinder>{ new RequestBinder(), new DefaultBinder() };
           
        //TODO: consider access level - this is primarily public for testing     
        public Task HandleRequest(string method, string path, string queryString,
            IDictionary<string, string[]> requestHeaders,
            Stream requestBody,
            IDictionary<string, string[]> responseHeaders,
            Stream responseStream, Action<int> setStatusCode)
        {
            
            var handler = _router.ResolveRoute(method, path);
            
            if (null != handler)
            {
                var parsedQueryString = ParseQueryString(queryString);
                var request = new Request { QueryString = parsedQueryString, 
                    RequestHeaders = requestHeaders, RequestBody = requestBody};
                //TODO: don't assume the return is a string
                var result = (handler.HandleRequest(request, _binders) ?? "").ToString();
                
                byte[] responseBytes = Encoding.UTF8.GetBytes(result);
                responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
                responseHeaders["Content-Type"] = new string[] { "text/plain; charset=utf-8" };

                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            else
            {
                setStatusCode(404);
                var text = path + " not found";
                byte[] responseBytes = Encoding.UTF8.GetBytes(text);
                responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
                responseHeaders["Content-Type"] = new string[] { "text/plain; charset=utf-8" };
                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            
       
            
        }
        
        private IDictionary<string, string[]> ParseQueryString(string queryString)
        {
            if(null == queryString)
            {
                return new Dictionary<string, string[]>();
            }
            
            var parts = queryString.Split('&').Where(s => !string.IsNullOrEmpty(s));
            
            var returnIt = new Dictionary<string, string[]>();
            
            foreach(var part in parts)
            {
                var split = part.Split('=');
                if (split.Length == 1)
                {
                    returnIt[split[0]] = new String[0];
                }
                else
                {
                    returnIt[split[0]] = new String[]{split[1]};
                }
                //TODO: the same key twice should append instead of overwriting
                //TODO: something if there are multiple equal signs, log an error maybe? Only split on the first one?
                
            }
            
            return returnIt;
        }
    
        public void RegisterParameterBinder(ParameterBinder binder)
        {
            _binders.Insert(0, binder);
        }
        
        public void RegisterResource(Resource resource)
        {
            foreach (var handler in resource.Handlers)
            {
                _router.AddRoute(handler.Item1, handler.Item2, handler.Item3);
            }
        }
    }
}