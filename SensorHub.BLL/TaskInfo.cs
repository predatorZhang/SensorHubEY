using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class TaskInfo
    {
       public Model.TaskInfo getTaskInfo(string username,string state)
       {
           if ( String.IsNullOrEmpty(username) || String.IsNullOrEmpty(state) )
           {
               return null;
           }
           return DALFactory.TaskInfo.Create().getTaskInfo(username,state);
       }

       public Model.TaskInfo getTaskInfo(long dbid)
       {
           return DALFactory.TaskInfo.Create().getTaskInfo(dbid);
       }

       public List<Model.TaskInfo> getTaskInfoByNameAndTwoStates(String name, String state1, String state2)
       {
           return DALFactory.TaskInfo.Create().getTaskInfoByNameAndTwoStates(name, state1, state2);
       }

       public void setTaskInfo(Model.TaskInfo taskInfo)
       {
           try
           {
               if (null == taskInfo)
               {
                   return;
               }
               IDAL.ITaskInfo dal = DALFactory.TaskInfo.Create();
               dal.setTaskInfo(taskInfo);
           }
           catch (Exception e)
           {
               throw e;
           }
       }
    }
}
