using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;

namespace JonDJones.Com.Initialization
{
    [InitializableModule]
    public class DataInitialization : IInitializableModule
    {
        Injected<IEpiServerDependencies> epiServerDependencies;

        public void Initialize(InitializationEngine context)
        {
            var dependency = epiServerDependencies.Service;
        }
        public void Uninitialize(InitializationEngine context)
        {

        }
        public void Preload(string[] parameters)
        {
        }
    }
}