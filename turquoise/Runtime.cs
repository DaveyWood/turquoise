using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Globalization;
using Turquoise.Routing;
using Turquoise.Owin;

namespace Turquoise
{
    public class Runtime
    {
        //TODO: separate the Owin integration from the request handling
        private readonly Router _router = new Router();
           
        //TODO: consider access level - this is primarily public for testing     
        public Task HandleRequest(string path, string method, IDictionary<string, string[]> responseHeaders,
            Stream responseStream, Action<int> setStatusCode)
        {
            
            
            var handler = _router.ResolveRoute(method, path) as Func<object>;
            
            if (null != handler)
            {
                var result = handler() as string;
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
    
        
        
        public void RegisterResource(Resource resource)
        {
            foreach (var handler in resource.Handlers)
            {
                _router.AddRoute(handler.Item1, handler.Item2, handler.Item3);
            }
        }
    }
}