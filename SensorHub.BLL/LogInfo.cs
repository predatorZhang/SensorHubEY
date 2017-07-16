using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class LogInfo
    {
       public void insertLogInfo(Model.LogInfo logInfo)
       {
           try
           {
               if (null == logInfo)
               {
                   return;
               }
               IDAL.ILogInfo dal = DALFactory.LogInfo.Create();
               dal.insertLogInfo(logInfo);
           }
           catch (Exception e)
           {
               throw e;
           }
       }
    }
}
