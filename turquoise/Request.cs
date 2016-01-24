using System.Collections.Generic;
using System.IO;

namespace Turquoise
{
    public class Request
    {
        public IDictionary<string, string[]> QueryString { get; set; }
        public IDictionary<string, string[]> RequestHeaders { get; set; }
        public Stream RequestBody { get; set; }
    }
}