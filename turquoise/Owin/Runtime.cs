using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Globalization;


using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>>;

namespace Turquoise.Owin
{

    public class Runtime
    {
        //TODO: routing
        private readonly Dictionary<string, Func<object>> _handlers = new Dictionary<string, Func<object>>();
        
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
            
            if (_handlers.ContainsKey(path))
            {
                var handler = _handlers[path];
                var result = handler() as string;
                byte[] responseBytes = Encoding.UTF8.GetBytes(result);
                responseHeaders["Content-Length"] = new string[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };
                responseHeaders["Content-Type"] = new string[] { "text/plain" };

                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
            else
            {
                turquoiseEnvironment.ResponseStatusCode = 404;
                var text = path + " not found\nAvailable paths are: " + String.Join(", ", _handlers.Keys);
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
                _handlers[handler.Item1] = handler.Item2;
            }
        }
    }
}

/*

Func<Func<IDictionary<string, object>, Task>, 
Func<IDictionary<string, object>, Task>>

*/