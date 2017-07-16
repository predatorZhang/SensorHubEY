using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
namespace SensorHub.Servers
{
    class CasicWakeUpJob:IJob
    {
        
        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute(IJobExecutionContext context)
        {
            JobDataMap map = context.JobDetail.JobDataMap;
            CasicServer server = (CasicServer)map.Get("server");
            Dictionary<String, String> devHubMaps = new Dictionary<String, String>();
            List<Model.DevHubInfo> devHubs = new BLL.DevHub().findAll();
            foreach (Model.DevHubInfo devHub in devHubs)
            {
                if (!devHub.DevCode.StartsWith("21"))
                {
                    devHubMaps.Add(devHub.DevCode, devHub.HubCode);
                }
            }
            CasicSender casicSender = new CasicSender(server);

            new BLL.DevHub().setOffLine();
            Model.CasicFireEvent fireEvent = new Model.CasicFireEvent();
            fireEvent.Version = "20";
            casicSender.sendWakeUpCmd(devHubMaps, fireEvent);

        }
    }
}
