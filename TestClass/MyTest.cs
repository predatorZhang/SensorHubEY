using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TestClass
{
    [TestFixture]
    public class MyTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            //
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
            string aa = DateTime.Now.ToString("yyyy-MM-dd");
            System.Console.Write(aa);
        }
    }
}
