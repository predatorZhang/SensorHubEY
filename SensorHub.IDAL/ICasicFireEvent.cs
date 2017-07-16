using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;

namespace SensorHub.IDAL
{
    public interface ICasicFireEvent
    {
        Model.CasicFireEvent isFireUp(String cmd);

        void fireOff(String cmd);
    }
}
