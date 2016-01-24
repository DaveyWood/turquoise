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
            else
            {
                Handlers.Add(Tuple.Create(method, AppendBasePath(path), new Handler(handler) as IHandler)); 
            }
        }
        
        public void Get(string path, Func<object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get(Func<object> handler)
        {
            Get("", handler);
        }
        public void Get<T>(string path, Func<T, object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T>(Func<T, object> handler)
        {
            Get<T>("", handler);
        }
        public void Get<T, T2>(string path, Func<T, T2, object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2>(Func<T, T2, object> handler)
        {
            Get<T, T2>("", handler);
        }
        public void Get<T, T2, T3>(string path, Func<T, T2, T3, object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2, T3>(Func<T, T2, T3, object> handler)
        {
            Get<T, T2, T3>("", handler);
        }
        public void Get<T, T2, T3, T4>(string path, Func<T, T2, T3, T4, object> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2, T3, T4>(Func<T, T2, T3, T4, object> handler)
        {
            Get<T, T2, T3, T4>("", handler);
        }
        
    }
}