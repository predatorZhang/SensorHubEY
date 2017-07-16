using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Utility;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Servers.Commands.YLCommands;

namespace SensorHub.Servers.Commands.YLCommands
{
    public class YLGetYLCmd : CommandBase<YLSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "0x10";
            }
        }

        public override void ExecuteCommand(YLSession session, BinaryRequestInfo requestInfo)//服务器接收到的二进制的请求实例当做参数被传进来
        {
            try
            {
                session.Logger.Info("雨量计上传数据:"+ BitConverter.ToString(requestInfo.Body));

                byte[] result = requestInfo.Body;
                //将采集到的数据保存到数据库
                byte[] receiveAddress = new byte[2];
                byte[] sendAddress = new byte[2];
                byte[] requestParams = new byte[11];
                byte[] dateTime = new byte[6];
                byte[] curMinCount = new byte[1];
                byte[] forMinCount = new byte[1];
                byte[] totalMinCount = new byte[2];
                byte[] order = new byte[1];
                

                Buffer.BlockCopy(result, 0, receiveAddress, 0, 2);
                Buffer.BlockCopy(result, 2, sendAddress, 0, 2);
                Buffer.BlockCopy(result, 4, order, 0, 1);
                Buffer.BlockCopy(result, 6, requestParams, 0, 11);
                Buffer.BlockCopy(requestParams, 0, dateTime, 0, 6);
                Buffer.BlockCopy(requestParams, 6, curMinCount, 0, 1);
                Buffer.BlockCopy(requestParams, 7, forMinCount, 0, 1);
                Buffer.BlockCopy(requestParams, 9, totalMinCount, 0, 2);


                //TODO LIST:更新session中的设备信息
                if (session.DeviceID == null)
                {
                    session.DeviceID = BitConverter.ToString(sendAddress).Replace("-", "");
                }
                
                //获取响应事务命令
                string reOrder = BitConverter.ToString(order);

                if (reOrder == "10")
                {
                    //获取接收端地址
                    string desId = BitConverter.ToString(receiveAddress).Replace("-", "");

                    //获取发送端地址
                    string sourId = BitConverter.ToString(sendAddress).Replace("-", "");

                    //获取上传时间
                    string timeStr = BitConverter.ToString(dateTime).Replace("-", "");
                    /*
                    DateTime upTime = Convert.ToDateTime("20" + timeStr.Substring(0, 2)
                        + "-" + timeStr.Substring(2, 2)
                        + "-" + timeStr.Substring(4, 2)
                        + " " + timeStr.Substring(6, 2)
                        + ":" + timeStr.Substring(8, 2)
                        + ":" + timeStr.Substring(10, 2));
                     */
                    DateTime upTime = Convert.ToDateTime("20" + dateTime[0].ToString()
                       + "-" + dateTime[1].ToString()
                       + "-" + dateTime[2].ToString()
                       + " " + dateTime[3].ToString()
                       + ":" + dateTime[4].ToString()
                       + ":" + dateTime[5].ToString());

                    
                    //获取当前分钟雨量的次数
                    int curMinuteCount = int.Parse(curMinCount[0].ToString());

                    //获取前一分钟雨量的次数
                    int forMinuteCount = int.Parse(forMinCount[0].ToString());

                    //获取总的雨量的次数
                    int totalMinuteCount = Int16.Parse(BitConverter.ToString(totalMinCount).Replace("-", ""),System.Globalization.NumberStyles.HexNumber);

                    Model.YLInfo dj = new YLInfo();
                    //接收端地址
                    dj.DstId = desId;
                    //发送端地址
                    dj.SrcId = sourId;
                    //当前分钟的雨量计算
                    dj.CurMinuteYuLiang = (curMinuteCount * 0.2).ToString();
                    //前一分钟个人雨量计算
                    dj.ForMinuteYuLiang = (forMinuteCount * 0.2).ToString();
                    //计算总的雨量
                    dj.TotalYuLiang = (totalMinuteCount * 0.2).ToString();
                    //采集时间
                    dj.UpTime = upTime;
                    //记录时间
                    dj.LogTime = DateTime.Now;

                    //任意一段时间累计的雨量
                    if (session.IsFirstTime)
                    {
                        session.YuLiangValue = 0;
                        dj.YLiangValue = session.YuLiangValue * 0.2;
                        session.IsFirstTime = false;
                    }
                    else
                    {
                        dj.YLiangValue = totalMinuteCount * 0.2 - session.YuLiangValue * 0.2;
                        session.YuLiangValue = totalMinuteCount;
                        new BLL.YLiang().insert(dj);
                        session.Logger.Info("雨量计：雨量数据已经保存！");
                    }
                  

                    //根据采集时间判断时间
                    TimeSpan ts = DateTime.Now.Subtract(dj.UpTime);
                    double miniutes = ts.TotalMinutes;
                    if (miniutes > 10.0)
                    {
                        byte[] set = ApplicationContext.getInstance().getUpdateTime();

                        byte[] sendCode = CodeUtils.yl_addCrc(set);
                        session.Send(sendCode, 0, sendCode.Length);
                        session.Logger.Info("服务器->设备：校时信息:" + BitConverter.ToString(sendCode));
                    }
                }
           
            }
            catch (Exception e)
            {
                session.Logger.Error("雨量计：雨量计数据保存失败：" + e.ToString());
            }
        }
        
    }
}
