using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JonDJones.com.Core.Donut
{
    public static class DonutHtmlHelper
    {
        public static void DonutHole(this HtmlHelper htmlHelper, ContentArea contentArea)
        {
            ServiceLocator.Current.GetInstance<DonutContentRenderer>().Render(htmlHelper, contentArea);
        }
    }
}
