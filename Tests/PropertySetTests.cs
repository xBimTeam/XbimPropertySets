using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Properties.Dictionary;

namespace Xbim.Properties.Tests
{
    [TestClass]
    public class PropertySetTests
    {
        [TestMethod]
        public void UserDefinedPropertySets()
        {
            var pSetsDef = new Definitions<PropertySetDef>(Version.IFC4);
            pSetsDef.Add(new[]
            {
                new PropertySetDef()
                {
                    Name = "Sample property set",
                    Definition = "Definition of sample property set.",
                    IfcVersion = new IfcVersion() {schema = "IfcSharedBldgElements", version = "IFC2x3"},
                    _applicableClasses = new List<string>() {"IfcWall", "IfcDoor", "IfcWindow"},
                    PropertyDefinitions = new List<PropertyDef>()
                    {
                        new PropertyDef()
                        {
                            Name = "Sample property",
                            Definition = "Sample property description",
                            PropertyType = new PropertyType()
                            {
                                PropertyValueType = new TypePropertySingleValue()
                                {
                                    DataType = new DataType() {Type = DataTypeEnum.IfcBoolean},
                                    UnitType = new UnitType() {Type = UnitTypeEnum.AREAUNIT}
                                }
                            }
                        },
                        new PropertyDef()
                        {
                            Name = "Sample property II",
                            Definition = "Sample property II description",
                            PropertyType = new PropertyType()
                            {
                                PropertyValueType = new TypePropertyBoundedValue()
                                {
                                    DataType = new DataType() {Type = DataTypeEnum.IfcBoolean},
                                    ValueRangeDef = new ValueRangeDef()
                                    {
                                        LowerBoundValue = new LowerBoundValue() {Value = "456"},
                                        UpperBoundValue = new UpperBoundValue() {Value = "4458"}
                                    }
                                }
                            }
                        },
                        new PropertyDef()
                        {
                            Name = "Enumeration",
                            Definition = "Definition of enumeration property",
                            PropertyType = new PropertyType()
                            {
                                PropertyValueType = new TypePropertyEnumeratedValue()
                                {
                                    EnumList = new EnumList()
                                    {
                                        Name = "Amazing name",
                                        Items = new List<string>() {"A", "B", "C"}
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var prop = pSetsDef.GetPropertiesWhere<PropertyDef>(d => d.Name == "Enumeration").FirstOrDefault();
            (prop.PropertyType.PropertyValueType as TypePropertyEnumeratedValue).EnumList.Name = "Amazing name";


            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            pSetsDef.SaveToDirectory(directory);

            var defs2 = new Definitions<PropertySetDef>(Version.IFC4);
            defs2.LoadFromDirectory(directory);

            Assert.IsNotNull(defs2);
            Assert.IsTrue(defs2.GetAllProperties<PropertyDef>(true).Count() == pSetsDef.GetAllProperties<PropertyDef>(true).Count());
        }

        [TestMethod]
        public void ApplicableClassesTest()
        {

            var defs2x3 = new Definitions<PropertySetDef>(Version.IFC2x3);
            defs2x3.LoadAllDefault();
            var classes2x3 = new List<string>();
                foreach (var item in defs2x3.DefinitionSets)
                    foreach (var clsName in item.ApplicableClasses)
                        classes2x3.Add(clsName.ClassName);
                Assert.IsTrue(classes2x3.Any());

            var defs4 = new Definitions<PropertySetDef>(Version.IFC4);
            defs4.LoadAllDefault();
            var classes4 = new List<string>();
                foreach (var item in defs4.DefinitionSets)
                    foreach (var clsName in item.ApplicableClasses)
                        classes4.Add(clsName.ClassName);
                Assert.IsTrue(classes4.Any());
        }

        [TestMethod]
        public void DictionaryAccess()
        {
            var connection = new DictionaryConnection();
            var concepts = connection.SearchConcept("Pset_ActionRequest.RequestSourceName");
            Assert.IsNotNull(concepts);
            Assert.IsNotNull(concepts.Concepts);
            Assert.IsTrue(concepts.Concepts.Any());
        }

        [TestMethod]
        public void IndexAccessOperators()
        {
            var defs = new Definitions<PropertySetDef>(Version.IFC4);
            defs.LoadAllDefault();
            var prop = defs["Pset_ActionRequest"]["RequestSourceName"] as PropertyDef;
            Assert.IsNotNull(prop);
        }
    }
}