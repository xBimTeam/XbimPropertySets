using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    /// <summary>
    /// This is the main class used to work with property set definitions.
    /// It implements buildingSMART data model for definition of properties and quantities for IFC4
    /// It also extends the model with a few deprecated values so that it is backwards compatible with 
    /// IFC 2x3 and it supports its property sets. Definitions of all property sets for IFC 2x3 and IFC4 and quantity sets 
    /// for IFC4 are part of the library so it is independent. It can also be used to define new
    /// property sets and to save them in the format compliant with buildingSMART data specification.
    /// </summary>
    public class Definitions<T> where T : QuantityPropertySetDef
    {
        private List<T> _definitions = new List<T>();
        private XmlSerializer _serializer;
        private Version _version;

        public Definitions(Version version)
        {
            _version = version;
            switch (version)
            {
                case Version.IFC4:
                    var ns = "";
                    if (typeof(T) == typeof(PropertySetDef))
                        ns = "http://buildingSMART-tech.org/xml/psd/PSD_IFC4.xsd";
                    if (typeof(T) == typeof(QtoSetDef))
                        ns = "http://www.buildingsmart-tech.org/xml/qto/QTO_IFC4.xsd";
                    _serializer = new XmlSerializer(typeof(T), ns);
                    break;
                case Version.IFC2x3:
                    _serializer = new XmlSerializer(typeof(T));
                    break;
            }
        }

        public void Add(T item)
        {
            _definitions.Add(item);
        }

        public void Add(IEnumerable<T> items)
        {
            _definitions.AddRange(items);
        }

        public IEnumerable<T> DefinitionSets
        {
            get {
                return _definitions;
            }
        }

        public T this[string name]
        {
            get
            {
                return _definitions.FirstOrDefault(d => d.Name == name);
            }
        }

        public void LoadFromDirectory(string directory, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            if (!Directory.Exists(directory))
                throw new ArgumentException("Directory doesn't exist.");

            var files = Directory.EnumerateFiles(directory, "*.xml", option);
            foreach (var file in files)
            {
                Load(file);
            }
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("Invalid path to file");

            using (var file = File.OpenRead(path))
            {
                Load(file);
                file.Close();
            }
        }

        public void Load(Stream stream)
        {
            var data = (T)_serializer.Deserialize(stream);
            if (data != null)
            {
                this.Add(data);
            }
        }

        public void Load(TextReader reader)
        {
            var data = (T)_serializer.Deserialize(reader);
            if (data != null)
            {
                this.Add(data);
            }
        }

        public void Save(string path, string name)
        {
            var def = this[name];
            if (def != null)
                Save(path, def);
        }

        public void Save(string path, T pSet)
        {
            using (var file = File.Create(path))
            {
                Save(file, pSet);
                file.Close();
            }
        }

        public void Save(Stream stream, T pSet)
        {
            _serializer.Serialize(stream, pSet);
        }

        public void SaveToDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            foreach (var pDef in this._definitions)
            {
                var file = Path.Combine(directory, (pDef.Name ?? "Unknown") + ".xml");
                Save(file, pDef);
            }
        }

        public IEnumerable<TP> GetAllProperties<TP>(bool nested = true) where TP : QuantityPropertyDef
        {
            return GetPropertiesWhere<TP>(p => true, nested);
        }

        public IEnumerable<TP> GetPropertiesWhere<TP>(Func<TP, bool> predicate, bool nested = true) where TP : QuantityPropertyDef
        {
            foreach (var set in this._definitions)
            {
                foreach (var qpDef in set.Definitions)
                {
                    var def = qpDef as TP;
                    if (def == null)
                        continue;

                    //check predicate
                    if (predicate(def))
                        yield return def;

                    if (!nested)
                        continue;

                    var pDef = qpDef as PropertyDef;
                    if (pDef == null)
                        continue;

                    //process nested properties in complex properties
                    var cType = pDef.PropertyType.PropertyValueType as TypeComplexProperty;
                    if (cType == null) continue;
                    foreach (var nDef in cType.OfType<TP>().Where(predicate))
                    {
                        yield return nDef;
                    }
                }
            }
        }

        public void LoadAllDefault()
        {
            ResourceManager mgr = null;
            List<string> definitionNames = null;

            if (_version == Version.IFC2x3)
            {
                if (typeof(T) == typeof(QtoSetDef))
                    throw new Exception("No quantity sets are defined for IFC 2x3 in definition files.");
                else
                {
                    mgr = Definitions.IFC2x3_Defitinion_files.ResourceManager;
                    definitionNames = DefinitionsIFC2x3;
                }
            }
            if (_version == Version.IFC4)
            {
                if (typeof(T) == typeof(QtoSetDef))
                {
                    mgr = Definitions.IFC4_QTO_Definition_files.ResourceManager;
                    definitionNames = DefinitionsIFC4_QTO;
                }
                else
                {
                    mgr = Definitions.IFC4_Definition_files.ResourceManager;
                    definitionNames = DefinitionsIFC4;
                }
            }

            if (mgr == null || definitionNames == null)
                return;

            foreach (var item in definitionNames)
            {
                var data = mgr.GetString(item, System.Globalization.CultureInfo.InvariantCulture);
                var reader = new StringReader(data);
                Load(reader);
            }
        }

      

        #region List of definition names
        private List<string> DefinitionsIFC2x3 = new List<string>() 
        {
            "Pset_ActionRequest",
            "Pset_ActorCommon",
            "Pset_ActuatorTypeCommon",
            "Pset_ActuatorTypeElectricActuator",
            "Pset_ActuatorTypeHydraulicActuator",
            "Pset_ActuatorTypeLinearActuation",
            "Pset_ActuatorTypePneumaticActuator",
            "Pset_ActuatorTypeRotationalActuation",
            "Pset_AirSideSystemInformation",
            "Pset_AirTerminalBoxPHistory",
            "Pset_AirTerminalBoxTypeCommon",
            "Pset_AirTerminalPHistory",
            "Pset_AirTerminalTypeCommon",
            "Pset_AirTerminalTypeRectangular",
            "Pset_AirTerminalTypeRound",
            "Pset_AirTerminalTypeSlot",
            "Pset_AirTerminalTypeSquare",
            "Pset_AirToAirHeatRecoveryPHist",
            "Pset_AirToAirHeatRecoveryTypeCommon",
            "Pset_AnalogInput",
            "Pset_AnalogOutput",
            "Pset_Asset",
            "Pset_BeamCommon",
            "Pset_BinaryInput",
            "Pset_BinaryOutput",
            "Pset_BoilerPHistory",
            "Pset_BoilerTypeCommon",
            "Pset_BoilerTypeSteam",
            "Pset_BuildingCommon",
            "Pset_BuildingElementProxyCommon",
            "Pset_BuildingStoreyCommon",
            "Pset_BuildingUse",
            "Pset_BuildingUseAdjacent",
            "Pset_BuildingWaterStorage",
            "Pset_CableCarrierSegmentTypeCableLadderSegment",
            "Pset_CableCarrierSegmentTypeCableTraySegment",
            "Pset_CableCarrierSegmentTypeCableTrunkingSegment",
            "Pset_CableCarrierSegmentTypeConduitSegment",
            "Pset_CableSegmentTypeCableSegment",
            "Pset_CableSegmentTypeConductorSegment",
            "Pset_ChillerPHistory",
            "Pset_ChillerTypeCommon",
            "Pset_CoilPHistory",
            "Pset_CoilTypeCommon",
            "Pset_CoilTypeHydronic",
            "Pset_ColumnCommon",
            "Pset_CompressorPHistory",
            "Pset_CompressorTypeCommon",
            "Pset_ConcreteElementGeneral",
            "Pset_ConcreteElementQuantityGeneral",
            "Pset_ConcreteElementSurfaceFinishQuantityGeneral",
            "Pset_CondenserPHistory",
            "Pset_CondenserTypeCommon",
            "Pset_ControllerTypeCommon",
            "Pset_ControllerTypeProportional",
            "Pset_ControllerTypeTwoPosition",
            "Pset_CooledBeamPHistory",
            "Pset_CooledBeamPHistoryActive",
            "Pset_CooledBeamTypeActive",
            "Pset_CooledBeamTypeCommon",
            "Pset_CoolingTowerPHistory",
            "Pset_CoolingTowerTypeCommon",
            "Pset_CoveringCeiling",
            "Pset_CoveringCommon",
            "Pset_CoveringFlooring",
            "Pset_CurtainWallCommon",
            "Pset_DamperPHistory",
            "Pset_DamperTypeCommon",
            "Pset_DamperTypeControlDamper",
            "Pset_DamperTypeFireDamper",
            "Pset_DamperTypeFireSmokeDamper",
            "Pset_DamperTypeSmokeDamper",
            "Pset_DesignPoint",
            "Pset_DiscreteAccessoryAnchorBolt",
            "Pset_DiscreteAccessoryColumnShoe",
            "Pset_DiscreteAccessoryCornerFixingPlate",
            "Pset_DiscreteAccessoryDiagonalTrussConnector",
            "Pset_DiscreteAccessoryEdgeFixingPlate",
            "Pset_DiscreteAccessoryFixingSocket",
            "Pset_DiscreteAccessoryLadderTrussConnector",
            "Pset_DiscreteAccessoryStandardFixingPlate",
            "Pset_DiscreteAccessoryWireLoop",
            "Pset_DistributionChamberElementTypeFormedDuct",
            "Pset_DistributionChamberElementTypeInspectionChamber",
            "Pset_DistributionChamberElementTypeInspectionPit",
            "Pset_DistributionChamberElementTypeManhole",
            "Pset_DistributionChamberElementTypeMeterChamber",
            "Pset_DistributionChamberElementTypeSump",
            "Pset_DistributionChamberElementTypeTrench",
            "Pset_DistributionChamberElementTypeValveChamber",
            "Pset_DistributionFlowElementCommon",
            "Pset_DistributionPortDuct",
            "Pset_DistributionPortPipe",
            "Pset_DoorCommon",
            "Pset_DoorWindowGlazingType",
            "Pset_DoorWindowShadingType",
            "Pset_DrainageCatchment",
            "Pset_DrainageCulvert",
            "Pset_DrainageOutfall",
            "Pset_DrainageReserve",
            "Pset_Draughting",
            "Pset_DuctConnection",
            "Pset_DuctDesignCriteria",
            "Pset_DuctFittingPHistory",
            "Pset_DuctFittingTypeCommon",
            "Pset_DuctSegmentPHistory",
            "Pset_DuctSegmentTypeCommon",
            "Pset_DuctSilencerPHistory",
            "Pset_DuctSilencerTypeCommon",
            "Pset_ElectricalCircuit",
            "Pset_ElectricalDeviceCommon",
            "Pset_ElectricDistributionPointCommon",
            "Pset_ElectricGeneratorTypeCommon",
            "Pset_ElectricHeaterTypeElectricalCableHeater",
            "Pset_ElectricHeaterTypeElectricalMatHeater",
            "Pset_ElectricHeaterTypeElectricalPointHeater",
            "Pset_ElectricMotorTypeCommon",
            "Pset_ElementShading",
            "Pset_EnergyConsumptionPHistoryElectricity",
            "Pset_EnergyConsumptionPHistoryFuel",
            "Pset_EnergyConsumptionPHistorySteam",
            "Pset_EnergyConversionDeviceCoil",
            "Pset_EnergyConversionDeviceSpaceHeaterPanel",
            "Pset_EnergyConversionDeviceSpaceHeaterSectional",
            "Pset_EvaporativeCoolerPHistory",
            "Pset_EvaporativeCoolerTypeCommon",
            "Pset_EvaporatorPHistory",
            "Pset_EvaporatorTypeCommon",
            "Pset_FanPHistory",
            "Pset_FanTypeCommon",
            "Pset_FanTypeSmokeControl",
            "Pset_FilterPHistory",
            "Pset_FilterTypeAirParticleFilter",
            "Pset_FilterTypeCommon",
            "Pset_FireRatingProperties",
            "Pset_FireSuppressionTerminalTypeBreechingInlet",
            "Pset_FireSuppressionTerminalTypeFireHydrant",
            "Pset_FireSuppressionTerminalTypeHoseReel",
            "Pset_FireSuppressionTerminalTypeSprinkler",
            "Pset_FlowControllerDamper",
            "Pset_FlowControllerFlowMeter",
            "Pset_FlowFittingDuctFitting",
            "Pset_FlowFittingPipeFitting",
            "Pset_FlowInstrumentTypePressureGauge",
            "Pset_FlowInstrumentTypeThermometer",
            "Pset_FlowMeterTypeCommon",
            "Pset_FlowMeterTypeEnergyMeter",
            "Pset_FlowMeterTypeGasMeter",
            "Pset_FlowMeterTypeOilMeter",
            "Pset_FlowMeterTypeWaterMeter",
            "Pset_FlowMovingDeviceCompressor",
            "Pset_FlowMovingDeviceFan",
            "Pset_FlowMovingDeviceFanCentrifugal",
            "Pset_FlowMovingDevicePump",
            "Pset_FlowSegmentDuctSegment",
            "Pset_FlowSegmentPipeSegment",
            "Pset_FlowStorageDeviceTank",
            "Pset_FlowTerminalAirTerminal",
            "Pset_FurnitureTypeChair",
            "Pset_FurnitureTypeCommon",
            "Pset_FurnitureTypeDesk",
            "Pset_FurnitureTypeFileCabinet",
            "Pset_FurnitureTypeTable",
            "Pset_GasTerminalPHistory",
            "Pset_GasTerminalTypeCommon",
            "Pset_GasTerminalTypeGasAppliance",
            "Pset_GasTerminalTypeGasBurner",
            "Pset_HeatExchangerTypeCommon",
            "Pset_HeatExchangerTypePlate",
            "Pset_HumidifierPHistory",
            "Pset_HumidifierTypeCommon",
            "Pset_LampTypeCommon",
            "Pset_LightFixtureTypeCommon",
            "Pset_LightFixtureTypeExitSign",
            "Pset_LightFixtureTypeThermal",
            "Pset_ManufacturerOccurrence",
            "Pset_ManufacturerTypeInformation",
            "Pset_MemberCommon",
            "Pset_MultiStateInput",
            "Pset_MultiStateOutput",
            "Pset_OpeningElementCommon",
            "Pset_OutletTypeCommon",
            "Pset_OutsideDesignCriteria",
            "Pset_PackingInstructions",
            "Pset_Permit",
            "Pset_PipeConnection",
            "Pset_PipeConnectionFlanged",
            "Pset_PipeFittingPHistory",
            "Pset_PipeFittingTypeCommon",
            "Pset_PipeSegmentPHistory",
            "Pset_PipeSegmentTypeCommon",
            "Pset_PipeSegmentTypeGutter",
            "Pset_PlateCommon",
            "Pset_PrecastConcreteElementGeneral",
            "Pset_ProductRequirements",
            "Pset_ProjectCommon",
            "Pset_ProjectionElementShadingDevicePHistory",
            "Pset_ProjectOrderChangeOrder",
            "Pset_ProjectOrderMaintenanceWorkOrder",
            "Pset_ProjectOrderMoveOrder",
            "Pset_ProjectOrderPurchaseOrder",
            "Pset_ProjectOrderWorkOrder",
            "Pset_PropertyAgreement",
            "Pset_ProtectiveDeviceTypeCircuitBreaker",
            "Pset_ProtectiveDeviceTypeCommon",
            "Pset_ProtectiveDeviceTypeEarthFailureDevice",
            "Pset_ProtectiveDeviceTypeFuseDisconnector",
            "Pset_ProtectiveDeviceTypeResidualCurrentCircuitBreaker",
            "Pset_ProtectiveDeviceTypeResidualCurrentSwitch",
            "Pset_ProtectiveDeviceTypeVaristor",
            "Pset_PumpPHistory",
            "Pset_PumpTypeCommon",
            "Pset_QuantityTakeOff",
            "Pset_RailingCommon",
            "Pset_RampCommon",
            "Pset_RampFlightCommon",
            "Pset_ReinforcementBarCountOfIndependentFooting",
            "Pset_ReinforcementBarPitchOfBeam",
            "Pset_ReinforcementBarPitchOfColumn",
            "Pset_ReinforcementBarPitchOfContinuousFooting",
            "Pset_ReinforcementBarPitchOfSlab",
            "Pset_ReinforcementBarPitchOfWall",
            "Pset_ReinforcingBarBendingsBECCommon",
            "Pset_ReinforcingBarBendingsBS8666Common",
            "Pset_ReinforcingBarBendingsDIN135610Common",
            "Pset_ReinforcingBarBendingsISOCD3766Common",
            "Pset_Reliability",
            "Pset_Risk",
            "Pset_RoofCommon",
            "Pset_SanitaryTerminalTypeBath",
            "Pset_SanitaryTerminalTypeBidet",
            "Pset_SanitaryTerminalTypeCistern",
            "Pset_SanitaryTerminalTypeSanitaryFountain",
            "Pset_SanitaryTerminalTypeShower",
            "Pset_SanitaryTerminalTypeSink",
            "Pset_SanitaryTerminalTypeToiletPan",
            "Pset_SanitaryTerminalTypeUrinal",
            "Pset_SanitaryTerminalTypeWashHandBasin",
            "Pset_SanitaryTerminalTypeWCSeat",
            "Pset_SensorTypeCO2Sensor",
            "Pset_SensorTypeFireSensor",
            "Pset_SensorTypeGasSensor",
            "Pset_SensorTypeHeatSensor",
            "Pset_SensorTypeHumiditySensor",
            "Pset_SensorTypeLightSensor",
            "Pset_SensorTypeMovementSensor",
            "Pset_SensorTypePressureSensor",
            "Pset_SensorTypeSmokeSensor",
            "Pset_SensorTypeSoundSensor",
            "Pset_SensorTypeTemperatureSensor",
            "Pset_SiteCommon",
            "Pset_SlabCommon",
            "Pset_SpaceCommon",
            "Pset_SpaceFireSafetyRequirements",
            "Pset_SpaceHeaterPHistoryCommon",
            "Pset_SpaceHeaterTypeCommon",
            "Pset_SpaceHeaterTypeHydronic",
            "Pset_SpaceLightingRequirements",
            "Pset_SpaceOccupancyRequirements",
            "Pset_SpaceParking",
            "Pset_SpaceParkingAisle",
            "Pset_SpaceProgramCommon",
            "Pset_SpaceThermalDesign",
            "Pset_SpaceThermalPHistory",
            "Pset_SpaceThermalRequirements",
            "Pset_StairCommon",
            "Pset_StairFlightCommon",
            "Pset_SwitchingDeviceTypeCommon",
            "Pset_SwitchingDeviceTypeContactor",
            "Pset_SwitchingDeviceTypeEmergencyStop",
            "Pset_SwitchingDeviceTypeStarter",
            "Pset_SwitchingDeviceTypeSwitchDisconnector",
            "Pset_SwitchingDeviceTypeToggleSwitch",
            "Pset_SystemFurnitureElementTypeCommon",
            "Pset_SystemFurnitureElementTypePanel",
            "Pset_SystemFurnitureElementTypeWorkSurface",
            "Pset_TankTypeCommon",
            "Pset_TankTypeExpansion",
            "Pset_TankTypePreformed",
            "Pset_TankTypePressureVessel",
            "Pset_TankTypeSectional",
            "Pset_ThermalLoadAggregate",
            "Pset_ThermalLoadDesignCriteria",
            "Pset_TransformerTypeCommon",
            "Pset_TransportElementCommon",
            "Pset_TransportElementElevator",
            "Pset_TubeBundleTypeCommon",
            "Pset_TubeBundleTypeFinned",
            "Pset_UnitaryEquipmentTypeAirConditioningUnit",
            "Pset_UnitaryEquipmentTypeAirHandler",
            "Pset_UtilityConsumption",
            "Pset_ValvePHistory",
            "Pset_ValveTypeAirRelease",
            "Pset_ValveTypeCommon",
            "Pset_ValveTypeDrawOffCock",
            "Pset_ValveTypeFaucet",
            "Pset_ValveTypeFlushing",
            "Pset_ValveTypeGasTap",
            "Pset_ValveTypeIsolating",
            "Pset_ValveTypeMixing",
            "Pset_ValveTypePressureReducing",
            "Pset_ValveTypePressureRelief",
            "Pset_VibrationIsolatorTypeCommon",
            "Pset_WallCommon",
            "Pset_Warranty",
            "Pset_WasteTerminalTypeFloorTrap",
            "Pset_WasteTerminalTypeFloorWaste",
            "Pset_WasteTerminalTypeGreaseInterceptor",
            "Pset_WasteTerminalTypeGullySump",
            "Pset_WasteTerminalTypeGullyTrap",
            "Pset_WasteTerminalTypeOilInterceptor",
            "Pset_WasteTerminalTypePetrolInterceptor",
            "Pset_WasteTerminalTypeRoofDrain",
            "Pset_WasteTerminalTypeWasteDisposalUnit",
            "Pset_WasteTerminalTypeWasteTrap",
            "Pset_WindowCommon",
            "Pset_ZoneCommon"
        };

        private List<string> DefinitionsIFC4 = new List<string>() 
        {
            "Pset_ActionRequest",
            "Pset_ActorCommon",
            "Pset_ActuatorPHistory",
            "Pset_ActuatorTypeCommon",
            "Pset_ActuatorTypeElectricActuator",
            "Pset_ActuatorTypeHydraulicActuator",
            "Pset_ActuatorTypeLinearActuation",
            "Pset_ActuatorTypePneumaticActuator",
            "Pset_ActuatorTypeRotationalActuation",
            "Pset_AirSideSystemInformation",
            "Pset_AirTerminalBoxPHistory",
            "Pset_AirTerminalBoxTypeCommon",
            "Pset_AirTerminalOccurrence",
            "Pset_AirTerminalPHistory",
            "Pset_AirTerminalTypeCommon",
            "Pset_AirToAirHeatRecoveryPHistory",
            "Pset_AirToAirHeatRecoveryTypeCommon",
            "Pset_AlarmPHistory",
            "Pset_AlarmTypeCommon",
            "Pset_AnnotationContourLine",
            "Pset_AnnotationLineOfSight",
            "Pset_AnnotationSurveyArea",
            "Pset_Asset",
            "Pset_AudioVisualAppliancePHistory",
            "Pset_AudioVisualApplianceTypeAmplifier",
            "Pset_AudioVisualApplianceTypeCamera",
            "Pset_AudioVisualApplianceTypeCommon",
            "Pset_AudioVisualApplianceTypeDisplay",
            "Pset_AudioVisualApplianceTypePlayer",
            "Pset_AudioVisualApplianceTypeProjector",
            "Pset_AudioVisualApplianceTypeReceiver",
            "Pset_AudioVisualApplianceTypeSpeaker",
            "Pset_AudioVisualApplianceTypeTuner",
            "Pset_BeamCommon",
            "Pset_BoilerPHistory",
            "Pset_BoilerTypeCommon",
            "Pset_BoilerTypeSteam",
            "Pset_BoilerTypeWater",
            "Pset_BuildingCommon",
            "Pset_BuildingElementProxyCommon",
            "Pset_BuildingElementProxyProvisionForVoid",
            "Pset_BuildingStoreyCommon",
            "Pset_BuildingSystemCommon",
            "Pset_BuildingUse",
            "Pset_BuildingUseAdjacent",
            "Pset_BurnerTypeCommon",
            "Pset_CableCarrierFittingTypeCommon",
            "Pset_CableCarrierSegmentTypeCableLadderSegment",
            "Pset_CableCarrierSegmentTypeCableTraySegment",
            "Pset_CableCarrierSegmentTypeCableTrunkingSegment",
            "Pset_CableCarrierSegmentTypeCommon",
            "Pset_CableCarrierSegmentTypeConduitSegment",
            "Pset_CableFittingTypeCommon",
            "Pset_CableSegmentOccurrence",
            "Pset_CableSegmentTypeBusBarSegment",
            "Pset_CableSegmentTypeCableSegment",
            "Pset_CableSegmentTypeCommon",
            "Pset_CableSegmentTypeConductorSegment",
            "Pset_CableSegmentTypeCoreSegment",
            "Pset_ChillerPHistory",
            "Pset_ChillerTypeCommon",
            "Pset_ChimneyCommon",
            "Pset_CoilOccurrence",
            "Pset_CoilPHistory",
            "Pset_CoilTypeCommon",
            "Pset_CoilTypeHydronic",
            "Pset_ColumnCommon",
            "Pset_CommunicationsAppliancePHistory",
            "Pset_CommunicationsApplianceTypeCommon",
            "Pset_CompressorPHistory",
            "Pset_CompressorTypeCommon",
            "Pset_ConcreteElementGeneral",
            "Pset_CondenserPHistory",
            "Pset_CondenserTypeCommon",
            "Pset_Condition",
            "Pset_ConstructionResource",
            "Pset_ControllerPHistory",
            "Pset_ControllerTypeCommon",
            "Pset_ControllerTypeFloating",
            "Pset_ControllerTypeMultiPosition",
            "Pset_ControllerTypeProgrammable",
            "Pset_ControllerTypeProportional",
            "Pset_ControllerTypeTwoPosition",
            "Pset_CooledBeamPHistory",
            "Pset_CooledBeamPHistoryActive",
            "Pset_CooledBeamTypeActive",
            "Pset_CooledBeamTypeCommon",
            "Pset_CoolingTowerPHistory",
            "Pset_CoolingTowerTypeCommon",
            "Pset_CoveringCeiling",
            "Pset_CoveringCommon",
            "Pset_CoveringFlooring",
            "Pset_CurtainWallCommon",
            "Pset_DamperOccurrence",
            "Pset_DamperPHistory",
            "Pset_DamperTypeCommon",
            "Pset_DamperTypeControlDamper",
            "Pset_DamperTypeFireDamper",
            "Pset_DamperTypeFireSmokeDamper",
            "Pset_DamperTypeSmokeDamper",
            "Pset_DiscreteAccessoryColumnShoe",
            "Pset_DiscreteAccessoryCornerFixingPlate",
            "Pset_DiscreteAccessoryDiagonalTrussConnector",
            "Pset_DiscreteAccessoryEdgeFixingPlate",
            "Pset_DiscreteAccessoryFixingSocket",
            "Pset_DiscreteAccessoryLadderTrussConnector",
            "Pset_DiscreteAccessoryStandardFixingPlate",
            "Pset_DiscreteAccessoryWireLoop",
            "Pset_DistributionChamberElementCommon",
            "Pset_DistributionChamberElementTypeFormedDuct",
            "Pset_DistributionChamberElementTypeInspectionChamber",
            "Pset_DistributionChamberElementTypeInspectionPit",
            "Pset_DistributionChamberElementTypeManhole",
            "Pset_DistributionChamberElementTypeMeterChamber",
            "Pset_DistributionChamberElementTypeSump",
            "Pset_DistributionChamberElementTypeTrench",
            "Pset_DistributionChamberElementTypeValveChamber",
            "Pset_DistributionPortCommon",
            "Pset_DistributionPortPHistoryCable",
            "Pset_DistributionPortPHistoryDuct",
            "Pset_DistributionPortPHistoryPipe",
            "Pset_DistributionPortTypeCable",
            "Pset_DistributionPortTypeDuct",
            "Pset_DistributionPortTypePipe",
            "Pset_DistributionSystemCommon",
            "Pset_DistributionSystemTypeElectrical",
            "Pset_DistributionSystemTypeVentilation",
            "Pset_DoorCommon",
            "Pset_DoorWindowGlazingType",
            "Pset_DoorWindowShadingType",
            "Pset_DuctFittingOccurrence",
            "Pset_DuctFittingPHistory",
            "Pset_DuctFittingTypeCommon",
            "Pset_DuctSegmentOccurrence",
            "Pset_DuctSegmentPHistory",
            "Pset_DuctSegmentTypeCommon",
            "Pset_DuctSilencerPHistory",
            "Pset_DuctSilencerTypeCommon",
            "Pset_ElectricalDeviceCommon",
            "Pset_ElectricAppliancePHistory",
            "Pset_ElectricApplianceTypeCommon",
            "Pset_ElectricApplianceTypeDishwasher",
            "Pset_ElectricApplianceTypeElectricCooker",
            "Pset_ElectricDistributionBoardOccurrence",
            "Pset_ElectricDistributionBoardTypeCommon",
            "Pset_ElectricFlowStorageDeviceTypeCommon",
            "Pset_ElectricGeneratorTypeCommon",
            "Pset_ElectricMotorTypeCommon",
            "Pset_ElectricTimeControlTypeCommon",
            "Pset_ElementComponentCommon",
            "Pset_EngineTypeCommon",
            "Pset_EnvironmentalImpactIndicators",
            "Pset_EnvironmentalImpactValues",
            "Pset_EvaporativeCoolerPHistory",
            "Pset_EvaporativeCoolerTypeCommon",
            "Pset_EvaporatorPHistory",
            "Pset_EvaporatorTypeCommon",
            "Pset_FanCentrifugal",
            "Pset_FanOccurrence",
            "Pset_FanPHistory",
            "Pset_FanTypeCommon",
            "Pset_FastenerWeld",
            "Pset_FilterPHistory",
            "Pset_FilterTypeAirParticleFilter",
            "Pset_FilterTypeCommon",
            "Pset_FilterTypeCompressedAirFilter",
            "Pset_FilterTypeWaterFilter",
            "Pset_FireSuppressionTerminalTypeBreechingInlet",
            "Pset_FireSuppressionTerminalTypeCommon",
            "Pset_FireSuppressionTerminalTypeFireHydrant",
            "Pset_FireSuppressionTerminalTypeHoseReel",
            "Pset_FireSuppressionTerminalTypeSprinkler",
            "Pset_FlowInstrumentPHistory",
            "Pset_FlowInstrumentTypeCommon",
            "Pset_FlowInstrumentTypePressureGauge",
            "Pset_FlowInstrumentTypeThermometer",
            "Pset_FlowMeterOccurrence",
            "Pset_FlowMeterTypeCommon",
            "Pset_FlowMeterTypeEnergyMeter",
            "Pset_FlowMeterTypeGasMeter",
            "Pset_FlowMeterTypeOilMeter",
            "Pset_FlowMeterTypeWaterMeter",
            "Pset_FootingCommon",
            "Pset_FurnitureTypeChair",
            "Pset_FurnitureTypeCommon",
            "Pset_FurnitureTypeDesk",
            "Pset_FurnitureTypeFileCabinet",
            "Pset_FurnitureTypeTable",
            "Pset_HeatExchangerTypeCommon",
            "Pset_HeatExchangerTypePlate",
            "Pset_HumidifierPHistory",
            "Pset_HumidifierTypeCommon",
            "Pset_InterceptorTypeCommon",
            "Pset_JunctionBoxTypeCommon",
            "Pset_LampTypeCommon",
            "Pset_LandRegistration",
            "Pset_LightFixtureTypeCommon",
            "Pset_LightFixtureTypeSecurityLighting",
            "Pset_ManufacturerOccurrence",
            "Pset_ManufacturerTypeInformation",
            "Pset_MaterialCombustion",
            "Pset_MaterialCommon",
            "Pset_MaterialConcrete",
            "Pset_MaterialEnergy",
            "Pset_MaterialFuel",
            "Pset_MaterialHygroscopic",
            "Pset_MaterialMechanical",
            "Pset_MaterialOptical",
            "Pset_MaterialSteel",
            "Pset_MaterialThermal",
            "Pset_MaterialWater",
            "Pset_MaterialWood",
            "Pset_MaterialWoodBasedBeam",
            "Pset_MaterialWoodBasedPanel",
            "Pset_MechanicalFastenerAnchorBolt",
            "Pset_MechanicalFastenerBolt",
            "Pset_MedicalDeviceTypeCommon",
            "Pset_MemberCommon",
            "Pset_MotorConnectionTypeCommon",
            "Pset_OpeningElementCommon",
            "Pset_OutletTypeCommon",
            "Pset_OutsideDesignCriteria",
            "Pset_PackingInstructions",
            "Pset_Permit",
            "Pset_PileCommon",
            "Pset_PipeConnectionFlanged",
            "Pset_PipeFittingOccurrence",
            "Pset_PipeFittingPHistory",
            "Pset_PipeFittingTypeBend",
            "Pset_PipeFittingTypeCommon",
            "Pset_PipeFittingTypeJunction",
            "Pset_PipeSegmentOccurrence",
            "Pset_PipeSegmentPHistory",
            "Pset_PipeSegmentTypeCommon",
            "Pset_PipeSegmentTypeCulvert",
            "Pset_PipeSegmentTypeGutter",
            "Pset_PlateCommon",
            "Pset_PrecastConcreteElementFabrication",
            "Pset_PrecastConcreteElementGeneral",
            "Pset_PrecastSlab",
            "Pset_ProfileArbitraryDoubleT",
            "Pset_ProfileArbitraryHollowCore",
            "Pset_ProfileMechanical",
            "Pset_ProjectOrderChangeOrder",
            "Pset_ProjectOrderMaintenanceWorkOrder",
            "Pset_ProjectOrderMoveOrder",
            "Pset_ProjectOrderPurchaseOrder",
            "Pset_ProjectOrderWorkOrder",
            "Pset_PropertyAgreement",
            "Pset_ProtectiveDeviceBreakerUnitI2TCurve",
            "Pset_ProtectiveDeviceBreakerUnitI2TFuseCurve",
            "Pset_ProtectiveDeviceBreakerUnitIPICurve",
            "Pset_ProtectiveDeviceBreakerUnitTypeMCB",
            "Pset_ProtectiveDeviceBreakerUnitTypeMotorProtection",
            "Pset_ProtectiveDeviceOccurrence",
            "Pset_ProtectiveDeviceTrippingCurve",
            "Pset_ProtectiveDeviceTrippingFunctionGCurve",
            "Pset_ProtectiveDeviceTrippingFunctionICurve",
            "Pset_ProtectiveDeviceTrippingFunctionLCurve",
            "Pset_ProtectiveDeviceTrippingFunctionSCurve",
            "Pset_ProtectiveDeviceTrippingUnitCurrentAdjustment",
            "Pset_ProtectiveDeviceTrippingUnitTimeAdjustment",
            "Pset_ProtectiveDeviceTrippingUnitTypeCommon",
            "Pset_ProtectiveDeviceTrippingUnitTypeElectroMagnetic",
            "Pset_ProtectiveDeviceTrippingUnitTypeElectronic",
            "Pset_ProtectiveDeviceTrippingUnitTypeResidualCurrent",
            "Pset_ProtectiveDeviceTrippingUnitTypeThermal",
            "Pset_ProtectiveDeviceTypeCircuitBreaker",
            "Pset_ProtectiveDeviceTypeCommon",
            "Pset_ProtectiveDeviceTypeEarthLeakageCircuitBreaker",
            "Pset_ProtectiveDeviceTypeFuseDisconnector",
            "Pset_ProtectiveDeviceTypeResidualCurrentCircuitBreaker",
            "Pset_ProtectiveDeviceTypeResidualCurrentSwitch",
            "Pset_ProtectiveDeviceTypeVaristor",
            "Pset_PumpOccurrence",
            "Pset_PumpPHistory",
            "Pset_PumpTypeCommon",
            "Pset_RailingCommon",
            "Pset_RampCommon",
            "Pset_RampFlightCommon",
            "Pset_ReinforcementBarCountOfIndependentFooting",
            "Pset_ReinforcementBarPitchOfBeam",
            "Pset_ReinforcementBarPitchOfColumn",
            "Pset_ReinforcementBarPitchOfContinuousFooting",
            "Pset_ReinforcementBarPitchOfSlab",
            "Pset_ReinforcementBarPitchOfWall",
            "Pset_Risk",
            "Pset_RoofCommon",
            "Pset_SanitaryTerminalTypeBath",
            "Pset_SanitaryTerminalTypeBidet",
            "Pset_SanitaryTerminalTypeCistern",
            "Pset_SanitaryTerminalTypeCommon",
            "Pset_SanitaryTerminalTypeSanitaryFountain",
            "Pset_SanitaryTerminalTypeShower",
            "Pset_SanitaryTerminalTypeSink",
            "Pset_SanitaryTerminalTypeToiletPan",
            "Pset_SanitaryTerminalTypeUrinal",
            "Pset_SanitaryTerminalTypeWashHandBasin",
            "Pset_SensorPHistory",
            "Pset_SensorTypeCommon",
            "Pset_SensorTypeConductanceSensor",
            "Pset_SensorTypeContactSensor",
            "Pset_SensorTypeFireSensor",
            "Pset_SensorTypeFlowSensor",
            "Pset_SensorTypeGasSensor",
            "Pset_SensorTypeHeatSensor",
            "Pset_SensorTypeHumiditySensor",
            "Pset_SensorTypeIonConcentrationSensor",
            "Pset_SensorTypeLevelSensor",
            "Pset_SensorTypeLightSensor",
            "Pset_SensorTypeMoistureSensor",
            "Pset_SensorTypeMovementSensor",
            "Pset_SensorTypePHSensor",
            "Pset_SensorTypePressureSensor",
            "Pset_SensorTypeRadiationSensor",
            "Pset_SensorTypeRadioactivitySensor",
            "Pset_SensorTypeSmokeSensor",
            "Pset_SensorTypeSoundSensor",
            "Pset_SensorTypeTemperatureSensor",
            "Pset_SensorTypeWindSensor",
            "Pset_ServiceLife",
            "Pset_ServiceLifeFactors",
            "Pset_ShadingDeviceCommon",
            "Pset_ShadingDevicePHistory",
            "Pset_SiteCommon",
            "Pset_SlabCommon",
            "Pset_SolarDeviceTypeCommon",
            "Pset_SoundAttenuation",
            "Pset_SoundGeneration",
            "Pset_SpaceCommon",
            "Pset_SpaceCoveringRequirements",
            "Pset_SpaceFireSafetyRequirements",
            "Pset_SpaceHeaterPHistory",
            "Pset_SpaceHeaterTypeCommon",
            "Pset_SpaceHeaterTypeConvector",
            "Pset_SpaceHeaterTypeRadiator",
            "Pset_SpaceLightingRequirements",
            "Pset_SpaceOccupancyRequirements",
            "Pset_SpaceParking",
            "Pset_SpaceThermalDesign",
            "Pset_SpaceThermalLoad",
            "Pset_SpaceThermalLoadPHistory",
            "Pset_SpaceThermalPHistory",
            "Pset_SpaceThermalRequirements",
            "Pset_StackTerminalTypeCommon",
            "Pset_StairCommon",
            "Pset_StairFlightCommon",
            "Pset_StructuralSurfaceMemberVaryingThickness",
            "Pset_SwitchingDeviceTypeCommon",
            "Pset_SwitchingDeviceTypeContactor",
            "Pset_SwitchingDeviceTypeDimmerSwitch",
            "Pset_SwitchingDeviceTypeEmergencyStop",
            "Pset_SwitchingDeviceTypeKeypad",
            "Pset_SwitchingDeviceTypeMomentarySwitch",
            "Pset_SwitchingDeviceTypePHistory",
            "Pset_SwitchingDeviceTypeSelectorSwitch",
            "Pset_SwitchingDeviceTypeStarter",
            "Pset_SwitchingDeviceTypeSwitchDisconnector",
            "Pset_SwitchingDeviceTypeToggleSwitch",
            "Pset_SystemFurnitureElementTypeCommon",
            "Pset_SystemFurnitureElementTypePanel",
            "Pset_SystemFurnitureElementTypeWorkSurface",
            "Pset_TankOccurrence",
            "Pset_TankTypeCommon",
            "Pset_TankTypeExpansion",
            "Pset_TankTypePreformed",
            "Pset_TankTypePressureVessel",
            "Pset_TankTypeSectional",
            "Pset_ThermalLoadAggregate",
            "Pset_ThermalLoadDesignCriteria",
            "Pset_TransformerTypeCommon",
            "Pset_TransportElementCommon",
            "Pset_TransportElementElevator",
            "Pset_TubeBundleTypeCommon",
            "Pset_TubeBundleTypeFinned",
            "Pset_UnitaryControlElementPHistory",
            "Pset_UnitaryControlElementTypeCommon",
            "Pset_UnitaryControlElementTypeIndicatorPanel",
            "Pset_UnitaryControlElementTypeThermostat",
            "Pset_UnitaryEquipmentTypeAirConditioningUnit",
            "Pset_UnitaryEquipmentTypeAirHandler",
            "Pset_UnitaryEquipmentTypeCommon",
            "Pset_UtilityConsumptionPHistory",
            "Pset_ValvePHistory",
            "Pset_ValveTypeAirRelease",
            "Pset_ValveTypeCommon",
            "Pset_ValveTypeDrawOffCock",
            "Pset_ValveTypeFaucet",
            "Pset_ValveTypeFlushing",
            "Pset_ValveTypeGasTap",
            "Pset_ValveTypeIsolating",
            "Pset_ValveTypeMixing",
            "Pset_ValveTypePressureReducing",
            "Pset_ValveTypePressureRelief",
            "Pset_VibrationIsolatorTypeCommon",
            "Pset_WallCommon",
            "Pset_Warranty",
            "Pset_WasteTerminalTypeCommon",
            "Pset_WasteTerminalTypeFloorTrap",
            "Pset_WasteTerminalTypeFloorWaste",
            "Pset_WasteTerminalTypeGullySump",
            "Pset_WasteTerminalTypeGullyTrap",
            "Pset_WasteTerminalTypeRoofDrain",
            "Pset_WasteTerminalTypeWasteDisposalUnit",
            "Pset_WasteTerminalTypeWasteTrap",
            "Pset_WindowCommon",
            "Pset_WorkControlCommon",
            "Pset_ZoneCommon"
        };

        private List<string> DefinitionsIFC4_QTO = new List<string>()
        {   
            "Qto_ActuatorBaseQuantities",
            "Qto_AirTerminalBaseQuantities",
            "Qto_AirTerminalBoxTypeBaseQuantities",
            "Qto_AirToAirHeatRecoveryBaseQuantities",
            "Qto_AlarmBaseQuantities",
            "Qto_AudioVisualApplianceBaseQuantities",
            "Qto_BeamBaseQuantities",
            "Qto_BoilerBaseQuantities",
            "Qto_BuildingBaseQuantities",
            "Qto_BuildingStoreyBaseQuantities",
            "Qto_BurnerBaseQuantities",
            "Qto_CableCarrierFittingBaseQuantities",
            "Qto_CableCarrierSegmentBaseQuantities",
            "Qto_CableFittingBaseQuantities",
            "Qto_CableSegmentBaseQuantities",
            "Qto_ChillerBaseQuantities",
            "Qto_ChimneyBaseQuantities",
            "Qto_CoilBaseQuantities",
            "Qto_ColumnBaseQuantities",
            "Qto_CommunicationsApplianceBaseQuantities",
            "Qto_CompressorBaseQuantities",
            "Qto_CondenserBaseQuantities",
            "Qto_ConstructionEquipmentResourceBaseQuantities",
            "Qto_ConstructionMaterialResourceBaseQuantities",
            "Qto_ControllerBaseQuantities",
            "Qto_CooledBeamBaseQuantities",
            "Qto_CoolingTowerBaseQuantities",
            "Qto_CoveringBaseQuantities",
            "Qto_CurtainWallQuantities",
            "Qto_DamperBaseQuantities",
            "Qto_DistributionChamberElementBaseQuantities",
            "Qto_DoorBaseQuantities",
            "Qto_DuctFittingBaseQuantities",
            "Qto_DuctSegmentBaseQuantities",
            "Qto_DuctSilencerBaseQuantities",
            "Qto_ElectricApplianceBaseQuantities",
            "Qto_ElectricDistributionBoardBaseQuantities",
            "Qto_ElectricFlowStorageDeviceBaseQuantities",
            "Qto_ElectricGeneratorBaseQuantities",
            "Qto_ElectricMotorBaseQuantities",
            "Qto_ElectricTimeControlBaseQuantities",
            "Qto_EvaporativeCoolerBaseQuantities",
            "Qto_EvaporatorBaseQuantities",
            "Qto_FanBaseQuantities",
            "Qto_FilterBaseQuantities",
            "Qto_FireSuppressionTerminalBaseQuantities",
            "Qto_FlowInstrumentBaseQuantities",
            "Qto_FlowMeterBaseQuantities",
            "Qto_FootingBaseQuantities",
            "Qto_HeatExchangerBaseQuantities",
            "Qto_HumidifierBaseQuantities",
            "Qto_InterceptorBaseQuantities",
            "Qto_JunctionBoxBaseQuantities",
            "Qto_LaborResourceBaseQuantities",
            "Qto_LampBaseQuantities",
            "Qto_LightFixtureBaseQuantities",
            "Qto_MemberBaseQuantities",
            "Qto_MotorConnectionBaseQuantities",
            "Qto_OpeningElementBaseQuantities",
            "Qto_OutletBaseQuantities",
            "Qto_PileBaseQuantities",
            "Qto_PipeFittingBaseQuantities",
            "Qto_PipeSegmentBaseQuantities",
            "Qto_PlateBaseQuantities",
            "Qto_ProjectionElementBaseQuantities",
            "Qto_ProtectiveDeviceBaseQuantities",
            "Qto_ProtectiveDeviceTrippingUnitBaseQuantities",
            "Qto_PumpBaseQuantities",
            "Qto_RailingBaseQuantities",
            "Qto_RampFlightBaseQuantities",
            "Qto_ReinforcingElementBaseQuantities",
            "Qto_RoofBaseQuantities",
            "Qto_SanitaryTerminalBaseQuantities",
            "Qto_SensorBaseQuantities",
            "Qto_SiteBaseQuantities",
            "Qto_SlabBaseQuantities",
            "Qto_SolarDeviceBaseQuantities",
            "Qto_SpaceBaseQuantities",
            "Qto_SpaceHeaterBaseQuantities",
            "Qto_StackTerminalBaseQuantities",
            "Qto_StairFlightBaseQuantities",
            "Qto_SwitchingDeviceBaseQuantities",
            "Qto_TankBaseQuantities",
            "Qto_TransformerBaseQuantities",
            "Qto_TubeBundleBaseQuantities",
            "Qto_UnitaryControlElementBaseQuantities",
            "Qto_UnitaryEquipmentBaseQuantities",
            "Qto_ValveBaseQuantities",
            "Qto_VibrationIsolatorBaseQuantities",
            "Qto_WallBaseQuantities",
            "Qto_WasteTerminalBaseQuantities",
            "Qto_WindowBaseQuantities"
        };
        #endregion
    }

    public enum Version
    {
        IFC2x3,
        IFC4
    }
}
