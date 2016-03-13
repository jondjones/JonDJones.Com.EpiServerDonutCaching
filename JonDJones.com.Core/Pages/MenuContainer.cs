using EPiServer.Core;
using EPiServer.DataAnnotations;
using JonDJones.com.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Pages
{

    [ContentType(
        DisplayName = "Container Page",
        Description = "A placeholder to help organise the EPiServer page tree",
        GUID = "9997138c-4469-4dbe-8adc-87b615f30f56",
        GroupName = "Standard")]
    public class MenuContainer : PageData, IContainerPage
    {
    }
}
