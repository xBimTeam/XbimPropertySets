using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of property type.
    /// </summary>
    public class PropertyType
    {
        [XmlElement("TypePropertySingleValue", typeof(TypePropertySingleValue))]
        [XmlElement("TypePropertyEnumeratedValue", typeof(TypePropertyEnumeratedValue))]
        [XmlElement("TypePropertyBoundedValue", typeof(TypePropertyBoundedValue))]
        [XmlElement("TypePropertyTableValue", typeof(TypePropertyTableValue))]
        [XmlElement("TypePropertyReferenceValue", typeof(TypePropertyReferenceValue))]
        [XmlElement("TypePropertyListValue", typeof(TypePropertyListValue))]
        [XmlElement("TypeComplexProperty", typeof(TypeComplexProperty))]
        public object _value { get; set; }

        [XmlIgnore]
        public IPropertyValueType PropertyValueType { get { return _value as IPropertyValueType; } set { _value = value; } }
    }
}
