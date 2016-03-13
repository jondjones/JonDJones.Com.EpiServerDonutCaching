using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using DevTrends.MvcDonutCaching;
using System.IO;

namespace JonDJones.Com.DonutHoleCaching
{
    public static class DonutHtmlHelper
    {
        public static void DonutHole(this HtmlHelper htmlHelper, ContentArea contentArea)
        {
            ServiceLocator.Current.GetInstance<DonutContentRenderer>().Render(htmlHelper, contentArea);
        }

        public static void DonutForContentArea(this HtmlHelper htmlHelper, ContentArea contentArea)
        {
            ServiceLocator.Current.GetInstance<DonutContentRenderer>().Render(htmlHelper, contentArea);
        }

        public static void DonutForContentArea(this HtmlHelper htmlHelper, ContentArea contentArea, object additionalViewData)
        {
            var additionalValues = new RouteValueDictionary(additionalViewData);

            foreach (var value in additionalValues)
            {
                htmlHelper.ViewContext.ViewData.Add(value.Key, value.Value);
            }

            ServiceLocator.Current.GetInstance<DonutContentRenderer>().Render(htmlHelper, contentArea);
        }

        public static void DonutForContent(this HtmlHelper htmlHelper, IContent content)
        {
            var serialisedContent = EpiServerDonutHelper.SerializeBlockContentReference(content);

            using (var textWriter = new StringWriter())
            {
                var cutomHtmlHelper = EpiServerDonutHelper.CreateHtmlHelper(htmlHelper.ViewContext.Controller, textWriter);
                EpiServerDonutHelper.RenderContentData(cutomHtmlHelper, content, string.Empty);

                var outputString = string.Format("<!--Donut#{0}#-->{1}<!--EndDonut-->", serialisedContent, textWriter);

                var htmlString =  new XhtmlString(outputString);
                htmlHelper.RenderXhtmlString(htmlString);
            }
        }
    }
}
