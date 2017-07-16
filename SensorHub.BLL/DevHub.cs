using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;
namespace SensorHub.BLL
{
    public class DevHub
    {
      
        public List<Model.DevHubInfo> findAll()
        {
            IDevHub idevHub = SensorHub.DALFactory.DevHub.Create();
            return idevHub.getAll();
        }

        public List<Model.DevHubInfo> findAllRealTimeDT()
        {
            IDevHub idevHub = SensorHub.DALFactory.DevHub.Create();
            return idevHub.getAllRealTimeDT();
        }
        public void setRealSerachDevStatus(String devCode)
        {
            IDevHub idevHub = SensorHub.DALFactory.DevHub.Create();
            idevHub.changeRealSerachDevStatus(devCode);
        }

        public void setOnLineByDevcode(String devCode)
        {
            IDevHub idevHub = SensorHub.DALFactory.DevHub.Create();
            idevHub.changeOnLineStatus(devCode, true);
        }

        public void setOffLine()
        {
            IDevHub idevHub = SensorHub.DALFactory.DevHub.Create();
            idevHub.setOffLine();
        }
    }
}
