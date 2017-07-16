using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IPatroler
    {
        Model.Patroler getPatrolerByName(string name);
        void setPatrolerAccountStateByName(Model.Patroler patroler);
        bool isExist(string name);
    }
}
