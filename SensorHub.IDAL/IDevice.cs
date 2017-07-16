using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IDevice
    {
        object getDevIdByCode(string code);

        String getDevTypeByCode(String devCode);

        string getDevTypeByDevId(int devId);

        List<String> getAllDevCode();

        List<Model.SensorType> getSensorTypeByDevId(int devId);

        float getWellDepthByDevCode(String devCode);
    }
}
