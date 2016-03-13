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
    [ContentType(DisplayName = "Nested Donut Block",
        GUID = "e9b68cf4-8f65-4082-9163-c037c498b0b8",
        Description = "Nested Donut Block")]
    public class NestedDonutBlock : BlockData
    {
    }
}