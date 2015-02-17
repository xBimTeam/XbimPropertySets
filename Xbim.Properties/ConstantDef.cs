using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of enumeration constaint definition.
    /// </summary>
    public class ConstantDef
    {
        /// <summary>
        /// The name of the enumeration constant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The definition of the enumeration constant.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// The container element of name alias.
        /// </summary>
        [XmlArray("NameAliases")]
        [XmlArrayItem("NameAlias")]
        public List<NameAlias> NameAliases { get; set; }

        [XmlArray("DefinitionAliases")]
        [XmlArrayItem("DefinitionAlias")]
        public List<NameAlias> DefinitionAliases { get; set; }
    }
}
