using System.Collections.Generic;
using System.IO;

namespace Turquoise
{
    public class Request
    {
        public IDictionary<string, string[]> QueryString { get; set; }
        public IDictionary<string, string[]> RequestHeaders { get; set; }
        public Stream RequestBody { get; set; }
        
        //TODO: I'm not sure if I want this to part of the request or a seperate contextual object
        public IDictionary<string, string> RouteTokens {get; set;}
    }
}