using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;

namespace SensorHub.Servers.Commands.EMSCommands
{
    [EmsCommandFilter]
    public class EmsCommand : CommandBase<EmsSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "HeartBeat";
            }
        }


        public override void ExecuteCommand(EmsSession session, StringRequestInfo requestInfo)
        {
            session.LastLoginTime = System.DateTime.Now;
            session.Logger.Info("##########################开始执行Command##########################");
            try
            {
                session.LastLoginTime = System.DateTime.Now;
                byte[] data = new byte[128];
                string[] dataList = requestInfo.Body.Split('$');
                string patrolerName = dataList[0];
                string equip = dataList[1];
                double x = Convert.ToDouble(dataList[2]);
                double y = Convert.ToDouble(dataList[3]);

                if ("OFFLINE" == session.Patroler.AccountState)
                {
                    //不在线
                    //修改巡检员在线状态
                    session.Patroler.AccountState = "ONLINE";
                    new BLL.Patroler().setPatrolerAccountStateByName(session.Patroler);
                    session.Logger.Info("离线巡检人" + session.Patroler.UserName + "状态更新为ONLINE");

                    //查询是否有任务
                    session.Taskinfo = new BLL.TaskInfo().getTaskInfo(session.Patroler.UserName, "下发任务");

                    if (null == session.Taskinfo)
                    {
                        data = Encoding.UTF8.GetBytes("none" + "\r\n");
                        session.Send(data, 0, data.Length);
                        session.Logger.Info("离线巡检人" + session.Patroler.UserName + "没有获取到下发任务");
                    }
                    else
                    {
                        //发送任务工单号！
                        data = Encoding.UTF8.GetBytes(session.Taskinfo.DbId + "\r\n");
                        session.Send(data, 0, data.Length);
                        session.Logger.Info("离线巡检人" + session.Patroler.UserName + "获取到下发任务");
                    }

                    Model.LogInfo logInfoModel = new Model.LogInfo();
                    logInfoModel.Equipment = session.Equipment.MacId;
                    logInfoModel.Operate = "登录";
                    logInfoModel.Operate_time = DateTime.Now;
                    logInfoModel.Username = session.Patroler.UserName;
                    logInfoModel.Patroler_id = session.Patroler.DbId;
                    new BLL.LogInfo().insertLogInfo(logInfoModel);

                    session.Logger.Info("保存离线巡检人" + session.Patroler.UserName + "操作日志");
                }
                else
                {
                    //在线人员定位信息
                    Model.Position position = new Model.Position();
                    position.Longitude = x;
                    position.Latitude = y;
                    position.Username = session.Patroler.UserName;
                    position.Patroler_id = session.Patroler.DbId;
                    position.ReceiveTime = DateTime.Now;

                    if (null != session.Taskinfo)
                    {
                        //插入带下发任务编号的人员定位信息
                        position.Task_id = session.Taskinfo.DbId;
                        new BLL.Position().insertPosition(position);                        
                        data = Encoding.UTF8.GetBytes(session.Taskinfo.DbId + "\r\n");
                        session.Send(data, 0, data.Length);
                        session.Logger.Info("保存在线巡检人" + session.Patroler.UserName + "任务位置信息！");
                    }
                    else
                    {//无下发任务编号 插入人员定位信息
                        new BLL.Position().insertPosition(position);
                        data = Encoding.UTF8.GetBytes("none" + "\r\n");
                        session.Send(data, 0, data.Length);
                        session.Logger.Info("保存在线巡检人" + session.Patroler.UserName + "位置信息！");
                    }
                }
                session.Logger.Info("##########################Command执行成功##########################");
                session.Logger.Info("\n");
                session.Logger.Info("\n");
                session.Logger.Info("\n");
            }
            catch (Exception e)
            {
                session.Logger.Error("##########################Command执行错误##########################");
                session.Logger.Error(e.ToString());
                session.Logger.Error("\n");
                session.Logger.Error("\n");
                session.Logger.Error("\n");
            }
        }
    }
}
