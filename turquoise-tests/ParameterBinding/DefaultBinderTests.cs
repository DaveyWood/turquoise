using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Turquoise;
using Turquoise.ParameterBinding;
using System.Threading.Tasks;
using System.IO;

namespace Turquoise.Tests.ParameterBinding
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class DefaultBinderTests
    {
        [Fact]
        public void QueryStringBinds()
        {
            var binder = new DefaultBinder();
            
            Assert.True(binder.SupportsType(typeof(int)));
            Assert.True(binder.SupportsType(typeof(string)));
            
            var queryString = new Dictionary<string, string[]>();
            queryString["myInt"] = new []{"2"};
            queryString["myString"] = new []{"turquoise"};
            
            var request = new Request{ QueryString = queryString, RouteTokens = new Dictionary<string, string>() };
            
            Assert.Equal(2, binder.Bind(request, "myInt", typeof(int)));
            Assert.Equal("turquoise", binder.Bind(request, "myString", typeof(string)));
            
        }
        
        [Fact]
        public void RouteTokensBind()
        {
            var binder = new DefaultBinder();
            
            Assert.True(binder.SupportsType(typeof(int)));
            Assert.True(binder.SupportsType(typeof(string)));
            
            var routeTokens = new Dictionary<string, string>();
            routeTokens["myInt"] = "2";
            routeTokens["myString"] = "turquoise";
            
            var request = new Request{ RouteTokens = routeTokens, QueryString = new Dictionary<string, string[]>() };
            
            Assert.Equal(2, binder.Bind(request, "myInt", typeof(int)));
            Assert.Equal("turquoise", binder.Bind(request, "myString", typeof(string)));
            
        }
        
        [Fact]
        public void RouteTokensHideQueryString()
        {
            //I don't know if this is the behavior I want, but since it's what happens now changing it should fail a test
            var binder = new DefaultBinder();
            
            Assert.True(binder.SupportsType(typeof(int)));
            Assert.True(binder.SupportsType(typeof(string)));
            
            var queryString = new Dictionary<string, string[]>();
            queryString["myInt"] = new []{"not 2"};
            queryString["myString"] = new []{"not turquoise"};
            
            var routeTokens = new Dictionary<string, string>();
            routeTokens["myInt"] = "2";
            routeTokens["myString"] = "turquoise";
            
            var request = new Request{ RouteTokens = routeTokens, QueryString = queryString };
            
            Assert.Equal(2, binder.Bind(request, "myInt", typeof(int)));
            Assert.Equal("turquoise", binder.Bind(request, "myString", typeof(string)));
            
        }
        
        [Fact]
        public void BodyGetsDeserializedAsJson()
        {
            var binder = new DefaultBinder();
            
            var request = new Request { RequestBody = GenerateStreamFromString("{ \"name\": \"Fred\", \"isBig\": true }") };
            
            var boundBird = binder.Bind(request, "bird", typeof(Bird)) as Bird;
            
            Assert.NotNull(boundBird);
            Assert.True(boundBird.IsBig);
            Assert.Equal("Fred", boundBird.Name);
        }
        
        // copied from http://stackoverflow.com/questions/1879395/how-to-generate-a-stream-from-a-string
        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        
    }
    
    public class Bird
    {
        public string Name {get; set;}
        
        public bool IsBig {get; set;}
    }
   
}
