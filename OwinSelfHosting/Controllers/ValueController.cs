using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace OwinSelfHost.Controllers
{
    public class ValuesController: ApiController
    {

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
