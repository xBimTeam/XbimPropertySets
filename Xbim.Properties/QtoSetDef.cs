using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    [XmlRoot("QtoSetDef")]
    public class QtoSetDef : QuantityPropertySetDef
    {
        public string MethodOfMeasurement { get; set; }
        public string ApplicableTypeValue { get; set; }

        [XmlArray("QtoDefs")]
        [XmlArrayItem("QtoDef")]
        public List<QtoDef> QuantityDefinitions { get; set; }

        [XmlArrayItem("QtoDefinitionAlias")]
        [XmlArray("QtoDefinitionAliases")]
        public List<NameAlias> _QtoDefinitionAliases { get { return DefinitionAliases; } set { DefinitionAliases = value; } }

        [XmlIgnore]
        public override IEnumerable<QuantityPropertyDef> Definitions
        {
            get
            {
                if (QuantityDefinitions != null)
                    foreach (var item in QuantityDefinitions)
                    {
                        yield return item;
                    }
            }
        }
    }
}
