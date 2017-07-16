using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class Position
    {
       public void insertPosition(Model.Position position)
       {
           try
           {
               if (null == position)
               {
                   return;
               }
               IDAL.IPosition dal = DALFactory.Position.Create();
               dal.insertPosition(position);
           }
           catch (Exception e)
           {
               throw e;
           }
       }
    }
}
