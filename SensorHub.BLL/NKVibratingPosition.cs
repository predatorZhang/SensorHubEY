using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class NKVibratingPosition
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="vibratingPosition">An adapter entity with information about the new adapter</param>
        public void insert(SensorHub.Model.NKVibratingPositionInfo vibratingPosition)
        {
            if (string.IsNullOrEmpty(vibratingPosition.DEVID))
            {
                return;
            }
            SensorHub.IDAL.INKVibratingPosition dal = SensorHub.DALFactory.NKVibratingPosition.Create();
            dal.insert(vibratingPosition);
        }
    }
}
