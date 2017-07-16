using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class CasicFireEvent
    {

        public Model.CasicFireEvent isFireUp(String cmd)
        {
            ICasicFireEvent dal = SensorHub.DALFactory.FireCasicEvent.Create();
            return dal.isFireUp(cmd);
        }
        public void fireOff(String cmd)
        {
            ICasicFireEvent dal = SensorHub.DALFactory.FireCasicEvent.Create();
            dal.fireOff(cmd);
        }
    }
}
