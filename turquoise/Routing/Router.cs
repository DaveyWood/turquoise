using System.Collections.Generic;
using System.Linq;
using System;

namespace Turquoise.Routing
{
    public class Router
    {
        private readonly IDictionary<string, RoutingNode> _routeTrees = new Dictionary<string, RoutingNode>();
        
        //TODO: handlers shouldn't really be objects
        //TODO: tons of shared code between these methods
        public void AddRoute(string method, string path, object handler)
        {
            var pathList = path.Split('/').Where(s => !String.IsNullOrEmpty(s)).Select(s => s.ToLowerInvariant());
            method = method.ToUpperInvariant();
            
            if (!_routeTrees.ContainsKey(method))
            {
                //add root route for method
                _routeTrees[method] = new RoutingNode();
            }
            
            var root = _routeTrees[method];
            root.AddNodeForPath(pathList.ToList(), path, handler);
            
        }
        
        public object ResolveRoute(string method, string path)
        {
            var pathList = path.Split('/').Where(s => !String.IsNullOrEmpty(s)).Select(s => s.ToLowerInvariant());
            method = method.ToUpperInvariant();
            
            if (!_routeTrees.ContainsKey(method))
            {
                return null;
            }
            
            var root = _routeTrees[method];
            return root.GetNodeForPath(pathList.ToList());
        }
    }
}