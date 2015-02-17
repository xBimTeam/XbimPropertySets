using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    public class EnumList
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("EnumItem")]
        public List<string> Items { get; set; }
    }
}
