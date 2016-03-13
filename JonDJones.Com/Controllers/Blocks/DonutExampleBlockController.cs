
using DevTrends.MvcDonutCaching;
using EPiServer.Framework.DataAnnotations;
using JonDJones.com.Core.Blocks;
using JonDJones.com.Core.Resources;
using JonDJones.com.Core.ViewModel.Blocks;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.DonutHoleCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JonDJones.Com.Controllers.Blocks
{
    public class DonutExampleBlockController : BaseBlockController<DonutExampleBlock>
    {
        [ChildActionOnly, EpiServerDonutCache(Duration = 60, Options = OutputCacheOptions.ReplaceDonutsInChildActions)]
        public override ActionResult Index(DonutExampleBlock currentBlock)
        {
            var displayTag = GetDisplayOptionTag();
            return PartialView("Index",
                               new DonutExampleBlockViewModel(currentBlock,
                               EpiServerDependencies,
                               displayTag));
        }
    }
}