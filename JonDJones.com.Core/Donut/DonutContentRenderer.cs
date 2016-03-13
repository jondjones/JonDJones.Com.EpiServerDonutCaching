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

namespace JonDJones.com.Core.Donut
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

            var serialisedContent = SerializeBlockContentReference(content);

            var epiServerDonutHoleFiller = new EpiServerDonutHoleFiller(new EncryptingActionSettingsSerialiser(new ActionSettingsSerialiser(), new Encryptor()));

            using (var textWriter = new StringWriter())
            {
                var cutomHtmlHelper = epiServerDonutHoleFiller.CreateHtmlHelper(htmlHelper.ViewContext.Controller, textWriter);
                epiServerDonutHoleFiller.RenderContentData(cutomHtmlHelper, content, string.Empty);

                var outputString = string.Format("<!--Donut#{0}#-->{1}<!--EndDonut-->", serialisedContent, textWriter);

                htmlHelper.RenderXhtmlString(new XhtmlString(outputString));
            }
        }

        private string SerializeBlockContentReference(IContent content)
        {
            return JsonConvert.SerializeObject(content.ContentLink);
        }
    }
}