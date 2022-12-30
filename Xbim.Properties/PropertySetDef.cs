using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The top node element of Property Set Definition (PSD).
    /// </summary>
    [XmlRoot("PropertySetDef")]
    public class PropertySetDef: QuantityPropertySetDef
    {
        /// <summary>
        /// The description of applicability and usecases, i.e., "IfcDoor entity", "Common Pset of Ifc...".
        /// </summary>
        public string Applicability { get; set; }

        

        /// <summary>
        /// The format of applicable type value is ENTITY_TYPE/PREDEFINED_TYPE. Multiple value is accepted, and format is: "TYPE_1 | TYPE_2 ...".
        /// </summary>
        public string ApplicableTypeValue { get; set; }

        /// <summary>
        /// The container element of property definition.
        /// </summary>
        [XmlArray("PropertyDefs")]
        [XmlArrayItem("PropertyDef")]
        public List<PropertyDef> PropertyDefinitions { get; set; }

        /// <summary>
        /// The container element of property set definition alias.
        /// </summary>
        [XmlArrayItem("PsetDefinitionAlias")]
        [XmlArray("PsetDefinitionAliases")]
        public List<NameAlias> _PsetDefinitionAliases { get { return DefinitionAliases; } set { DefinitionAliases = value; } }

        /// <summary>
        /// The Globally Unique Identifier (GIUD) for the property set definition. The ID is referencing the IFD GUID.
        /// </summary>
        [XmlAttribute("ifdguid")]
        public string IfdGuid { get; set; }

        /// <summary>
        /// The property set template type.
        /// </summary>
        [XmlAttribute]
        public templatetype templatetype { get; set; }

        [XmlIgnore]
        public override IEnumerable<QuantityPropertyDef> Definitions
        {
            get
            {
                if (PropertyDefinitions != null)
                    foreach (var item in PropertyDefinitions)
                    {
                        yield return item;
                    }
            }
        }
    }

    public enum templatetype
    {
        NOTDEFINED,
        PSET_TYPEDRIVENONLY,
        PSET_TYPEDRIVENOVERRIDE,
        PSET_OCCURRENCEDRIVEN,
        PSET_PERFORMANCEDRIVEN,
        PSET_PROFILEDRIVEN,
        PSET_MATERIALDRIVEN,
        QTO_TYPEDRIVENONLY,
        QTO_TYPEDRIVENOVERRIDE,
        QTO_OCCURRENCEDRIVEN,
    }
}
