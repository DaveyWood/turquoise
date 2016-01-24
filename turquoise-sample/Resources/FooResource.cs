using Turquoise;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        Get(() => "Hello Foo!");
        Get("bar", () => "Hello Bar!");
    }
    
}