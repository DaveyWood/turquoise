using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Xunit;
using Turquoise.Routing;
using Turquoise.Handlers;
using Turquoise.ParameterBinding;

namespace Turquoise.Tests.Routing
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class RouterTests
    {
        // defaults for tests that only really need to satisfy the parameters
        private readonly List<ParameterBinder> _binders = new List<ParameterBinder>{ new DefaultBinder() };
        private readonly Request _request = new Request { QueryString = new Dictionary<string, string[]>(),
            RequestHeaders = new Dictionary<string, string[]>(), RequestBody = new MemoryStream(),
            RouteTokens = new Dictionary<string, string>() };
        
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
            Assert.Equal(handler, router.ResolveRoute("GET", "foo", _request.RouteTokens).HandleRequest(_request, _binders));
        }
        
        [Fact]
        public void MissingRouteIsNull()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", MakeNoArgumentHandler(handler));
            Assert.Null(router.ResolveRoute("GET", "foo/bar", _request.RouteTokens));
        }
        
        [Fact]
        public void MultiPartRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar", _request.RouteTokens).HandleRequest(_request, _binders));
            
            var handler2 = new object();
            router.AddRoute("GET", "foo/bar/buzz", MakeNoArgumentHandler(handler2));
            Assert.Equal(handler2, router.ResolveRoute("GET", "foo/bar/buzz/", _request.RouteTokens).HandleRequest(_request, _binders));
        }
        
        [Fact]
        public void MultiPartRouteResolvesWithDifferentSlashes()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar/", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar/", _request.RouteTokens).HandleRequest(_request, _binders));
        }
        
        [Fact]
        public void RootRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "", MakeNoArgumentHandler(handler));
            Assert.Equal(handler, router.ResolveRoute("GET", "/", _request.RouteTokens).HandleRequest(_request, _binders));
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
            Assert.Equal(handler1, router.ResolveRoute("GET", "/", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler2, router.ResolveRoute("GET", "/foo", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler3, router.ResolveRoute("GET", "/foo/bar", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler4, router.ResolveRoute("GET", "bar/", _request.RouteTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler5, router.ResolveRoute("GET", "bar/foo/", _request.RouteTokens).HandleRequest(_request, _binders));
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
        
        [Fact]
        public void TokenizedRoutesResolve()
        {
            var router = new Router();
            var handler = new object();
            var handler2 = new object();
            
            router.AddRoute("GET", "foo/{nah}", MakeNoArgumentHandler(handler));
            router.AddRoute("GET", "foo/{nah}/bar", MakeNoArgumentHandler(handler2));
            
            //since these tests should write to the dictionary use a local instead of being lazy
            var routeTokens = new Dictionary<string, string>();
            
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar", routeTokens).HandleRequest(_request, _binders));
            Assert.Equal(handler2, router.ResolveRoute("GET", "foo/bar/bar", routeTokens).HandleRequest(_request, _binders));
        }
        
        [Fact]
        public void TokenizedRoutesSetValuesInRouteTokens()
        {
            var router = new Router();
            var handler = new object();
            var handler2 = new object();
            
            router.AddRoute("GET", "foo/{nah}", MakeNoArgumentHandler(handler));
            router.AddRoute("GET", "foo/{yep}/bar", MakeNoArgumentHandler(handler2));
            
            //since these tests should write to the dictionary use a local instead of being lazy
            var routeTokens = new Dictionary<string, string>();
            
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar", routeTokens).HandleRequest(_request, _binders));
            Assert.Equal(routeTokens.Count, 1);
            Assert.Equal(routeTokens["nah"], "bar");
            Assert.Equal(handler2, router.ResolveRoute("GET", "foo/buzz/bar", routeTokens).HandleRequest(_request, _binders));
            
            Assert.Equal(routeTokens["yep"], "buzz");
        }
        
        
    }
}
