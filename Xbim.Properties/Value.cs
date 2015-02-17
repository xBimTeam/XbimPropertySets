using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The defined values which are applicable for the scope as defined by the defining values.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// The defining value data type.
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// The defining value unit.
        /// </summary>
        public UnitType UnitType { get; set; }

        /// <summary>
        /// The container element of values.
        /// </summary>
        [XmlArrayItem("ValueItem")]
        [XmlArray("Values")]
        public List<string> Values { get; set; }
    }
}
