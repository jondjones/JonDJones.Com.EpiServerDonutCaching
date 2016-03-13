using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using JonDJones.com.Core.Blocks.Base;
using JonDJones.com.Core.Entities;
using JonDJones.com.Core.ViewModel.Base;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JonDJones.Com.Controllers.Base
{
    [TemplateDescriptor(Inherited = true,
       Tags = new[] { RenderingTags.Preview },
       TemplateTypeCategory = TemplateTypeCategories.MvcController)]
    public class PreviewController : Controller, IRenderTemplate<BlockData>
    {
        private readonly DisplayOptions displayOptions;

        private readonly TemplateResolver templateResolver;

        private readonly IEpiServerDependencies epiDependencies;

        private IContent currentContent;

        public PreviewController(DisplayOptions displayOptions, TemplateResolver templateResolver, IEpiServerDependencies epiDependencies)
        {
            this.displayOptions = displayOptions;
            this.templateResolver = templateResolver;
            this.epiDependencies = epiDependencies;
        }

        public IEpiServerDependencies EpiServerDependencies
        {
            get { return epiDependencies; }
        }

        public ActionResult Index(IContent currentContent)
        {
            this.currentContent = currentContent;

            var startPage = epiDependencies.ContentRepository.Get<StartPage>(SiteDefinition.Current.StartPage);

            var supportedAreas = GetSupportedPreviewAreas();

            var viewModel = new PreviewViewModel(startPage, currentContent, supportedAreas);

            return View("~/Views/Blocks/Preview.cshtml", viewModel);
        }

        private IEnumerable<PreviewArea> GetSupportedPreviewAreas()
        {
            return displayOptions.Select(x => new PreviewDisplayOption
                                {
                                    Tag = x.Tag,
                                    Name = x.Name,
                                    IsSupported = IsTagSupported(currentContent, x.Tag)
                                })
                                .Where(x => x.IsSupported)
                                .Select(CreatePreviewArea);
        }

        private PreviewArea CreatePreviewArea(PreviewDisplayOption previewDisplayOption)
        {
            var contentArea = new ContentArea();
            var item = new ContentAreaItem
            {
                ContentLink = currentContent.ContentLink
            };

            contentArea.Items.Add(item);

            var areaModel = new PreviewArea
            {
                ContentArea = contentArea,
                Supported = previewDisplayOption.IsSupported,
                AreaTag = previewDisplayOption.Tag,
                AreaName = previewDisplayOption.Name,
            };

            return areaModel;
        }

        private bool IsTagSupported(IContent content, string tag)
        {
            var templateModel = templateResolver.Resolve(HttpContext,
                                      content.GetOriginalType(),
                                      content,
                                      TemplateTypeCategories.MvcPartial,
                                      tag);

            return templateModel != null;
        }
    }
}