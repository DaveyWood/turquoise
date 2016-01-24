using System.Collections.Generic;
using System.IO;

namespace Turquoise.Owin
{
    // A thin wrapper around a Dictionary with Owin standard keys
    // Will be extended to wrap Turquoise keys if any ever exist
    public class OwinEnvironment
    {
        private readonly IDictionary<string, object> _environment;
        
        public OwinEnvironment(IDictionary<string, object> environment)
        {
            this._environment = environment;
        }
        
        public Stream RequestBody
        {
            get
            {
                return _environment["owin.RequestBody"] as Stream;
            }
            set
            {
                _environment["owin.RequestBody"] = value;
            }
        }
        
        public IDictionary<string, string[]> RequestHeaders
        {
            get
            {
                return _environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
            }
            set
            {
                _environment["owin.RequestHeaders"] = value;
            }
        }
        
        public string RequestMethod
        {
            get
            {
                return _environment["owin.RequestMethod"] as string;
            }
            set
            {
                _environment["owin.RequestMethod"] = value;
            }
        }
        
        public string RequestPath
        {
            get
            {
                return _environment["owin.RequestPath"] as string;
            }
            set
            {
                _environment["owin.RequestPath"] = value;
            }
        }
        
        public string RequestPathBase
        {
            get
            {
                return _environment["owin.RequestPathBase"] as string;
            }
            set
            {
                _environment["owin.RequestPathBase"] = value;
            }
        }
        
        public string RequestProtocol
        {
            get
            {
                return _environment["owin.RequestProtocol"] as string;
            }
            set
            {
                _environment["owin.RequestProtocol"] = value;
            }
        }
        
        public string RequestQueryString
        {
            get
            {
                return _environment["owin.RequestQueryString"] as string;
            }
            set
            {
                _environment["owin.RequestQueryString"] = value;
            }
        }
        
        public string RequestScheme
        {
            get
            {
                return _environment["owin.RequestScheme"] as string;
            }
            set
            {
                _environment["owin.RequestScheme"] = value;
            }
        }
        
        public Stream ResponseBody
        {
            get
            {
                return _environment["owin.ResponseBody"] as Stream;
            }
            set
            {
                _environment["owin.ResponseBody"] = value;
            }
        }
        
        public IDictionary<string, string[]> ResponseHeaders
        {
            get
            {
                return _environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
            }
            set
            {
                _environment["owin.ResponseHeaders"] = value;
            }
        }
        
        public int? ResponseStatusCode
        {
            get
            {
                return _environment.ContainsKey("owin.ResponseStatusCode") ?
                    (int?)_environment["owin.ResponseStatusCode"] : null;
            }
            set
            {
                if (!value.HasValue && _environment.ContainsKey("owin.ResponseStatusCode"))
                {
                    _environment.Remove("owin.ResponseStatusCode");
                }
                else if (value.HasValue)
                {
                    _environment["owin.ResponseStatusCode"] = value.Value;
                }
            }
        }
        
    }
}