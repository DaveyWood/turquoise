using Turquoise;
using System;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        Get(() => "Hello Foo!");
        Func<string, object> meh = s => s.Length;
        Get("bar/zip", meh);
        
        Get<int>("bar", a => a++);
    }
    
    //TODO: allow annotations for resources to be written as methods or funcs
    public object HelloBar(int a)
    {
        return "Hello Bar! a is " + a;
    }
}