using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    [XmlInclude(typeof(PropertySetDef))]
    [XmlInclude(typeof(QtoSetDef))]
    public abstract class QuantityPropertySetDef
    {
        /// <summary>
        /// Version information of IFC release and sub schema.
        /// </summary>
        public IfcVersion IfcVersion { get; set; }

        /// <summary>
        /// The name of property set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The definition of property set  from buildingSMART.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// This property needs to be overwritten in inhereted classes 
        /// with corret attributes sut up for serialization
        /// </summary>
        [XmlIgnore]
        public List<NameAlias> DefinitionAliases { get; set; }

        /// <summary>
        /// Applicable entity types.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<ApplicableClass> ApplicableClasses
        {
            get
            {
                var res = new ApplicableClass();
                foreach (var ac in _applicableClasses)
                {
                    if (String.IsNullOrEmpty(ac))
                        continue;

                    var fields = ac.Trim().Split('/');
                    if (fields.Length == 1)
                    {
                        res.ClassName = fields[0].Trim();
                        yield return res;
                        continue;
                    }

                    //IFC4 variant
                    if (fields[0].Contains("Ifc"))
                    {
                        res.ClassName = fields[0].Trim();
                        res.PredefinedType = fields[1].Trim();
                        yield return res;
                        continue;
                    }
                    //IFC2x3 variant where fields[0] is the name of the schema domain
                    else
                    {
                        res.ClassName = fields[1].Trim();
                        yield return res;
                        continue;
                    }
                }
            }
        }

        public void AddApplicableClass(ApplicableClass cls)
        {
            _applicableClasses.Add(cls.ToString());
        }

        /// <summary>
        /// The container element of applicable entity types. This is only to be used by
        /// serializer. Use "ApplicableClasses" instead.
        /// </summary>
        [XmlArray("ApplicableClasses")]
        [XmlArrayItem("ClassName")]
        public List<string> _applicableClasses { get; set; }

        [XmlIgnore]
        public QuantityPropertyDef this[string name]
        {
            get
            {
                return Definitions.First(d => d.Name == name);
            }
        }

        [XmlIgnore]
        public abstract IEnumerable<QuantityPropertyDef> Definitions { get; }
    }
}
