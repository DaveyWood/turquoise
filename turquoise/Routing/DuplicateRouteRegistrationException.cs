

namespace Turquoise.Routing
{
    public class DuplicateRouteRegistrationException : System.Exception
    {
        public DuplicateRouteRegistrationException(string path)
            : base("You cannot register two handlers for " + path) { }
    }
}