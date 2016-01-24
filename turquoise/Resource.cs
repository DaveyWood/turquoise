using System;
using System.Collections.Generic;
using Turquoise.Handlers;

namespace Turquoise
{
    public abstract class Resource
    {
        private readonly string _basePath;
        internal readonly List<Tuple<string, string, IHandler>> Handlers = new List<Tuple<string, string, IHandler>>();
        
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
        
        private void MapHandler(string method, string path, Func<object> handler)
        {
            Handlers.Add(Tuple.Create(method, AppendBasePath(path), new NoArgumentHandler(handler) as IHandler));            
        }
        private void MapHandler<T>(string method, string path, Func<T, object> handler)
        {
            Handlers.Add(Tuple.Create(method, AppendBasePath(path), new Handler<T>(handler) as IHandler));            
        }
        
        public void Get(string path, Func<object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get(Func<object> handler)
        {
            Get("", handler);
        }
        
        //TODO: the generics didn't get picked up as cleverly as I had hoped, so I need to rethink this api
        public void Get2<T>(string path, Func<T, object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get2<T>(Func<T, object> handler)
        {
            Get2("", handler);
        }
    }
}