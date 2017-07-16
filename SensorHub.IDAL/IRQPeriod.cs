using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.IDAL
{
    public interface IRQPeriod
    {
        void insert(List<Model.RQPeriodInfo> rqs);
    }
}
