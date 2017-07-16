using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface INKTemperatureCurve
    {
        void insert(NKTemperatureCurveInfo temperatureCurve);
        Model.NKTemperatureCurveInfo getLastCurve(string devCode);
    }
}
