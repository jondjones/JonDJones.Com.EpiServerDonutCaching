using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace JonDJones.com.Core.Blocks
{
    [ContentType(DisplayName = "Donut Example Block",
        GUID = "EB15ECE1-341D-4F3D-887B-315963732790",
        Description = "Donut Example")]
    public class DonutExampleBlock : BlockData
    {
        [Display(
            Name = "Main Content Area",
            Description = "Region where content blocks can be placed",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual ContentArea MainContentArea { get; set; }
    }
}