using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertySingleValue.
    /// </summary>
    public class TypeSimpleProperty : IPropertyValueType
    {
        public override string ToString()
        {
            if (UnitType == null)
                return DataType.Type;
            return DataType.Type = UnitType.Type;
        }

        /// <summary>
        /// The element of property data type.
        /// </summary>
        [XmlElement("DataType")]
        public SimplePropertyDataType DataType { get; set; }

        /// <summary>
        /// The element of property data unit.
        /// </summary>
        [XmlElement("UnitType")]
        public SimplePropertyUnitType UnitType { get; set; }

        [XmlIgnore]
        public string DataTypeValue => DataType.Type;

        [XmlIgnore]
        public string UnitTypeValue => UnitType?.Type ?? "";

    }
}
