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

    }
   
}
