using System;
using System.Collections.Generic;
using Turquoise;
using Turquoise.ParameterBinding;

namespace Turquoise.Handlers
{
    public interface IHandler
    {
        //TODO: everything else about a request
        object HandleRequest(Request request, List<ParameterBinder> parameterBinders);
    }
}
