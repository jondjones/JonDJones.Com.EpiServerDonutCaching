using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;
using EPiServer;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Sub Menu Page",
        GUID = "12040C26-3CD5-4D0B-BA0D-3FB7D9FBAA8E",
        Description = "Menu Page",
        GroupName = "Standard")]
    public class SubMenuPage : PageData
    {
        [Display(
            Name = "Menu Uel",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual Url LinkUrl { get; set; }
    }
}