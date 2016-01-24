using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Globalization;

//using MidFunc = Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>;
//this can't take advantage of the previous using statements :(
//I got the MidFunc off a blog post. I think it's a common convention, but I need to find the source.    

using MidFunc = System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Threading.Tasks.Task>>;

namespace Turquoise.Owin
{
    public static class OwinIntegration
    {
        public static Action<MidFunc> UseTurquoise(this Action<MidFunc> builder, Runtime runtime)
        {
            builder(b => environment => HandleRequest(environment, runtime));
            return builder;
        }
        
        private static Task HandleRequest(IDictionary<string, object> environment, Runtime runtime)
        {
            var turquoiseEnvironment = new OwinEnvironment(environment);
            var path = turquoiseEnvironment.RequestPath;
            var responseStream = turquoiseEnvironment.ResponseBody;
            var responseHeaders = turquoiseEnvironment.ResponseHeaders;
            
            return runtime.HandleRequest(turquoiseEnvironment.RequestMethod, path,
                turquoiseEnvironment.RequestQueryString, turquoiseEnvironment.RequestHeaders,
                turquoiseEnvironment.RequestBody, turquoiseEnvironment.ResponseHeaders,
                turquoiseEnvironment.ResponseBody, statusCode => { turquoiseEnvironment.ResponseStatusCode = statusCode; });
        }
    }
}
