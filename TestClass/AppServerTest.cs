using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketEngine;
using System.Net.Sockets;
using SensorHub.Servers;
namespace TestClass
{
   [TestFixture]
    public class AppServerTest
    {


       private byte[] data = {0x7D,0x01,0x00,0x16,0x00,0x12,0x31,0x35,
                                 0x33,0x31,0x34,0x37,0x39,0x34,0x33,
                                 0x33,0x36,0x01,0xCB,0x04,0xF2,0x13,0x89,0x7D};
        
        //private RQServer  m_Server;
        private IServerConfig m_Config;

        [TestFixtureSetUp]
        public void Setup()
        {
            m_Config = new ServerConfig
                {
                    Port = 2013,
                    Ip = "Any",
                    MaxConnectionNumber = 1000,
                    Mode = SocketMode.Tcp,
                    Name = "RQServer"
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
                socket.Connect(serverAddress);

                var socketStream = new NetworkStream(socket);
                var reader = new StreamReader(socketStream, Encoding.ASCII, false);
                socketStream.Write(data, 0,data.Length);
                socketStream.Flush();

                Console.WriteLine("Sent: " + BitConverter.ToString(data));

                var line = reader.ReadLine();
                 
                }
            }
        }
}
