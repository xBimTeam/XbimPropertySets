using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertyEnumeratedValue.
    /// </summary>
    public class TypePropertyEnumeratedValue : IPropertyValueType
    {
        /// <summary>
        /// The container element of enumeration list.
        /// </summary>
        public EnumList EnumList { get; set; }

        /// <summary>
        /// The container element of enumeration constants with localized names and descriptions.
        /// </summary>
        public List<ConstantDef> ConstantList { get; set; }
    }
}
