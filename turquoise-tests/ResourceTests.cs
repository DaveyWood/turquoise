using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Turquoise;
using System.Threading.Tasks;
using System.IO;

namespace Turquoise.Tests
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class ResourceTests
    {
        [Fact]
        public void GetReturnsFoo()
        {
            var responseHeaders = new Dictionary<string, string[]>();
            var responseStream = new MemoryStream();
            var requestBody = new MemoryStream();
            
            //set status code defaults to 200, so it's not called
            Action<int> setStatusCode = i => {};
            var runtime = new Runtime();
            var resource = new ResourceTestResource();
            runtime.RegisterResource(resource);
            
            var task = runtime.HandleRequest("GET", "foo/yippy", "x=4&y=t6", responseHeaders, requestBody, responseHeaders,
                responseStream, setStatusCode);
            Task.WaitAll(task);
            
            responseStream.Position = 0;
            
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var response = reader.ReadToEnd();
                Assert.Equal("4t6", response);
            }
            
            //TODO: test content type, etc once they're not hard coded
        }
        
        

    }
    
    public class ResourceTestResource : Resource
    {
        public ResourceTestResource()
            : base("foo")
        {
            Get("Foo", () => "foo");
            Get("yippy", (int x, string y) => x + y);
        }
    }
}