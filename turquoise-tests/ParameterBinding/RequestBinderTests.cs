using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Turquoise;
using Turquoise.ParameterBinding;
using System.Threading.Tasks;
using System.IO;

namespace Turquoise.Tests.ParameterBinding
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dnx.html
    public class RequestBinderTests
    {
        [Fact]
        public void RequestBindsToRequest()
        {
            var binder = new RequestBinder();
            
            Assert.True(binder.SupportsType(typeof(Request)));
            
            var request = new Request();
            
            Assert.Equal(request, binder.Bind(request, "blah", typeof(Request)));
            
        }
        
        

    }
   
}
