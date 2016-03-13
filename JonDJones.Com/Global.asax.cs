using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using System.Web.Http;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Web.Routing.Segments;
using EPiServer;
using EPiServer.ServiceLocation;
using System.Web;

namespace JonDJones.Com
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Insert(0, new CustomViewEngine());

            AreaRegistration.RegisterAllAreas();
        }


        protected override void RegisterRoutes(RouteCollection routes)
        {
            base.RegisterRoutes(routes);
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg == "MyKey")
            {
                object o = context.Application["MyGuid"];
                if (o == null)
                {
                    o = Guid.NewGuid();
                    context.Application["MyGuid"] = o;
                }
                return o.ToString();
            }
            return base.GetVaryByCustomString(context, arg);
        }
    }
}