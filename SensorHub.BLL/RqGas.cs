using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class RqGas
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="gas">An adapter entity with information about the new adapter</param>
        public void insert(RqGasInfo gas)
        {
            // Validate input
            if (gas.DBID <= 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(gas.DEVID))
            {
                return;
            }
            IRqGas dal = SensorHub.DALFactory.RqGas.Create();
            dal.insert(gas);

        }
    }
}
