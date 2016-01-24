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
    public class RutimeTests
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
            var resource = new TestResource();
            runtime.RegisterResource(resource);
            
            var task = runtime.HandleRequest("GET", "foo", "", responseHeaders, requestBody, responseHeaders,
                responseStream, setStatusCode);
            Task.WaitAll(task);
            
            responseStream.Position = 0;
            
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var response = reader.ReadToEnd();
                Assert.Equal("foo", response);
            }
            
            //TODO: test content type, etc once they're not hard coded
        }
        
        [Fact]
        public void GetReturns404()
        {
            var responseHeaders = new Dictionary<string, string[]>();
            var responseStream = new MemoryStream();
            var requestBody = new MemoryStream();
            
            var statusCode = 0;
            
            Action<int> setStatusCode = i => { statusCode = i; };
            var runtime = new Runtime();
            var resource = new TestResource();
            runtime.RegisterResource(resource);
            
            var task = runtime.HandleRequest("GET", "foo2", "", responseHeaders, requestBody, responseHeaders,
                responseStream, setStatusCode);
            Task.WaitAll(task);
            
            responseStream.Position = 0;
            
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                var response = reader.ReadToEnd();
                Assert.True(response.EndsWith("not found"));
            }
            
            Assert.Equal(404, statusCode);
        }

    }
    
    public class TestResource : Resource
    {
        public TestResource()
        {
            Get("Foo", () => "foo");
            
        }
    }
}
