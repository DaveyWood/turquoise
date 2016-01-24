using System.Collections.Generic;
using System.Linq;
using System;

namespace Turquoise.Routing
{
    public class Router
    {
        private readonly IDictionary<string, RoutingNode> _routeTrees = new Dictionary<string, RoutingNode>(6);
        
        public void AddRoute(string path, string method, object handler)
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
    }
}