using EPiServer.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core;
using JonDJones.Com.Core.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;
using System;

namespace JonDJones.Com.Core.ViewModel.Pages
{
    public class StartPageViewModel : BaseViewModel<StartPage>
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        private readonly StartPage _currentPage;

        public StartPageViewModel(StartPage currentPage, IEpiServerDependencies epiServerDependencies)
            : base(currentPage)
        {
            _epiServerDependencies = epiServerDependencies;
        }

        public string TheTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        public bool Refreshed { get; set; }
    }
}