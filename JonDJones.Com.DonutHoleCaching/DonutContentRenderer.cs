using DevTrends.MvcDonutCaching;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JonDJones.Com.DonutHoleCaching
{
    public class DonutContentRenderer : ContentAreaRenderer
    {
        public DonutContentRenderer(IContentRenderer contentRenderer,
                                    TemplateResolver templateResolver,
                                    ContentFragmentAttributeAssembler attributeAssembler,
                                    IContentRepository contentRepository,
                                    DisplayOptions displayOptions)
            : base(contentRenderer, templateResolver, attributeAssembler, contentRepository, displayOptions)
        {
        }

        protected override void RenderContentAreaItem(
            HtmlHelper htmlHelper,
            ContentAreaItem contentAreaItem,
            string templateTag,
            string htmlTag,
            string cssClass)
        {
            var content = contentAreaItem.GetContent(ContentRepository);
            if (content == null)
                return;

            var serialisedContent = EpiServerDonutHelper.SerializeBlockContentReference(content);

            using (var textWriter = new StringWriter())
            {
                var cutomHtmlHelper = EpiServerDonutHelper.CreateHtmlHelper(htmlHelper.ViewContext.Controller, textWriter);
                EpiServerDonutHelper.RenderContentData(cutomHtmlHelper, content, string.Empty);

                var tagBuilder = CreateContentAreaSeperatorHtmlTags(contentAreaItem, htmlTag, cssClass);
                var epiServerHtml = EpiServerDonutHelper.CreateContentAreaDonutTag(tagBuilder, serialisedContent, textWriter.ToString());

                htmlHelper.RenderXhtmlString(new XhtmlString(epiServerHtml));
            }
        }

        private TagBuilder CreateContentAreaSeperatorHtmlTags(ContentAreaItem contentAreaItem, string htmlTag, string cssClass)
        {
            var tagBuilder = new TagBuilder(htmlTag);
            AddNonEmptyCssClass(tagBuilder, cssClass);
            BeforeRenderContentAreaItemStartTag(tagBuilder, contentAreaItem);
            return tagBuilder;
        }

    }
}