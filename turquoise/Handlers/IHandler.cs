using System;
using System.Collections.Generic;

namespace Turquoise.Handlers
{
    public interface IHandler
    {
        //TODO: everything else about a request
        object HandleRequest(IDictionary<string, string[]> queryString);
    }
}
