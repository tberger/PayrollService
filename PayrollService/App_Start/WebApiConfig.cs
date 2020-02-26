using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PayrollService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste

            // Web-API-Routen
            config.MapHttpAttributeRoutes();
        }
    }
}
