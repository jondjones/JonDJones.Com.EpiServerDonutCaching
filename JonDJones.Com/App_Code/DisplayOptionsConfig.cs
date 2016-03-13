using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using JonDJones.com.Core.Enums;
using JonDJones.com.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace JonDJones.Com.App_Code
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
     public class DisplayOptionsConfig : IInitializableModule
     {

        public void Initialize(InitializationEngine context)
        {
            var options = ServiceLocator.Current.GetInstance<DisplayOptions>();
            foreach (var optionId in options.Select(x => x.Id).ToArray())
            {
                options.Remove(optionId);
            }

            options
                .Add("full", DisplayOptionTags.FullWidth, DisplayOptionEnum.Full.ToString(), string.Empty, "epi-icon__layout--full")
                .Add("wide", DisplayOptionTags.TwoThirdsWidth, DisplayOptionEnum.TwoThirds.ToString(), string.Empty, "epi-icon__layout--two-thirds")
                .Add("half", DisplayOptionTags.HalfWidth, DisplayOptionEnum.Half.ToString(), string.Empty, "epi-icon__layout--half")
                .Add("narrow", DisplayOptionTags.OneThirdWidth, DisplayOptionEnum.OneThird.ToString(), string.Empty, "epi-icon__layout--one-third")
                .Add("quarter", DisplayOptionTags.OneFourthWidth, DisplayOptionEnum.OneFourth.ToString(), string.Empty, "epi-icon__layout--one-fourth");
         }
 
         public static void RegisterRoutes(RouteCollection routes)
         {
         }
 
         public void Uninitialize(InitializationEngine context)
         {
         }
 
         public void Preload(string[] parameters)
         {
         }
     }

}