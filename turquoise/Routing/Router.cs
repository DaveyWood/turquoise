using System.Collections.Generic;
using System.Linq;
using System;
using Turquoise;
using Turquoise.Handlers;

namespace Turquoise.Routing
{
    public class Router
    {
        private readonly IDictionary<string, RoutingNode> _routeTrees = new Dictionary<string, RoutingNode>();
        
        //TODO: handlers shouldn't really be objects
        //TODO: tons of shared code between these methods
        public void AddRoute(string method, string path, IHandler handler)
        {
            var pathList = path.Split('/').Where(s => !String.IsNullOrEmpty(s)).Select(s => s.ToLowerInvariant());
            method = method.ToUpperInvariant();
            
            if (!_routeTrees.ContainsKey(method))
            {
                //add root route for method
                _routeTrees[method] = new RoutingNode(method);
            }
            
            var root = _routeTrees[method];
            root.AddNodeForPath(pathList.ToArray(), 0, path, handler);
            
        }
        
        public IHandler ResolveRoute(string method, string path, IDictionary<string, string> routeTokens)
        {
            var pathList = path.Split('/').Where(s => !String.IsNullOrEmpty(s)).Select(s => s.ToLowerInvariant());
            method = method.ToUpperInvariant();
            
            if (!_routeTrees.ContainsKey(method))
            {
                return null;
            }
            
            var root = _routeTrees[method];
            return root.GetNodeForPath(pathList.ToArray(), 0, routeTokens);
        }
    }
}