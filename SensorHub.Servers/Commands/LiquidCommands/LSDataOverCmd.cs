using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SensorHub.Model;
using SensorHub.BLL;
using SensorHub.Utility;

namespace SensorHub.Servers.Commands
{
    public class LSDataOverCmd : CommandBase<SZLiquidSession, StringRequestInfo>
    {
        public override string Name
        {
            get
            {
                return "LSDATOVER";
            }
        }

        public override void ExecuteCommand(SZLiquidSession session, StringRequestInfo requestInfo)
        {
            session.Logger.Info("LSDATOVER！");
            try
            {
                //数据格式处理
                string[] data = requestInfo.Body.Split(',');
                String dataStr = "";
                for (int i = 0; i < data.Length - 1; i++)
                {
                    dataStr += data[i];
                    if (i != data.Length - 2)
                    {
                        dataStr += ',';
                    }
                }
                String header9received = data[data.Length - 1];

                //数据应用
                String macId;
                if (string.IsNullOrEmpty(session.MacID))
                {
                    macId = "未知";
                }
                else
                {
                    macId = session.MacID;                    
                }
                session.Logger.Info("液位监测仪器：" + macId + "：液位数据上传完成帧： " + dataStr);

                byte[] batchSet = System.BitConverter.GetBytes(session.Batch);
                byte[] stateSet = { 0x50, 
                                    0x00, 0x09,
                                    0x02,
                                    0x00, 0x00,0x34,
                                    0x22,
                                    batchSet[3], batchSet[2], batchSet[1], batchSet[0],
                                    0x03};
                byte[] set = new byte[stateSet.Length + 9];
                for (int i = 9; i < set.Length; i++)
                {
                    set[i] = stateSet[i - 9];
                }
                
                set[0] = 0XAA;
                set[1] = 0X1D;
                set[2] = byte.Parse(header9received.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                set[3] = byte.Parse(header9received.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                set[4] = 0X03;
                set[5] = 0X00;
                set[6] = System.BitConverter.GetBytes(set.Length-9)[0];
                set[7] = 0X00;
                set[8] = 0X00;

                String crcIn = "";
                for (int i = 0; i < set.Length; i++)
                {
                    crcIn += set[i].ToString("X2");
                }
                ushort crcOut = CodeUtils.QuickCRC16(crcIn, 0, crcIn.Length);
                byte[] crcOutByte = BitConverter.GetBytes(crcOut);
                set[7] = crcOutByte[1];
                set[8] = crcOutByte[0];
                session.Send(set, 0, set.Length);
                session.Logger.Info("成功下发液位数据上传完成帧:" + BitConverter.ToString(set, 0));
            }
            catch (Exception e)
            {
                session.Logger.Error("液位数据上传完成帧下发失败" + requestInfo.Body);
                session.Logger.Error(e.ToString());
            }
            finally
            {
            }
        }
    }
}
