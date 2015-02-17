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
    /// The element of data type.
    /// </summary>
    public class DataType
    {
        private DataTypeEnum? _type;

        /// <summary>
        /// The property data type based on IfcValue (IfcMeasureValue, IfcSimpleValue, and IfcDerivedMeasureValue of IFC4 Official Release).
        /// </summary>
        [XmlAttribute("type")]
        [DefaultValue("IfcText")]
        public string _Value {
            get
            {
                return _type.ToString();
            }
            set
            {
                DataTypeEnum type = DataTypeEnum.IfcText;
                if (String.IsNullOrEmpty(value))
                {
                    _type = null;
                }
                else if (Enum.TryParse<DataTypeEnum>(value, out type))
                {
                    _type = type;
                }
                else
                    throw new ArgumentOutOfRangeException(value + " is not valid IFC value type.");

            }
        }

        [XmlIgnore]
        public DataTypeEnum? Type { get { return _type; } set { _type = value; } }
    }

    public enum DataTypeEnum
    {
        IfcAmountOfSubstanceMeasure,
        IfcAreaMeasure,
        IfcComplexNumber,
        IfcContextDependentMeasure,
        IfcCountMeasure,
        IfcDescriptiveMeasure,
        IfcElectricCurrentMeasure,
        IfcLengthMeasure,
        IfcLuminousIntensityMeasure,
        IfcMassMeasure,
        IfcNonNegativeLengthMeasure,
        IfcNormalisedRatioMeasure,
        IfcNumericMeasure,
        IfcParameterValue,
        IfcPlaneAngleMeasure,
        IfcPositiveLengthMeasure,
        IfcPositivePlaneAngleMeasure,
        IfcPositiveRatioMeasure,
        IfcRatioMeasure,
        IfcSolidAngleMeasure,
        IfcThermodynamicTemperatureMeasure,
        IfcTimeMeasure,
        IfcVolumeMeasure,
        IfcBoolean,
        IfcDate,
        IfcDateTime,
        IfcDuration,
        IfcIdentifier,
        IfcInteger,
        IfcLabel,
        IfcLogical,
        IfcReal,
        IfcText,
        IfcTime,
        IfcTimeStamp,
        IfcAbsorbedDoseMeasure,
        IfcAccelerationMeasure,
        IfcAngularVelocityMeasure,
        IfcAreaDensityMeasure,
        IfcCompoundPlaneAngleMeasure,
        IfcCurvatureMeasure,
        IfcDoseEquivalentMeasure,
        IfcDynamicViscosityMeasure,
        IfcElectricCapacitanceMeasure,
        IfcElectricChargeMeasure,
        IfcElectricConductanceMeasure,
        IfcElectricResistanceMeasure,
        IfcElectricVoltageMeasure,
        IfcEnergyMeasure,
        IfcForceMeasure,
        IfcFrequencyMeasure,
        IfcHeatFluxDensityMeasure,
        IfcHeatingValueMeasure,
        IfcIlluminanceMeasure,
        IfcInductanceMeasure,
        IfcIntegerCountRateMeasure,
        IfcIonConcentrationMeasure,
        IfcIsothermalMoistureCapacityMeasure,
        IfcKinematicViscosityMeasure,
        IfcLinearForceMeasure,
        IfcLinearMomentMeasure,
        IfcLinearStiffnessMeasure,
        IfcLinearVelocityMeasure,
        IfcLuminousFluxMeasure,
        IfcLuminousIntensityDistributionMeasure,
        IfcMagneticFluxDensityMeasure,
        IfcMagneticFluxMeasure,
        IfcMassDensityMeasure,
        IfcMassFlowRateMeasure,
        IfcMassPerLengthMeasure,
        IfcModulusOfElasticityMeasure,
        IfcModulusOfLinearSubgradeReactionMeasure,
        IfcModulusOfRotationalSubgradeReactionMeasure,
        IfcModulusOfSubgradeReactionMeasure,
        IfcMoistureDiffusivityMeasure,
        IfcMolecularWeightMeasure,
        IfcMomentOfInertiaMeasure,
        IfcMonetaryMeasure,
        IfcPHMeasure,
        IfcPlanarForceMeasure,
        IfcPowerMeasure,
        IfcPressureMeasure,
        IfcRadioActivityMeasure,
        IfcRotationalFrequencyMeasure,
        IfcRotationalMassMeasure,
        IfcRotationalStiffnessMeasure,
        IfcSectionModulusMeasure,
        IfcSectionalAreaIntegralMeasure,
        IfcShearModulusMeasure,
        IfcSoundPowerLevelMeasure,
        IfcSoundPowerMeasure,
        IfcSoundPressureLevelMeasure,
        IfcSoundPressureMeasure,
        IfcSpecificHeatCapacityMeasure,
        IfcTemperatureGradientMeasure,
        IfcTemperatureRateOfChangeMeasure,
        IfcThermalAdmittanceMeasure,
        IfcThermalConductivityMeasure,
        IfcThermalExpansionCoefficientMeasure,
        IfcThermalResistanceMeasure,
        IfcThermalTransmittanceMeasure,
        IfcTorqueMeasure,
        IfcVaporPermeabilityMeasure,
        IfcVolumetricFlowRateMeasure,
        IfcWarpingConstantMeasure,
        IfcWarpingMomentMeasure
    }
}
