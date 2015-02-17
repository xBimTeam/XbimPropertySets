using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The container element of bound value.
    /// </summary>
    public class ValueRangeDef
    {
        /// <summary>
        /// The lower bound value.
        /// </summary>
        public LowerBoundValue LowerBoundValue { get; set; }

        /// <summary>
        /// The upper bound value.
        /// </summary>
        public UpperBoundValue UpperBoundValue { get; set; }
    }

    public class LowerBoundValue
    {
        /// <summary>
        /// The lower value.
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }
    }

    public class UpperBoundValue
    {
        /// <summary>
        /// The upper value.
        /// </summary>
        [XmlAttribute]
        public string Value { get; set; }
    }
}
