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
using System.Text;

namespace JonDJones.com.Core.Donut
{
    public class EpiServerDonutHoleFiller : DonutHoleFiller
    {
        private static readonly Regex DonutHoleRegex = new Regex(EpiServerDonutHelper.DonutTag, RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex OpenTagRegex = new Regex(EpiServerDonutHelper.OpenDisplayTag,
                                                             RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex CloseTagRegex = new Regex(EpiServerDonutHelper.CloseDisplayTag,
                                                             RegexOptions.Compiled | RegexOptions.Singleline);

        public EpiServerDonutHoleFiller(IActionSettingsSerialiser actionSettingsSerialiser)
            : base(actionSettingsSerialiser)
        {
        }

        public string ReplaceDonutHoleContent(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            if (filterContext.IsChildAction &&
                (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
                return content;

            return DonutHoleRegex.Replace(content, match =>
            {
                var contentReference = JsonConvert.DeserializeObject<ContentReference>(match.Groups[1].Value);

                if (contentReference == null)
                    return null;

                var htmlToRenderWithoutDonutComment = new StringBuilder();

                using (var stringWriter = new StringWriter())
                {
                    var htmlHelper = EpiServerDonutHelper.CreateHtmlHelper(filterContext.Controller, stringWriter);

                    var repo = ServiceLocator.Current.GetInstance<IContentRepository>();
                    var epiContentToRender = repo.Get<IContent>(contentReference);

                    var openTag = OpenTagRegex.Match(match.Groups[1].ToString()).Groups[1].ToString();

                    if (!string.IsNullOrEmpty(openTag))
                        htmlToRenderWithoutDonutComment.Append(openTag);

                    EpiServerDonutHelper.RenderContentData(htmlHelper, epiContentToRender, string.Empty);
                    htmlToRenderWithoutDonutComment.Append(stringWriter.ToString());

                    var closeTag = CloseTagRegex.Match(match.Groups[1].ToString()).Groups[1].ToString();

                    if (!string.IsNullOrEmpty(closeTag))
                        htmlToRenderWithoutDonutComment.Append(closeTag);
                }

                return htmlToRenderWithoutDonutComment.ToString();
            });
        }
    }
}