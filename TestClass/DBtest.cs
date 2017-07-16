using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestClass
{
    [TestFixture]
    public class DBtest
    {

        [TestFixtureSetUp]
        public void Setup()
        {
            // setup
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
            try
            {
                SensorHub.Model.ConfigInfo cfg = new SensorHub.BLL.Config().getConfigByDevId("1");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
