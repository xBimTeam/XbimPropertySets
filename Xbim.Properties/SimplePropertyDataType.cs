using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of data type.
    /// </summary>
    public class SimplePropertyDataType
    {
        [XmlAttribute("type")]
        public string Type { get; set; }    
    }

}
