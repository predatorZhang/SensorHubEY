using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.ADCommands
{
    public class TagConfig:Config
    {
        private Config config;

        public TagConfig(Config config)
        {
            this.config = config;
        }

        public override byte[] getConfig(byte[] src)
        {
            byte[] tag = null;
            if (config != null)
            {
                tag = config.getConfig(src);
                return tag;
            }
            return src;
        }
    }
}
