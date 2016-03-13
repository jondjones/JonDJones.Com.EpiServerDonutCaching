using JonDJones.com.Core.Interfaces;
using JonDJones.Com.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Shared
{
    public class HeaderViewModel
    {
        private readonly IEpiServerDependencies _epiServerDependencies;

        public HeaderViewModel(IHeaderProperties headerProperties, IEpiServerDependencies epiServerDependencies)
        {
            Current = headerProperties;
            _epiServerDependencies = epiServerDependencies;
        }

        public IHeaderProperties Current { get; private set; }
    }
}
