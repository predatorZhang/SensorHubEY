using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
    public class Equipment
    {
        public Model.Equipment getEquipmentByMacId(string macId)
        {
            if (string.IsNullOrEmpty(macId))
            {
                return null;
            }
            return DALFactory.Equipment.Create().getEquipmentByMacId(macId);
        }

        public bool isExist(string macid)
        {
            if (string.IsNullOrEmpty(macid))
            {
                return false;
            }
            return DALFactory.Equipment.Create().isExist(macid);
        }

    }
}
