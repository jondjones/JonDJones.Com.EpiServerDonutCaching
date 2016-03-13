using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using JonDJones.Com.Core;
using JonDJones.com.Core.Enums;
using System;
using JonDJones.com.Core.Attributes;
using JonDJones.com.Core.Extensions;
using System.Linq;

namespace JonDJones.Com.Controllers.Base
{
    public abstract class BaseBlockController<TBlockData> : BlockController<TBlockData>
         where TBlockData : BlockData
    {
        private Injected<EpiServerDependencies> epiServerDependencies;

        private Injected<PageRouteHelper> routeHelper;

        protected PageData CurrentPage
        {
            get
            {
                return routeHelper.Service.Page;
            }
        }

        protected EpiServerDependencies EpiServerDependencies
        {
            get
            {

                var value = epiServerDependencies.Service;
                value.CurrentPage = CurrentPage;

                return value;
            }
        }

        protected DisplayOptionEnum GetDisplayOptionTag()
        {
            var renderSettings = this.ControllerContext.RouteData.Values["renderSettings"] as Dictionary<string, object>;
            if (renderSettings == null)
                return DisplayOptionEnum.Unknown;

            object tag;
            if (!renderSettings.TryGetValue("tag", out tag))
                return DisplayOptionEnum.Unknown;

            if (tag == null)
                return DisplayOptionEnum.Unknown;

            return GetDisplayOptionTag(tag.ToString());
        }

        public static DisplayOptionEnum GetDisplayOptionTag(string tag)
        {
            DisplayOptionEnum displayOptionEnum;
            Enum.TryParse<DisplayOptionEnum>(tag, out displayOptionEnum);

            return displayOptionEnum;
        }

        public static string GetDisplayOptionsTag(DisplayOptionEnum value)
        {
            var displayOptionName 
                = value.GetAttributeOfEnumValue<DisplayOptionNameAttribute>()
                       .IfNotDefault(x => x.Name);

            return displayOptionName;
        }
    }
}