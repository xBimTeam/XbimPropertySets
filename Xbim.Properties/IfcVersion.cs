using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// Version information of IFC release and sub schema.
    /// </summary>
    public class IfcVersion
    {
        /// <summary>
        /// The version information of IFC, i.e., "2x3 TC1", "IFC4".
        /// </summary>
        [XmlAttribute]
        public string version { get; set; }

        /// <summary>
        /// The sub schema name,  i.e., "IfcSharedBldgElements".
        /// </summary>
        [XmlAttribute]
        public string schema { get; set; }
    }
}
