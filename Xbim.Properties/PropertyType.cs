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
        [XmlElement(nameof(TypeSimpleProperty), typeof(TypeSimpleProperty))]
        [XmlElement(nameof(TypePropertySingleValue), typeof(TypePropertySingleValue))]
        [XmlElement(nameof(TypePropertyEnumeratedValue), typeof(TypePropertyEnumeratedValue))]
        [XmlElement(nameof(TypePropertyBoundedValue), typeof(TypePropertyBoundedValue))]
        [XmlElement(nameof(TypePropertyTableValue), typeof(TypePropertyTableValue))]
        [XmlElement(nameof(TypePropertyReferenceValue), typeof(TypePropertyReferenceValue))]
        [XmlElement(nameof(TypePropertyListValue), typeof(TypePropertyListValue))]
        [XmlElement(nameof(TypeComplexProperty), typeof(TypeComplexProperty))]
        public object _value { get; set; }

        [XmlIgnore]
        public IPropertyValueType PropertyValueType { get { return _value as IPropertyValueType; } set { _value = value; } }
    }
}
