using System.Collections.Generic;
using Turquoise.Handlers;

namespace Turquoise.Routing
{
    

    public class RoutingNode
    {
        // basic approach
        // a route is broken into a list of strings - /foo/bar becomes [foo, bar]
        // for each of these we go one level down the tree
        // if we're at the end of the list, then call Handler (if it's not null) or 404 if it's null
        // else if there is an exact match in named child nodes we go there
        // else we iterate through the token nodes until there is a match
        
        private IHandler Handler {get; set;}
        
        private readonly IDictionary<string, RoutingNode> _namedChildNodes = new Dictionary<string, RoutingNode>();
        
        private RoutingNode _tokenNode = null;
        
        //The name of the node - for a token this will be the token value
        public string Name {get; private set;}
        
        public RoutingNode(string name)
        {
            Name = name;
        }
        
        //TODO: rewrite with an immutable list or a start index instead of making all these lists
        //TODO: add support for token nodes
        public IHandler GetNodeForPath(string[] pathParts, int index, IDictionary<string, string> routeTokens)
        {
            if(pathParts.Length == index)
            {
                return this.Handler;
            }
            else if(_namedChildNodes.ContainsKey(pathParts[index]))
            {
                
                return _namedChildNodes[pathParts[index]].GetNodeForPath(pathParts, index + 1, routeTokens);
            }
            else if (null != _tokenNode) //token node catches anything, since this would be the value
            {
                //TODO: store the token value!
                
                return _tokenNode.GetNodeForPath(pathParts, index + 1, routeTokens);
            }
            
            return null;
        }
        
        internal void AddNodeForPath(string[] pathParts, int index, string path, IHandler handler)
        {
            if (pathParts.Length == index)
            {
                if(Handler == null)
                {
                    Handler = handler;
                }
                else
                {
                    throw new DuplicateRouteRegistrationException(path);
                }
            }
            else
            {
                                
                var nextPart = pathParts[index];
                if (nextPart.StartsWith("{") && nextPart.EndsWith("}")) //next part is token
                {
                    if (null == _tokenNode)
                    {
                        _tokenNode = new RoutingNode(nextPart.Substring(1, nextPart.Length - 2));
                    }
                    
                    _tokenNode.AddNodeForPath(pathParts, index + 1, path, handler);
                }
                else
                {
                    if (!_namedChildNodes.ContainsKey(nextPart))
                    {
                        _namedChildNodes[nextPart] = new RoutingNode(nextPart);
                    }
                    _namedChildNodes[nextPart].AddNodeForPath(pathParts, index + 1, path, handler);
                }
            }
            
        }
    }
}