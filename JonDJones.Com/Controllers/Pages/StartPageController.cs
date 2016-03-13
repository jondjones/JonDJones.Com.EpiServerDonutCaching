using System.Web.Mvc;
using JonDJones.Com.Core.Pages;

using EPiServer.Core;
using JonDJones.Com.Core.ViewModel;
using JonDJones.Com.Controllers.Base;
using JonDJones.Com.Core.ViewModel.Pages;
using DevTrends.MvcDonutCaching;
using DevTrends.MvcDonutCaching.Annotations;
using System;
using System.Web.Providers.Entities;
using System.Web;

using EPiServer.Web.Routing;
using EPiServer;
using EPiServer.ServiceLocation;
using JonDJones.Com.DonutHoleCaching;

namespace JonDJones.Com.Controllers.Pages
{
    public class StartPageController : BasePageController<StartPage>
    {
        [EpiServerDonutCache(Duration = 24 * 3600)]
        public ActionResult Index(StartPage currentPage)
        {
            return View("Index", new StartPageViewModel(currentPage, EpiServerDependencies));
        }

        public ActionResult ExpirePage()
        {
            Expire("32:0::HomePage");
            Expire("110:0::Donut One");
            Expire("113:0::Donut Two");
            Expire("112:0::Nested Donut");

            PageData startPage =
                   ServiceLocator.Current.GetInstance<IContentRepository>().Get<PageData>(ContentReference.StartPage);

            // get URL of the start page
            var startPageUrl = ServiceLocator.Current.GetInstance<UrlResolver>()
                        .GetVirtualPath(startPage.ContentLink, startPage.LanguageBranch);

            return Redirect("/");
        }

        public ActionResult ExpireDonutOne()
        {
            Expire("110:0::Donut One");
            return View("Index", CreateViewExpireModel());
        }

        public ActionResult ExpireDonutTwo()
        {
            Expire("113:0::Donut Two");
            return View("Index", CreateViewExpireModel());
        }

        private void Expire(string key)
        {
            OutputCache.Instance.Remove(key);
        }


        private StartPageViewModel CreateViewExpireModel()
        {
            var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.PageRouteHelper>();
            var currentPage = pageRouteHelper.Page as StartPage;

            var startPage = new StartPageViewModel(currentPage, EpiServerDependencies);
            startPage.Refreshed = true;

            return startPage;
        }

        public OutputCacheManager OutputCacheManager
        {
            get;
            [UsedImplicitly]
            set;
        }
    }
}