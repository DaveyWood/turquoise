using Turquoise;
using System;
using System.Linq.Expressions;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        //no argument endpoint
        Get(() => "Hello Foo!");
        
        //single argument, declared as Func
        Expression<Func<string, object>> meh = s => s.Length.ToString();
        Get("bar/zip", meh);
        
        //single argument, inline - generics can't be inferred
        Get<int>("bar", a => "" + a + 1);
        
        //single argument. generics inferred.
        Get("zip", (int x) => x.ToString());
        
        //two parameters
        Get<string, int>("zap", (x, y) => x + y);
        
        //two parameters, generics inferred
        Get("yippy", (int x, string y) => x + y);
    }
    
    //TODO: allow annotations for resources to be written as methods or funcs
    public object HelloBar(int a)
    {
        return "Hello Bar! a is " + a;
    }
}