using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.BLL
{
   public class Patroler
    {
       public Model.Patroler getPatrolerByName(string name)
       {
           if (string.IsNullOrEmpty(name))
           {
               return null;
           }
           return DALFactory.Patroler.Create().getPatrolerByName(name);
       }

       public void setPatrolerAccountStateByName(Model.Patroler patroler)
       {
           try
           {
               if (null == patroler)
               {
                   return;
               }
               IDAL.IPatroler dal = DALFactory.Patroler.Create();
               dal.setPatrolerAccountStateByName(patroler);
           }
           catch (Exception e)
           {
               throw e;
           }
       }

       public bool isExist(string username)
       {
           if (string.IsNullOrEmpty(username))
           {
               return false;
           }
           return DALFactory.Patroler.Create().isExist(username);
       }
    }
}
