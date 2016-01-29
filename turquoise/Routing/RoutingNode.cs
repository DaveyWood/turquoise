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
        
        //TODO: rewrite with an immutable list or a start index instead of making all these lists
        //TODO: add support for token nodes
        public IHandler GetNodeForPath(List<string> pathParts)
        {
            if(pathParts.Count == 0)
            {
                return this.Handler;
            }
            else if(_namedChildNodes.ContainsKey(pathParts[0]))
            {
                var subsequentTokens = new List<string>(pathParts.Count -1);
                for(var i = 1; i < pathParts.Count; i++)
                {
                    subsequentTokens.Add(pathParts[i]);
                }
                return _namedChildNodes[pathParts[0]].GetNodeForPath(subsequentTokens);
            }
            else if (null != _tokenNode) //token node catches anything, since this would be the value
            {
                //TODO: store the token value!
                //TODO: when the subsequent tokens stuff is refactored to not be a new list don't copy/paste so much
                var subsequentTokens = new List<string>(pathParts.Count -1);
                for(var i = 1; i < pathParts.Count; i++)
                {
                    subsequentTokens.Add(pathParts[i]);
                }
                return _tokenNode.GetNodeForPath(subsequentTokens);
            }
            
            return null;
        }
        
        internal void AddNodeForPath(List<string> pathParts, string path, IHandler handler)
        {
            //TODO support for the next part being a token
            if (pathParts.Count == 0)
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
                //TODO: use a data structure that doesn't require building new lists every time, it's silly
                var subsequentTokens = new List<string>(pathParts.Count -1);
                for(var i = 1; i < pathParts.Count; i++)
                {
                    subsequentTokens.Add(pathParts[i]);
                }
                
                var nextPart = pathParts[0];
                if (nextPart.StartsWith("{") && nextPart.EndsWith("}")) //next part is token
                {
                    if (null == _tokenNode)
                    {
                        _tokenNode = new RoutingNode();
                    }
                    
                    _tokenNode.AddNodeForPath(subsequentTokens, path, handler);
                }
                else
                {
                    if (!_namedChildNodes.ContainsKey(nextPart))
                    {
                        _namedChildNodes[nextPart] = new RoutingNode();
                    }
                    _namedChildNodes[nextPart].AddNodeForPath(subsequentTokens, path, handler);
                }
            }
            
        }
    }
}