using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Turquoise.Routing;
using Turquoise.Handlers;

namespace Turquoise.Tests.Routing
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class SampleTest
    {
        private IHandler MakeNoArgumentHandler(object returnValue)
        {
            return new NoArgumentHandler(() => returnValue);
        }
        
        [Fact]
        public void RouteGetsAdded()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo").HandleRequest(""));
        }
        
        [Fact]
        public void MissingRouteIsNull()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", MakeNoArgumentHandler(handler));
            Assert.Null(router.ResolveRoute("GET", "foo/bar"));
        }
        
        [Fact]
        public void MultiPartRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar").HandleRequest(""));
            
            var handler2 = new object();
            router.AddRoute("GET", "foo/bar/buzz", MakeNoArgumentHandler(handler2));
            Assert.Equal(handler2, router.ResolveRoute("GET", "foo/bar/buzz/").HandleRequest(""));
        }
        
        [Fact]
        public void MultiPartRouteResolvesWithDifferentSlashes()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar").HandleRequest(""));
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar/").HandleRequest(""));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar").HandleRequest(""));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar/").HandleRequest(""));
        }
        
        [Fact]
        public void RootRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "/").HandleRequest(""));
        }
        
        [Fact]
        public void MultipleMultiPartRoutes()
        {
            var router = new Router();
            var handler1 = new object();
            var handler2 = new object();
            var handler3 = new object();
            var handler4 = new object();
            var handler5 = new object();
            router.AddRoute("GET", "", MakeNoArgumentHandler(handler1));
            router.AddRoute("GET", "foo", MakeNoArgumentHandler(handler2));
            router.AddRoute("GET", "foo/bar", MakeNoArgumentHandler(handler3));
            router.AddRoute("GET", "bar", MakeNoArgumentHandler(handler4));
            router.AddRoute("GET", "bar/foo", MakeNoArgumentHandler(handler5));
            Assert.Equal(handler1, router.ResolveRoute("GET", "/").HandleRequest(""));
            Assert.Equal(handler2, router.ResolveRoute("GET", "/foo").HandleRequest(""));
            Assert.Equal(handler3, router.ResolveRoute("GET", "/foo/bar").HandleRequest(""));
            Assert.Equal(handler4, router.ResolveRoute("GET", "bar/").HandleRequest(""));
            Assert.Equal(handler5, router.ResolveRoute("GET", "bar/foo/").HandleRequest(""));
        }
        
        [Fact]
        public void DuplicatePathsShouldThrow()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", MakeNoArgumentHandler(handler));
            router.AddRoute("POST", "foo", MakeNoArgumentHandler(new object()));
            
            //using a new object because double registering the same handler should throw now, but I don't know if I want that
            Assert.Throws<DuplicateRouteRegistrationException>(() => router.AddRoute("GET", "/foo", MakeNoArgumentHandler(new object())));
        }
    }
}
