using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of property definition.
    /// </summary>
    public class PropertyDef : QuantityPropertyDef
    {
        /// <summary>
        /// Not in use. This element is deprecated.
        /// </summary>
        [Obsolete]
        public ValueDef ValueDef { get; set; }

        /// <summary>
        /// The container element of property type.
        /// </summary>
        public PropertyType PropertyType { get; set; }

        [XmlAttribute("ifdguid")]
        public string IfdGuid { get; set; }
    }
}
