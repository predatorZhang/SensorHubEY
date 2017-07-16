using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TestClass
{
    [TestFixture]
    public class HeartBeatTest
    {
        private IServerConfig m_Config;

        [TestFixtureSetUp]
        public void Setup()
        {
            m_Config = new ServerConfig
            {
                Port = 2015,
                Ip = "Any",
                MaxConnectionNumber = 1000,
                Mode = SocketMode.Tcp,
                Name = "GXServer"
            };

            //   m_Server = new RQServer();
            //   m_Server.Setup(m_Config, logFactory: new ConsoleLogFactory());
        }

        [SetUp]
        public void StartServer()
        {
            //   m_Server.Start();
        }

        [TearDown]
        public void StopServer()
        {
            //  m_Server.Stop();
        }

        [Test]
        public void TestCustomProtocol()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_Config.Port);

            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                //上传数据必须是/r/n结尾
                string sdata = "HeartBeat:12345,20140810";

                byte[] data = new byte[sdata.Length + 2];
                Encoding.ASCII.GetBytes(sdata, 0, sdata.Length, data, 0);
                socket.Connect(serverAddress);
                var socketStream = new NetworkStream(socket);
                var reader = new StreamReader(socketStream, Encoding.ASCII, false);
                data[sdata.Length] = 0x0D;
                data[sdata.Length + 1] = 0x0A;
                socketStream.Write(data, 0, data.Length);
                socketStream.Flush();
            }
        }
    }
}
