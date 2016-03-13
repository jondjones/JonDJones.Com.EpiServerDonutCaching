
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
    public class NestDonutBlockController : BaseBlockController<NestedDonutBlock>
    {
        [ChildActionOnly, EpiServerDonutCache(Duration = 5)]
        public override ActionResult Index(NestedDonutBlock currentBlock)
        {
            var displayTag = GetDisplayOptionTag();
            return PartialView("Index",
                               new NestedDonutBlockViewModel(currentBlock,
                               EpiServerDependencies,
                               displayTag));
        }
    }
}