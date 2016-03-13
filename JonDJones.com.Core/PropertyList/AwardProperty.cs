using EPiServer.Core;
using EPiServer.Framework.Serialization;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using JonDJones.com.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JonDJones.com.Core.PropertyList
{
    [PropertyDefinitionTypePlugIn]
    public class AwardProperty : PropertyList<Award>
    {
        protected override Award ParseItem(string value)    
        {
            return JsonConvert.DeserializeObject<Award>(value); 
        }     
        
        public override PropertyData ParseToObject(string value)    
        {        
            ParseToSelf(value);        
            return this;    
        }  
    }
}
