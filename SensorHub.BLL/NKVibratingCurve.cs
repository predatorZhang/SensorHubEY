using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class NKVibratingCurve
    {
                /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="VibratingCurve">An adapter entity with information about the new adapter</param>
        public void insert(SensorHub.Model.NKVibratingCurveInfo VibratingCurve)
        {
            if (string.IsNullOrEmpty(VibratingCurve.DEVID))
            {
                return;
            }
            SensorHub.IDAL.INKVibratingCurve dal = SensorHub.DALFactory.NKVibratingCurve.Create();
            dal.insert(VibratingCurve);
        }
    }
}
