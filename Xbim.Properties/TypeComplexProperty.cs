using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcComplexProperty.
    /// </summary>
    public class TypeComplexProperty : List<PropertyDef>, IPropertyValueType
    {
        /// <summary>
        /// The name of complex property, i.e., "CP_*".
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
