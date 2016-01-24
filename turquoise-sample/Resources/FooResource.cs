using Turquoise;
using System;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        Get((Func<object>)(() => "Hello Foo!"));
        Get("bar", (Func<int, object>)HelloBar);
    }
    
    private object HelloBar(int a)
    {
        return "Hello Bar! a is " + a;
    }
}