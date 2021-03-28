using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// The element of unit type.
    /// </summary>
    public class UnitType
    {
        private UnitTypeEnum? _type;

        /// <summary>
        /// The unit type based on IfcDerivedUnit(IfcDerivedUnitEnum) and IfcNamedUnit(IfcUnitEnum) of IFC4 Official Release.
        /// </summary>
        [XmlAttribute("type")]
        [DefaultValue("USERDEFINED")]
        public string _Value 
        {
            get
            {
                return _type.ToString();
            }
            set
            {
				if (string.IsNullOrEmpty(value))
					_type = null;
				else if (Enum.TryParse(value, out UnitTypeEnum type))
				{
					_type = type;
				}
				else
					throw new ArgumentOutOfRangeException(value);

			}
        }

        [XmlIgnore]
        public UnitTypeEnum? Type { get { return _type; } set { _type = value; } }

        [XmlAttribute("currencytype")]
        [DefaultValue("USERDEFINED")]
        public string CurrencyType { get; set; }
    }

    public enum UnitTypeEnum
    {
        ACCELERATIONUNIT,
        ANGULARVELOCITYUNIT,
        AREADENSITYUNIT,
        COMPOUNDPLANEANGLEUNIT,
        DYNAMICVISCOSITYUNIT,
        HEATFLUXDENSITYUNIT,
        INTEGERCOUNTRATEUNIT,
        ISOTHERMALMOISTURECAPACITYUNIT,
        KINEMATICVISCOSITYUNIT,
        LINEARFORCEUNIT,
        LINEARMOMENTUNIT,
        LINEARSTIFFNESSUNIT,
        LINEARVELOCITYUNIT,
        MASSDENSITYUNIT,
        MASSFLOWRATEUNIT,
        MODULUSOFELASTICITYUNIT,
        MODULUSOFSUBGRADEREACTIONUNIT,
        MOISTUREDIFFUSIVITYUNIT,
        MOLECULARWEIGHTUNIT,
        MOMENTORINERTIAUNIT,
        PLANARFORCEUNIT,
        ROTATIONALFREQUENCYUNIT,
        ROTATIONALSTIFFNESSUNIT,
        SHEARMODULUSUNIT,
        SPECIFICHEATCAPACITYUNIT,
        THERMALADMITTANCEUNIT,
        THERMALCONDUCTANCEUNIT,
        THERMALRESISTANCEUNIT,
        THERMALTRANSMITTANCEUNIT,
        TORQUEUNIT,
        VAPORPERMEABILITYUNIT,
        VOLUMETRICFLOWRATEUNIT,
        CURVATUREUNIT,
        HEATINGVALUEUNIT,
        IONCONCENTRATIONUNIT,
        LUMINOUSINTENSITYDISTRIBUTIONUNIT,
        MASSPERLENGTHUNIT,
        MODULUSOFLINEARSUBGRADEREACTIONUNIT,
        MODULUSOFROTATIONALSUBGRADEREACTIONUNIT,
        PHUNIT,
        ROTATIONALMASSUNIT,
        SECTIONAREAINTEGRALUNIT,
        SECTIONMODULUSUNIT,
        SOUNDPOWERLEVELUNIT,
        SOUNDPOWERUNIT,
        SOUNDPRESSURELEVELUNIT,
        SOUNDPRESSUREUNIT,
        TEMPERATUREGRADIENTUNIT,
        TEMPERATURERATEOFCHANGEUNIT,
        THERMALEXPANSIONCOEFFICIENTUNIT,
        WARPINGCONSTANTUNIT,
        WARPINGMOMENTUNIT,
        ABSORBEDDOSEUNIT,
        AMOUNTOFSUBSTANCEUNIT,
        AREAUNIT,
        DOSEEQUIVALENTUNIT,
        ELECTRICCAPACITANCEUNIT,
        ELECTRICCHARGEUNIT,
        ELECTRICCONDUCTANCEUNIT,
        ELECTRICCURRENTUNIT,
        ELECTRICRESISTANCEUNIT,
        ELECTRICVOLTAGEUNIT,
        ENERGYUNIT,
        FORCEUNIT,
        FREQUENCYUNIT,
        ILLUMINANCEUNIT,
        INDUCTANCEUNIT,
        LENGTHUNIT,
        LUMINOUSFLUXUNIT,
        LUMINOUSINTENSITYUNIT,
        MAGNETICFLUXDENSITYUNIT,
        MAGNETICFLUXUNIT,
        MASSUNIT,
        PLANEANGLEUNIT,
        POWERUNIT,
        PRESSUREUNIT,
        RADIOACTIVITYUNIT,
        SOLIDANGLEUNIT,
        THERMODYNAMICTEMPERATUREUNIT,
        TIMEUNIT,
        VOLUMEUNIT,
        USERDEFINED,

        [Obsolete]
        IFCMONETARYUNIT,
        [Obsolete]
        UNSPECIFIED
    }
}
