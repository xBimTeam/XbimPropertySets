using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The name alias in local language.
    /// </summary>
    public class NameAlias
    {
        /// <summary>
        /// The language code based on ISO 639-1 and ISO 3166-1 alpha-2 codes, i.e., "de-DE", "ja-JP", "fr-FR", "no-NO".
        /// </summary>
        [XmlAttribute("lang")]
        public string Lang { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
