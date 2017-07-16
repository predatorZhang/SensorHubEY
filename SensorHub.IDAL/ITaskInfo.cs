using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface ITaskInfo
    {
        Model.TaskInfo getTaskInfo(string username,string state);
        Model.TaskInfo getTaskInfo(long dbid);
        List<Model.TaskInfo> getTaskInfoByNameAndTwoStates(String name, String state1, String state2);
        void setTaskInfo(Model.TaskInfo taskInfo);
    }
}
