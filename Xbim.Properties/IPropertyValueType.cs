using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    [XmlInclude(typeof(TypePropertySingleValue))]
    [XmlInclude(typeof(TypePropertyEnumeratedValue))]
    [XmlInclude(typeof(TypePropertyBoundedValue))]
    [XmlInclude(typeof(TypePropertyTableValue))]
    [XmlInclude(typeof(TypePropertyReferenceValue))]
    [XmlInclude(typeof(TypePropertyListValue))]
    [XmlInclude(typeof(TypeComplexProperty))]
    public interface IPropertyValueType
    {
    }
}
