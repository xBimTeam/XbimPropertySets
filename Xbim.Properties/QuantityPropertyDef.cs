using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    [XmlInclude(typeof(QtoDef))]
    [XmlInclude(typeof(PropertyDef))]
    public class QuantityPropertyDef
    {
        /// <summary>
        /// The name of property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The definition of property.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Name aliases in different languages.
        /// </summary>
        [XmlArrayItem("NameAlias")]
        [XmlArray("NameAliases")]
        public List<NameAlias> NameAliases { get; set; }

        /// <summary>
        /// Definition aliases in different languages.
        /// </summary>
        [XmlArrayItem("DefinitionAlias")]
        [XmlArray("DefinitionAliases")]
        public List<NameAlias> DefinitionAliases { get; set; }
    }
}
