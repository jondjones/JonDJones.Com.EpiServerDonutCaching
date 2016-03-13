using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Entities
{
    public class PreviewArea
    {
        public bool Supported { get; set; }
        public string AreaName { get; set; }
        public string AreaTag { get; set; }
        public ContentArea ContentArea { get; set; }
    }
}
