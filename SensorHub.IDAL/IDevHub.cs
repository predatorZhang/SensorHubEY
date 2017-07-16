using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using System.Collections;

namespace SensorHub.IDAL
{
    public interface IDevHub
    {
        List<DevHubInfo> getAll();
        List<DevHubInfo> getAllRealTimeDT();
        void changeRealSerachDevStatus(String devCode);
        void changeOnLineStatus(String devCode, bool isOnline);
        void setOffLine();
    }
}
