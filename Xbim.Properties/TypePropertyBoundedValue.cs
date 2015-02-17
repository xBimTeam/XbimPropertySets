using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertyBoundedValue.
    /// </summary>
    public class TypePropertyBoundedValue : IPropertyValueType
    {
        /// <summary>
        /// The container element of bound value.
        /// </summary>
        public ValueRangeDef ValueRangeDef { get; set; }

        /// <summary>
        /// The property data type.
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// The property data unit.
        /// </summary>
        public UnitType UnitType { get; set; }
    }
}
