using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbim.Properties
{
    public class ApplicableClass
    {
        public string ClassName { get; set; }
        public string PredefinedType { get; set; }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(PredefinedType))
                return ClassName;
            else
                return String.Format("{0}/{1}", ClassName, PredefinedType);
        }
    }
}
