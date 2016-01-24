using System;
using System.Collections.Generic;

namespace Turquoise
{
    public abstract class Resource
    {
        private readonly string _basePath;
        internal readonly List<Tuple<string, Func<object>>> Handlers = new List<Tuple<string, Func<object>>>();
        
        //The base path for the resource
        public string BasePath
        {
            get
            {
                return _basePath;
            }
        }
        
        public Resource(string basePath = "")
        {
            //ensure basePath ends in /
            _basePath = (basePath ?? "").TrimEnd('/') + "/";
        }
        
        public bool CanHandlePath(string requestPath)
        {
            return true;
        }
        
        private string AppendBasePath(string path)
        {
            return _basePath + (path ?? "").TrimStart('/');
        }
        
        public void MapGet(string path, Func<object> handler)
        {
            Handlers.Add(Tuple.Create(AppendBasePath(path), handler));
        }
        
        public void MapGet(Func<object> handler)
        {
            MapGet("", handler);
        }
    }
}