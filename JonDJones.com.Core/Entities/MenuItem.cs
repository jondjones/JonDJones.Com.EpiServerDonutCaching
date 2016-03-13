using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Entities
{
    public class NavigationItem
    {
        public NavigationItem()
        {
            SubMenuItems = new List<NavigationItem>();
        }

        public string Name { get; set; }

        public string SubMenuTitle { get; set; }

        public string Link { get; set; }

        public string ImageUrl { get; set; }

        public List<NavigationItem> SubMenuItems { get; set; }
    }
}
