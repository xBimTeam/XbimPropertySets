using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xbim.Properties
{
    public class QtoDef : QuantityPropertyDef
    {
        [XmlElement("QtoType")]
        public QtoTypeEnum QuantityType { get; set; }
    }

    public enum QtoTypeEnum
    {
        Q_LENGTH,
        Q_AREA,
        Q_VOLUME,
        Q_COUNT,
        Q_TIME,
        Q_WEIGHT
    }
}
