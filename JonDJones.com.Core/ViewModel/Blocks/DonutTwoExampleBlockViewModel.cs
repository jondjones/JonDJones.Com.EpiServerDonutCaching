using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using JonDJones.com.Core.Blocks;
using JonDJones.com.Core.Enums;
using JonDJones.Com.Core;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Blocks
{
    public class DonutTwoExampleBlockViewModel : BlockViewModel<DonutTwoExampleBlock>
    {
        public DonutTwoExampleBlockViewModel(DonutTwoExampleBlock currentBlock, IEpiServerDependencies epiServerDependencies, DisplayOptionEnum displayOptionTag)
            : base(currentBlock, epiServerDependencies, displayOptionTag)
        {
        }

        public string TheTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }
    }
}
