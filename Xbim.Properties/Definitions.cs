using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                    if (typeof (T) == typeof (PropertySetDef))
                        ns = "http://buildingSMART-tech.org/xml/psd/PSD_IFC4.xsd";
                    if (typeof (T) == typeof (QtoSetDef))
                        ns = "http://www.buildingsmart-tech.org/xml/qto/QTO_IFC4.xsd";
                    _serializer = new XmlSerializer(typeof (T), ns);
                    break;
                case Version.IFC2x3:
                    _serializer = new XmlSerializer(typeof (T));
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
            get { return _definitions; }
        }

        public T this[string name]
        {
            get { return _definitions.FirstOrDefault(d => d.Name == name); }
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
            var data = (T) _serializer.Deserialize(stream);
            if (data != null)
            {
                this.Add(data);
            }
        }

        public void Load(TextReader reader)
        {
            var data = (T) _serializer.Deserialize(reader);
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

        public IEnumerable<TP> GetPropertiesWhere<TP>(Func<TP, bool> predicate, bool nested = true)
            where TP : QuantityPropertyDef
        {
            foreach (var set in this._definitions)
            {
                foreach (var qpDef in set.Definitions)
                {
					if (!(qpDef is TP def))
						continue;

					//check predicate
					if (predicate(def))
                        yield return def;

                    if (!nested)
                        continue;

					if (!(qpDef is PropertyDef pDef))
						continue;

					//process nested properties in complex properties
					if (!(pDef.PropertyType.PropertyValueType is TypeComplexProperty cType)) 
                        continue;
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
            if (_version == Version.IFC2x3)
            {
                if (typeof (T) == typeof (QtoSetDef))
                    throw new Exception("No quantity sets are defined for IFC 2x3 in definition files.");
                mgr = Definitions.IFC2x3_Defitinion_files.ResourceManager;
            }
            if (_version == Version.IFC4)
            {
                mgr = typeof (T) == typeof (QtoSetDef)
                    ? Definitions.IFC4_QTO_Definition_files.ResourceManager
                    : Definitions.IFC4_Definition_files.ResourceManager;
            }

            if (mgr == null)
                throw new Exception("No default content defined for this combination of version and property type");

            var resources = mgr.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            foreach (var value in from DictionaryEntry entry in resources select entry.Value as string)
            {
                if (value == null) throw new Exception("Invalid input data");
                var reader = new StringReader(value);
                Load(reader);
            }
        }

        public void LoadIFC4COBie()
        {
            if (_version != Version.IFC4)
                throw new Exception("IFC4 COBie properties can only be loaded if this set is defined to be IFC4.");
            var resources =
                Definitions.IFC4_COBie_Definition_files.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture,
                    true, true);
            foreach (var value in from DictionaryEntry entry in resources select entry.Value as string)
            {
                if (value == null) throw new Exception("Invalid input data");
                var reader = new StringReader(value);
                Load(reader);
            }
        }

        public void LoadIFC4AndCOBie()
        {
            if (_version != Version.IFC4)
                throw new Exception("IFC4 COBie properties can only be loaded if this set is defined to be IFC4.");
            var resources =
                Definitions.IFC4_and_COBie_Definition_files.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture,
                    true, true);
            foreach (var value in from DictionaryEntry entry in resources select entry.Value as string)
            {
                if (value == null) throw new Exception("Invalid input data");
                var reader = new StringReader(value);
                Load(reader);
            }
        }
    }

    public enum Version
    {
        IFC2x3,
        IFC4
    }
}