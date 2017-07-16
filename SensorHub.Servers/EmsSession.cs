using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;

namespace SensorHub.Servers
{
    public class EmsSession : AppSession<EmsSession, StringRequestInfo>
    {
        private System.DateTime lastLoginTime;
        private string taskId;
        private string equip;
        private string patrolerName;
        private long patrolerId;
        private bool initial = false;
        private Model.Patroler patroler;
        private Model.Equipment equipment;
        private Model.TaskInfo taskinfo;

        public System.DateTime LastLoginTime
        {
            get { return lastLoginTime; }
            set { lastLoginTime = value; }
        }

        public string EquIp
        {
            get { return equip; }
            set { equip = value; }
        }

        public string TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }

        public string PatrolerName
        {
            get { return patrolerName; }
            set { patrolerName = value; }
        }

        public long PatrolerId
        {
            get { return patrolerId; }
            set { patrolerId = value; }
        }

        public bool Initial
        {
            get { return initial; }
            set { initial = value; }
        }

        public Model.Patroler Patroler
        {
            get { return patroler; }
            set { patroler = value; }
        }

        public Model.Equipment Equipment
        {
            get { return equipment; }
            set { equipment = value; }
        }

        public Model.TaskInfo Taskinfo
        {
            get { return taskinfo; }
            set { taskinfo = value; }
        }



        protected override void OnSessionStarted()
        {

        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            try
            {
                Logger.Info("########################SessionClosed开始执行###############################");
                if (null != this.equipment)
                {
                    //设备离线时更新人员状态
                    this.patroler.AccountState = "OFFLINE";
                    new BLL.Patroler().setPatrolerAccountStateByName(this.patroler);
                    Logger.Info("将巡检人" + this.patroler.UserName + "状态更新为OFFLINE");

                    //填写离线日志
                    Model.LogInfo logInfoModel = new Model.LogInfo();
                    logInfoModel.Equipment = this.equipment.MacId;
                    logInfoModel.Operate = "离线";
                    logInfoModel.Operate_time = DateTime.Now;
                    logInfoModel.Username = this.patroler.UserName;
                    logInfoModel.Patroler_id = this.patroler.DbId;
                    new BLL.LogInfo().insertLogInfo(logInfoModel);
                    Logger.Info("保存巡检人" + this.patroler.UserName + "操作日志");

                    //修改任务状态
                    Model.TaskInfo task = new BLL.TaskInfo().getTaskInfo(this.taskinfo.DbId);
                    if ("接受任务".Equals(task.TaskState) || "开始任务".Equals(task.TaskState))
                    {
                        task.TaskState = "执行过程中";
                        new BLL.TaskInfo().setTaskInfo(task);
                        Logger.Info("修改巡检人" + this.patroler.UserName + "当前任务状态为'执行过程中'");
                    }
                    else
                    {
                        Logger.Info("巡检人" + this.patroler.UserName + "已经完成任务");
                    }

                }
                Logger.Info("#################SessionClosed关闭成功####################");
                Logger.Info("\n");
                Logger.Info("\n");
                Logger.Info("\n");
            }
            catch (Exception e)
            {
                Logger.Error("#################SessionClosed关闭失败####################");
                Logger.Error(e.ToString());
                Logger.Error("\n");
                Logger.Error("\n");
                Logger.Error("\n");
            }

        }
    }
}
