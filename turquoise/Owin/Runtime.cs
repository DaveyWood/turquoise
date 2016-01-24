using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Globalization;
using Turquoise.Routing;



using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>>;

namespace Turquoise.Owin
{

    public class Runtime
    {
        //TODO: separate the Owin integration from the request handling
        private readonly Router _router = new Router();
        
        public Func<IDictionary<string, object>, Task> DoStuff(Func<IDictionary<string, object>, Task> innerHandler)
        {
            return this.Handler;
        }
        
        private Task Handler(IDictionary<string, object> environment)
        {
            var turquoiseEnvironment = new OwinEnvironment(environment);
            var path = turquoiseEnvironment.RequestPath;
            var responseStream = turquoiseEnvironment.ResponseBody;
            var responseHeaders = turquoiseEnvironment.ResponseHeaders;
            
            var handler = _router.ResolveRoute(turquoiseEnvironment.RequestMethod, path) as Func<object>;
            
            if (null != handler)
            {
                var result = handler() as string;
                byte[] responseBytes = Encoding.UTF8.GetBytes(result);
                responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
                responseHeaders["Content-Type"] = new string[] { "text/plain" };

                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            else
            {
                turquoiseEnvironment.ResponseStatusCode = 404;
                var text = path + " not found";
                byte[] responseBytes = Encoding.UTF8.GetBytes(text);
                responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
                responseHeaders["Content-Type"] = new string[] { "text/plain" };
                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            
       
            
        }
    
        public Action<MidFunc> UseTurquoise(Action<MidFunc> builder)
        {
            builder(this.DoStuff);
            return builder;
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

/*

Func<Func<IDictionary<string, object>, Task>, 
Func<IDictionary<string, object>, Task>>

*/