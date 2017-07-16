using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IEquipment
    {
        //void insertEquipment(Model.Equipment equipment);
        Model.Equipment getEquipmentByMacId(string macId);
        bool isExist(string macId);
    }
}
