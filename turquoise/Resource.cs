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
        
        private void MapHandler(string method, string path, Delegate handler)
        {
            var parameters = handler.Method.GetParameters();
            if (0 == parameters.Length)
            {
                Handlers.Add(Tuple.Create(method, AppendBasePath(path), new NoArgumentHandler((Func<object>)handler) as IHandler));         
            }
            else if (1 == parameters.Length)
            {
                Handlers.Add(Tuple.Create(method, AppendBasePath(path), new Handler(handler) as IHandler)); 
            }
        }
        
        public void Get(string path, Delegate handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get(Delegate handler)
        {
            Get("", handler);
        }
        
    }
}