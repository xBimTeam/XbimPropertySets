using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertyListValue.
    /// </summary>
    public class TypePropertyListValue : IPropertyValueType
    {
        /// <summary>
        /// The container element of list value.
        /// </summary>
        public Value ListValue { get; set; }
    }
}
