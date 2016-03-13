using EPiServer.Core;
using JonDJones.com.Core.Entities;
using JonDJones.com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.com.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer;
using EPiServer.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace JonDJones.com.Core.Repository
{
    public class MenuRepository
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        public MenuRepository(IEpiServerDependencies epiServerDependencies)
        {
            _epiServerDependencies = epiServerDependencies;
        }

        public List<NavigationItem> GetMainMenu()
        {
            var navigationItems = new List<NavigationItem>();
            var urlHelper = ServiceLocator.Current.GetInstance<UrlResolver>();

            var menuContainer = 
                                  _epiServerDependencies.ContentRepository
                                  .GetChildren<MenuContainer>(ContentReference.RootPage)
                                  .FirstOrDefault();

            var menuPages = _epiServerDependencies.ContentRepository
                                                  .GetChildren<MenuPage>(menuContainer.ContentLink);

            foreach(var menuPage in menuPages)
            {
                var navigationItem = new NavigationItem();

                navigationItem.Name = menuPage.MainMenuTitle;
                navigationItem.SubMenuTitle = menuPage.SubMenuTitle;
                navigationItem.ImageUrl = menuPage.MenuImageUrl.ToString();

                var subMenuPages =
                    _epiServerDependencies.ContentRepository
                                          .GetChildren<SubMenuPage>(menuPage.ContentLink);

                foreach(var subMenuPage in subMenuPages)
                {
                    var subNavigationItem =
                        new NavigationItem
                        {
                            Name = subMenuPage.Name,
                            Link = subMenuPage.LinkUrl.ToString()
                        };

                        navigationItem.SubMenuItems.Add(subNavigationItem);
                }

                navigationItems.Add(navigationItem);
            }

            return navigationItems;

        }
    }
}
