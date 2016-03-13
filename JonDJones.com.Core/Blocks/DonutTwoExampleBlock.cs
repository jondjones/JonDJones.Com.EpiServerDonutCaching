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
    [ContentType(DisplayName = "Donut Two Example Block",
        GUID = "dea37095-d991-4030-b3e0-d4a17475d06a",
        Description = "Donut Two Example")]
    public class DonutTwoExampleBlock : BlockData
    {
    }
}