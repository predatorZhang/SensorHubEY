using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class SewPeriodData
    {
        /// <summary>
        /// A method to insert a new Adapter
        /// </summary>
        /// <param name="rqs">An adapter entity with information about the new adapter</param>
        public void insert(Model.SewPeriodDataInfo sew)
        {
            IDAL.ISewPeriodData dal = SensorHub.DALFactory.SewPeriodData.Create();
            dal.insert(sew);

        }
    }
}
