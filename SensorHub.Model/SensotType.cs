using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Model
{
    public class SensorType
    {
       
        private string typeCode;
        private string typeName;
      
        public string TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

    }
}
