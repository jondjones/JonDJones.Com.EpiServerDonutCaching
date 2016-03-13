using EPiServer.Core;
using JonDJones.com.Core.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.Interfaces
{
    public interface IHeaderProperties
    {
        ContentArea SearchBar { get; set; }
    }
}
