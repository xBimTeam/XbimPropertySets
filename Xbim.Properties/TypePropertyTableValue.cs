using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of IfcPropertyTableValue.
    /// </summary>
    public class TypePropertyTableValue : IPropertyValueType
    {
        /// <summary>
        /// The information about the expression for the derivation of defined values from the defining values.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// The list of defining values, which determine the defined values. This list shall have unique values only.
        /// </summary>
        public Value DefiningValue { get; set; }

        /// <summary>
        /// The defined values which are applicable for the scope as defined by the defining values.
        /// </summary>
        public Value DefinedValue { get; set; }
    }
}
