using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorHub.Utility;
using NUnit.Framework;
namespace TestClass
{
    [TestFixture]
    public class CRC16Test
    {
        [Test]
        public void testCRC()
        {
                string data = "0101020404028485A1";
                string crc = CodeUtils.CRC16_Standard(data,0,data.Length);
                Console.Write(crc);
        }
    }
}
