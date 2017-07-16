using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
namespace SensorHub.Servers.Commands.RQCommands
{
    /**
     * 燃气周期性数据上报
     **/
    [RQCommandFilter]
    public class RQPeriodUploadCmd : CommandBase<RQSession, BinaryRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "7B-89-00-10";
            }
        }

        public override void ExecuteCommand(RQSession session, BinaryRequestInfo requestInfo)
        {
            try
            {
                session.Logger.Info("燃气数据已经上传！");
                session.Logger.Info(BitConverter.ToString(requestInfo.Body, 0, requestInfo.Body.Length));
                byte[] body = requestInfo.Body;

                //sim卡号
                byte[] sim = new byte[6];
                Buffer.BlockCopy(body, 0, sim, 0, 6);


                ///
                ///校时
                ///
                string now = DateTime.Now.ToString("yyMMddHHmmss");
                byte[] head = { 0x7B, 0x89, 0x00, 0x14, 
                                0x00, 0x15,
                                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                                0x02, 
                                0x10,
                                0x00, 0x34,
                                0x00, 0x03,
                                0x06};
                byte[] bcd = Utility.CodeUtils.Time2BCD(now);
                byte[] validateData = new byte[head.Length + bcd.Length];
                head.CopyTo(validateData, 0);
                bcd.CopyTo(validateData, head.Length);
                byte[] crc = Utility.CodeUtils.getCrcByModBusData(validateData);
                byte[] resp = new byte[validateData.Length + crc.Length];
                validateData.CopyTo(resp, 0);
                crc.CopyTo(resp, validateData.Length);

                session.Logger.Info("燃气开始校时");
                session.Send(resp, 0, resp.Length);
                session.Logger.Info("燃气校时数据已经发送！");
                session.Logger.Info(BitConverter.ToString(resp));
                

                if (string.IsNullOrEmpty(session.MacID))
                {
                    //session.MacID = BitConverter.ToString(sim, 0, sim.Length).Replace("-", "").Replace("F", "");
                    #region how can u repalce f in ff ff ff ff ??
                    session.MacID = BitConverter.ToString(sim, 0, sim.Length).Replace("-", "").Substring(1);
                    #endregion 
                }

                //主机地址
                string address = session.MacID;

                //本次上传一共有多少条数据
                //int count = (body.Length - 9) / 32;
                int count = (body.Length - 15) / 32;

                //将本次上传的所有采集数据保存在records数组中
                byte[] records = new byte[body.Length - 15];
                Buffer.BlockCopy(body, 13, records, 0, records.Length);

                List<Model.RQPeriodInfo> rqs = new List<Model.RQPeriodInfo>();
                for (int i = 0; i < count; i++)
                {
                    byte[] record = new byte[32];
                    byte[] dateTime = new byte[6];

                    //将第i条数据保存在record数组中
                    Buffer.BlockCopy(records, i * 32, record, 0, 32);
                    Buffer.BlockCopy(record, 0, dateTime, 0, 6);

                    session.Logger.Info(BitConverter.ToString(dateTime));
                    session.Logger.Info(Utility.CodeUtils.String2DateTime(BitConverter.ToString(dateTime).Replace("-", "")));

                    Model.RQPeriodInfo rq = new Model.RQPeriodInfo();
                    rq.ADDRESS = address;
                    rq.INPRESS = BitConverter.ToSingle(new byte[] { record[7], record[6], record[9], record[8] }, 0).ToString();
                    rq.OUTPRESS = BitConverter.ToSingle(new byte[] { record[11], record[10], record[13], record[12] }, 0).ToString();
                    rq.FLOW = BitConverter.ToSingle(new byte[] { record[15], record[14], record[17], record[16] }, 0).ToString();
                    rq.STRENGTH = BitConverter.ToSingle(new byte[] { record[19], record[18], record[21], record[20] }, 0).ToString();
                    rq.TEMPERATURE = BitConverter.ToSingle(new byte[] { record[23], record[22], record[25], record[24] }, 0).ToString();
                    rq.CELL = BitConverter.ToSingle(new byte[] { record[27], record[26], record[29], record[28] }, 0).ToString();
                    rq.UPTIME = Utility.CodeUtils.String2DateTime(BitConverter.ToString(dateTime).Replace("-", ""));
                    rq.LOGTIME = DateTime.Now;
                    rqs.Add(rq);
                }

                BLL.RQPeriod bll = new BLL.RQPeriod();
                bll.insert(rqs);
                session.Logger.Info("迅腾燃气数据已经保存！");

                //增加燃气数据上报设备运行日志thc20150610
                Model.DeviceLogInfo log = new Model.DeviceLogInfo();
                log.DEVICE_ID = session.ID;
                log.MESSAGE = "燃气监测数据上报！";
                log.OPERATETYPE = "燃气数据上报";
                log.LOGTIME = DateTime.Now;
                new BLL.DeviceLog().insert(log);


                //之后进行数据记录的操作
                byte[] first = { 0x7B, 0x89, 0x00, 0x10, 0x00, 0x0E };
                byte[] second = { 0x01, 0x10, 0x00, 0x2C, 0x00, 0x60 };
                byte[] modBusData = new byte[first.Length + sim.Length + second.Length];

                first.CopyTo(modBusData, 0);
                sim.CopyTo(modBusData, first.Length);
                second.CopyTo(modBusData, first.Length + sim.Length);

                byte[] crcData = Utility.CodeUtils.getCrcByModBusData(modBusData);
                byte[] responseData = new byte[modBusData.Length + crcData.Length];
                modBusData.CopyTo(responseData, 0);
                crcData.CopyTo(responseData, modBusData.Length);

                session.Send(responseData, 0, responseData.Length);

                session.Logger.Info("迅腾燃气数据上传确认已经发送！");



                ///
                ///配置SIM卡号
                ///
                //byte[] head = { 0x7B, 0x89, 0x00, 0x15, 
                //                  0x00, 0x0F, 
                //                  0x02, 
                //                  0x10, 
                //                  0x00, 0x56, 
                //                  0x00, 0x03, 
                //                  0x06, 
                //                  0xF1, 0x52, 0x32, 0x67, 0x15, 0x21 
                //              };
                //byte[] crc = Utility.CodeUtils.getCrcByModBusData(head);

                //byte[] resp = new byte[head.Length + crc.Length];

                //head.CopyTo(resp, 0);
                //crc.CopyTo(resp, head.Length);

                //session.Logger.Info("燃气SIM卡开始配置");
                //session.Send(resp, 0, resp.Length);
                //session.Logger.Info("燃气SIM卡号已经发送！");
                //session.Logger.Info(BitConverter.ToString(resp));

                ///
                ///读取采集器配置信息
                //
                //byte[] head = { 0x7B, 0x89, 0x00, 0x12, 
                //                 0x00, 0x0E, 
                //                 0xF1, 0x50, 0x11, 0x19, 0x19, 0x23, 
                //                 0x02, 
                //                 0x03, 
                //                 0x00, 0x0D, 
                //                 0x00, 0x27 };
                //byte[] crc = Utility.CodeUtils.getCrcByModBusData(head);
                //byte[] resp = new byte[head.Length + crc.Length];
                //head.CopyTo(resp, 0);
                //crc.CopyTo(resp, head.Length);

                //session.Logger.Info("燃气读取配置信息开始请求");
                //session.Send(resp, 0, resp.Length);
                //session.Logger.Info("燃气读取配置信息请求已经发送！");
                //session.Logger.Info(BitConverter.ToString(resp));

                //发送配置信息
                this.SendConfig(session, requestInfo);
            }
            catch (Exception e)
            {
                session.Logger.Error("燃气上传数据处理异常！");
                session.Logger.Error(e.ToString());
            }
        }


        private void SendConfig(RQSession session, BinaryRequestInfo requestInfo)
        {
            byte[] body = requestInfo.Body;
            byte[] sim = new byte[6];
            Buffer.BlockCopy(body, 0, sim, 0, 6);
            byte[] head = { 0x7B, 0x89, 0x00, 0x13,
                          0x00,0x55,
                          sim[0],sim[1],sim[2],sim[3],sim[4],sim[5],
                          0x02,
                          0x10,
                          0x00,0x11,
                          0x00,0x23,
                          0x46};

            string rqUploadPeriod = System.Configuration.ConfigurationSettings.AppSettings["RQ_UploadPeriod"];
            string collectPeriod = System.Configuration.ConfigurationSettings.AppSettings["RQ_CollectPeriod"];
            string inPressMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_inPressMax"];
            string inPressMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_inPressMin"];

            string outPressMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_outPressMax"];
            string outPressMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_outPressMin"];

            string flowMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_flowMax"];
            string flowMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_flowMin"];

            string densityMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_densityMax"];
            string densityMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_densityMin"];

            string tempMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_tempMax"];
            string tempMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_tempMin"];

            string voltageMax = System.Configuration.ConfigurationSettings.AppSettings["RQ_voltageMax"];
            string voltageMin = System.Configuration.ConfigurationSettings.AppSettings["RQ_voltageMin"];

            string phone0 = System.Configuration.ConfigurationSettings.AppSettings["RQ_phone0"];
            string phone1 = System.Configuration.ConfigurationSettings.AppSettings["RQ_phone1"];
           
            byte[] ss0 =  BitConverter.GetBytes(float.Parse(rqUploadPeriod));
            byte[] btuploadPeriod = { ss0[1], ss0[0], ss0[3], ss0[2] };//上传周期

            byte[] ss1 =  BitConverter.GetBytes(float.Parse(collectPeriod));
            byte[] btperiod = { ss1[1], ss1[0], ss1[3], ss1[2] };//采集周期

            byte[] ss2 =  BitConverter.GetBytes(float.Parse(inPressMax));
            byte[] btInPressMax = {ss2[1], ss2[0], ss2[3], ss2[2]};//进站压力上限

            byte[] ss3 =  BitConverter.GetBytes(float.Parse(inPressMin));
            byte[] btInPressMin = { ss3[1], ss3[0], ss3[3], ss3[2]};//进站压力下限

            byte[] ss4 =  BitConverter.GetBytes(float.Parse(outPressMax));
            byte[] btOutPressMax = {ss4[1], ss4[0], ss4[3], ss4[2]};//出站压力上限

            byte[] ss5 =  BitConverter.GetBytes(float.Parse(outPressMin));
            byte[] btOutPressMin = { ss5[1], ss5[0], ss5[3], ss5[2]};//出站压力下限

            byte[] ss6 =  BitConverter.GetBytes(float.Parse(flowMax));
            byte[] btFlowMax = {ss6[1], ss6[0], ss6[3], ss6[2]};//流量上限

            byte[] ss7 =  BitConverter.GetBytes(float.Parse(flowMin));
            byte[] btFlowMin = { ss7[1], ss7[0], ss7[3], ss7[2]};//流量下限

            byte[] ss8 =  BitConverter.GetBytes(float.Parse(densityMax));
            byte[] btDensityMax = {ss8[1], ss8[0], ss8[3], ss8[2]};//浓度上限

            byte[] ss9 =  BitConverter.GetBytes(float.Parse(densityMin));
            byte[] btDensityMin = { ss9[1], ss9[0], ss9[3], ss9[2]};//浓度下限

            byte[] ss10 =  BitConverter.GetBytes(float.Parse(tempMax));
            byte[] btTempMax = {ss10[1], ss10[0], ss10[3], ss10[2]};//温度上限

            byte[] ss11 =  BitConverter.GetBytes(float.Parse(tempMin));
            byte[] btTempMin = { ss11[1], ss11[0], ss11[3], ss11[2]};//温度下限

             byte[] ss12 =  BitConverter.GetBytes(float.Parse(voltageMax));
            byte[] btVoltageMax = {ss12[1], ss12[0], ss12[3], ss12[2]};//电压上限

            byte[] ss13 =  BitConverter.GetBytes(float.Parse(voltageMin));
            byte[] btVoltageMin = { ss13[1], ss13[0], ss13[3], ss13[2]};//电压下限

            byte[] btPhone0 = Utility.CodeUtils.String2ByteArray("F" + phone0); ;//报警手机号1
            byte[] btPhone1 = Utility.CodeUtils.String2ByteArray("F" + phone1); ;//报警手机号2

            byte[] isAlarm = { 0x00, 0x3F };

            byte[] sendData  = new byte[89];
            head.CopyTo(sendData, 0);
            btuploadPeriod.CopyTo(sendData,19);
            btperiod.CopyTo(sendData, 19+4);
            btInPressMax.CopyTo(sendData, 19 + 4 + 4);
            btInPressMin.CopyTo(sendData, 19 + 4 + 4 + 4);
            btOutPressMax.CopyTo(sendData,19 + 4 + 4 + 4 + 4);
            btOutPressMin.CopyTo(sendData,19 + 4 + 4 + 4 + 4 + 4);
            btFlowMax.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4);
            btFlowMin.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            btDensityMax.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 +4 );
            btDensityMin.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            btTempMax.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            btTempMin.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            btVoltageMax.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            btVoltageMin.CopyTo(sendData, 19 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4);
            isAlarm.CopyTo(sendData,75);
            btPhone0.CopyTo(sendData,77);
            btPhone1.CopyTo(sendData, 83);

            byte[] crcData = Utility.CodeUtils.getCrcByModBusData(sendData);
            byte[] responseData = new byte[sendData.Length + crcData.Length];
            sendData.CopyTo(responseData, 0);
            crcData.CopyTo(responseData, sendData.Length);

            session.Send(responseData, 0, responseData.Length);

            session.Logger.Info("燃气智能监测终端配置下发：" + BitConverter.ToString(responseData));
        }
    
    }
}
