using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JonDJones.Com.DonutHoleCaching
{
    public static class EpiServerDonutHelper
    {
        public static readonly string OpenDisplayTag = "<!--OpenTagOpen-->(.*?)<!--OpenTagClose-->";

        public static readonly string CloseDisplayTag = "<!--CloseTagOpen-->(.*?)<!--CloseTagClose-->";

        public static readonly string DonutTag = "<!--Donut#(.*?)#-->(.*?)<!--EndDonut-->";

        public static readonly string DisplayTagSwap = ">(.*?)<";

        private static readonly string TempCacheKey = "temp";

        public static System.Web.Mvc.HtmlHelper CreateHtmlHelper(ControllerBase controller, TextWriter textWriter)
        {
            var viewContext = new ViewContext(
                controller.ControllerContext,
                new WebFormView(controller.ControllerContext, TempCacheKey),
                controller.ViewData,
                controller.TempData,
                textWriter
            );

            return new HtmlHelper(viewContext, new ViewPage());
        }

        public static void RenderContentData(HtmlHelper html,
                                             IContentData content,
                                             string tag)
        {
            var templateResolver = ServiceLocator.Current.GetInstance<TemplateResolver>();
            var templateModel = templateResolver.Resolve(
                html.ViewContext.HttpContext,
                content.GetOriginalType(),
                content,
                TemplateTypeCategories.MvcPartial,
                tag);

            var contentRenderer = ServiceLocator.Current.GetInstance<IContentRenderer>();
            html.RenderContentData(
                content,
                true,
                templateModel,
                contentRenderer);
        }

        public static string GenerateUniqueCacheKey(ControllerContext filterContext)
        {
            if (filterContext.Controller.ControllerContext.RouteData.Values["currentContent"] == null)
            {
                var pageRouteHelper = ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.PageRouteHelper>();

                var currentPage = pageRouteHelper.Page;

                var key = GenerateUniqueKey(currentPage);
                return key;
            }

            var blockContentReference = filterContext.Controller
                                             .ControllerContext
                                             .RouteData
                                             .Values["currentContent"] as IContent;

            if (blockContentReference != null)
            {
                var key = GenerateUniqueKey(blockContentReference);
                return key;
            }

            return null;
        }


        public static string CreateContentAreaDonutTag(TagBuilder tagBuilder,
                                                       string donutUniqueId,
                                                       string contentToRender)
        {
            var donutTagHtml = new StringBuilder();

            var startTag = Regex.Replace(OpenDisplayTag,
                                         DisplayTagSwap,
                                         string.Format(">{0}<", tagBuilder.ToString(TagRenderMode.StartTag)));

            var endTag = Regex.Replace(CloseDisplayTag,
                                       DisplayTagSwap,
                                       string.Format(">{0}<", tagBuilder.ToString(TagRenderMode.EndTag)));

            donutTagHtml.Append(startTag);
            donutTagHtml.Append(contentToRender);
            donutTagHtml.Append(endTag);

            var outputString = string.Format("<!--Donut#{0}#-->{1}<!--EndDonut-->", donutUniqueId, donutTagHtml.ToString());

            return outputString;
        }

        public static string SerializeBlockContentReference(IContent content)
        {
            return JsonConvert.SerializeObject(content.ContentLink);
        }

        private static string GenerateUniqueKey(IContent content)
        {
            return string.Format("{0}:{1}:{2}:{3}", content.ContentLink.ID,
                                                    content.ContentLink.WorkID,
                                                    content.ContentLink.ProviderName,
                                                    content.Name);
        }
    }
}
