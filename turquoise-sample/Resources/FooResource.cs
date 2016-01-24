using Turquoise;

public class FooResource : Resource
{
    public FooResource()
        : base("/Foo")
    {
        Get(() => "Hello Foo!");
        Get2<int>("bar", HelloBar);
    }
    
    private object HelloBar(int a)
    {
        return "Hello Bar! a is " + a;
    }
}