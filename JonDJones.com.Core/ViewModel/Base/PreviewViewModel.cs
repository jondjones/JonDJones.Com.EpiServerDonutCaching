using EPiServer.Core;
using JonDJones.com.Core.Entities;
using JonDJones.Com.Core.Pages;
using JonDJones.Com.Core.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.ViewModel.Base
{
    public class PreviewViewModel : BaseViewModel<StartPage>
    {
        public PreviewViewModel(StartPage currentPage, IContent previewContent, IEnumerable<PreviewArea> areas)
            : base(currentPage)
        {
            this.PreviewContent = previewContent;
            this.Areas = areas.ToList();
        }

        public IContent PreviewContent { get; private set; }

        public IList<PreviewArea> Areas { get; private set; }
    }
}
