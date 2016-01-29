using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turquoise;

namespace turquoise_mocks
{
    public class TestResource : Resource
    {
        public TestResource()
        {
            Get("Foo", () => "foo");
            Get("foo/yippy", (int x, string y) => x + y);
            Get("foo/request", (Request req) => req.QueryString.Count);
        }
    }
}
