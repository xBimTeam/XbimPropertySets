using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of reference select.
    /// </summary>
    public class ReferenceSelect
    {
        #region RefType
        private RefTypeEnum? _reftype;

        /// <summary>
        /// The reference type based on SELECT Type IfcObjectReferenceSelect.
        /// </summary>
        [XmlAttribute("reftype")]
        public string _Value
        {
            get
            {
                return _reftype.ToString();
            }
            set
            {
				if (string.IsNullOrEmpty(value))
				{
					_reftype = null;
				}
				else if (Enum.TryParse<RefTypeEnum>(value, out RefTypeEnum type))
				{
					_reftype = type;
				}
				else
					throw new ArgumentOutOfRangeException(value);

			}
        }

        public RefTypeEnum? ReferenceType
        {
            get { return _reftype; }
            set { _reftype = value; }
        }
        #endregion

        /// <summary>
        /// The GUID for reference.
        /// </summary>
        [XmlAttribute("guid")]
        public string Guid { get; set; }

        /// <summary>
        /// The URL for reference.
        /// </summary>
        [XmlAttribute("URL")]
        public string URL { get; set; }

        /// <summary>
        /// The library name for reference.
        /// </summary>
        [XmlAttribute("libraryname")]
        public string LibraryName { get; set; }

        /// <summary>
        /// The section information of reference.
        /// </summary>
        [XmlAttribute("sectionref")]
        public string SectionReference { get; set; }
    }

    public enum RefTypeEnum
    {
        IfcMaterialDefinition,
        IfcPerson,
        IfcOrganization,
        IfcPersonAndOrganization,
        IfcExternalReference,
        IfcTimeSeries,
        IfcAddress,
        IfcAppliedValue,

        [Obsolete]
        IfcMaterial,
        [Obsolete]
        IfcDateAndTime,
        [Obsolete]
        IfcMaterialList,
        [Obsolete]
        IfcCalendarDate,
        [Obsolete]
        IfcLocalTime,
        [Obsolete]
        IfcMaterialLayer,
        [Obsolete]
        IfcClassificationReference
    }
}
