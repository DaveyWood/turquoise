using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Turquoise.Routing;

namespace Turquoise.Tests.Routing
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class SampleTest
    {
        [Fact]
        public void RouteGetsAdded()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", handler);
            Assert.Equal(handler, router.ResolveRoute("GET", "foo"));
        }
        
        [Fact]
        public void MissingRouteIsNull()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", handler);
            Assert.Null(router.ResolveRoute("GET", "foo/bar"));
        }
        
        [Fact]
        public void MultiPartRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", handler);
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar"));
        }
        
        [Fact]
        public void MultiPartRouteResolvesWithDifferentSlashes()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo/bar", handler);
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar"));
            Assert.Equal(handler, router.ResolveRoute("GET", "/foo/bar/"));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar"));
            Assert.Equal(handler, router.ResolveRoute("GET", "foo/bar/"));
        }
        
        [Fact]
        public void RootRouteResolves()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "", handler);
            Assert.Equal(handler, router.ResolveRoute("GET", "/"));
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
            router.AddRoute("GET", "", handler1);
            router.AddRoute("GET", "foo", handler2);
            router.AddRoute("GET", "foo/bar", handler3);
            router.AddRoute("GET", "bar", handler4);
            router.AddRoute("GET", "bar/foo", handler5);
            Assert.Equal(handler1, router.ResolveRoute("GET", "/"));
            Assert.Equal(handler2, router.ResolveRoute("GET", "/foo"));
            Assert.Equal(handler3, router.ResolveRoute("GET", "/foo/bar"));
            Assert.Equal(handler4, router.ResolveRoute("GET", "bar/"));
            Assert.Equal(handler5, router.ResolveRoute("GET", "bar/foo/"));
        }
        
        [Fact]
        public void DuplicatePathsShouldThrow()
        {
            var router = new Router();
            var handler = new object();
            router.AddRoute("GET", "foo", handler);
            router.AddRoute("POST", "foo", new object());
            
            //using a new object because double registering the same handler should throw now, but won't hurt anything either way
            Assert.Throws<DuplicateRouteRegistrationException>(() => router.AddRoute("GET", "/foo", new object()));
        }
    }
}
