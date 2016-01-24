using System;

namespace Turquoise.Handlers
{
    public interface IHandler
    {
        //TODO: everything else about a request
        object HandleRequest(string queryString);
    }
}
