using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;
using System.Net.Http.Formatting;

namespace TLWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Configure();
            var rhm = new RequestHeaderMapping("Accept", "text/html",
            StringComparison.InvariantCultureIgnoreCase, true, "application/json");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(rhm);
        }
    }
}
