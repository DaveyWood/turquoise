using Turquoise;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        MapGet(() => "Hello Foo!");
        MapGet("bar", () => "Hello Bar!");
    }
    
}