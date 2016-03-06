using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        
        private void MapHandler(string method, string path, LambdaExpression handler)
        {
            Handlers.Add(Tuple.Create(method, AppendBasePath(path), new Handler(handler) as IHandler));          
        }
        
        public void Get(string path, Expression<Func<object>> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get(Expression<Func<object>> handler)
        {
            Get("", handler);
        }
        public void Get<T>(string path, Expression<Func<T, object>> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T>(Expression<Func<T, object>> handler)
        {
            Get<T>("", handler);
        }
        public void Get<T, T2>(string path, Expression<Func<T, T2, object>> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2>(Expression<Func<T, T2, object>> handler)
        {
            Get<T, T2>("", handler);
        }
        public void Get<T, T2, T3>(string path, Expression<Func<T, T2, T3, object>> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2, T3>(Expression<Func<T, T2, T3, object>> handler)
        {
            Get<T, T2, T3>("", handler);
        }
        public void Get<T, T2, T3, T4>(string path, Expression<Func<T, T2, T3, T4, object>> handler)
        {
            MapHandler("GET", path, handler);
        }
        
        public void Get<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> handler)
        {
            Get<T, T2, T3, T4>("", handler);
        }
        
    }
}