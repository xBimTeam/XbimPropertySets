using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertySingleValue.
    /// </summary>
    public class TypePropertySingleValue : IPropertyValueType
    {
        /// <summary>
        /// The element of property data type.
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// The element of property data unit.
        /// </summary>
        public UnitType UnitType { get; set; }
    }
}
