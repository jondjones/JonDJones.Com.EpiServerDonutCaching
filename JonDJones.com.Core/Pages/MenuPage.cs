using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using JonDJones.Com.Core.Pages.Base;
using EPiServer.DataAbstraction;
using EPiServer;
using EPiServer.Web;

namespace JonDJones.Com.Core.Pages
{

    [ContentType(
        DisplayName = "Menu Page",
        GUID = "A74BDA60-305C-47AA-9795-408BD343DD04",
        Description = "Menu Page",
        GroupName = "Standard")]
    public class MenuPage : PageData
    {
        [Display(
            Name = "Main Menu Title",
            Description = "Main Menu Title",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string MainMenuTitle { get; set; }

        [Display(
            Name = "Sub Menu Title",
            Description = "Sub Menu Title",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual string SubMenuTitle { get; set; }

        [Display(
            Name = "Menu Image",
            Description = "Sub Menu Title",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        [UIHint(UIHint.Image)]
        public virtual Url MenuImageUrl { get; set; }
    }
}