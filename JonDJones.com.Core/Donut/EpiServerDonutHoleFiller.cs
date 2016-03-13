using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Newtonsoft.Json;
using System;
using EPiServer.DataAbstraction;

namespace JonDJones.com.Core.Donut
{
    public class EpiServerDonutHoleFiller : DonutHoleFiller
    {
        private static readonly Regex DonutHoles = new Regex("<!--Donut#(.*?)#-->(.*?)<!--EndDonut-->", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly string TempCacheKey = "temp";

        public EpiServerDonutHoleFiller(IActionSettingsSerialiser actionSettingsSerialiser)
            : base(actionSettingsSerialiser)
        {
        }

        public string ReplaceDonutHoleContent(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            if (filterContext.IsChildAction &&
                (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
                return content;

            return DonutHoles.Replace(content, match =>
            {
                var contentReference = JsonConvert.DeserializeObject<ContentReference>(match.Groups[1].Value);

                if (contentReference == null)
                    return null;

                var nonCachedHtml = string.Empty;

                using (var stringWriter = new StringWriter())
                {
                    var htmlHelper = CreateHtmlHelper(filterContext.Controller, stringWriter);

                    var repo = ServiceLocator.Current.GetInstance<IContentRepository>();
                    var contentRef = repo.Get<IContent>(contentReference);

                    RenderContentData(htmlHelper, contentRef, string.Empty);
                    nonCachedHtml = stringWriter.ToString();
                }

                return nonCachedHtml;
            });
        }

        public HtmlHelper CreateHtmlHelper(ControllerBase controller, TextWriter textWriter)
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

        public void RenderContentData(HtmlHelper html,
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

        public string GenerateUniqueCacheKey(ControllerContext filterContext)
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

        private string GenerateUniqueKey(IContent content)
        {
            return string.Format("{0}:{1}:{2}:{3}", content.ContentLink.ID,
                                                    content.ContentLink.WorkID,
                                                    content.ContentLink.ProviderName,
                                                    content.Name);
        }
    }
}