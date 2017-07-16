using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Model;
using SensorHub.IDAL;

namespace SensorHub.BLL
{
    public class YLiang
    {
        public void insert(Model.YLInfo djs)
        {
            if (djs == null)
            {
                return;
            }
            IYLiang dal = SensorHub.DALFactory.YLiang.Create();
            dal.insert(djs);
        }
    }
}


/*
      public void insert(List<Model.YLInfo> djs)
      {

          if (djs.Count <= 0)
          {
              return;
          }

          IYLiang dal = SensorHub.DALFactory.YLiang.Create();
          List<Model.YLInfo> list = new List<YLInfo>();

          foreach (Model.YLInfo dj in djs)
          {
              if (dal.queryCountByDevAndUpTime(dj.DevId, dj.UpTime) <= 0)
              {
                  list.Add(dj);
              }
          }
          if (list.Count > 0)
          {
              dal.insert(djs);
          }
      }
       * */