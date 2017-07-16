using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public abstract class Config
    {
        public abstract byte[] getConfig(byte[] src);
    }
}
