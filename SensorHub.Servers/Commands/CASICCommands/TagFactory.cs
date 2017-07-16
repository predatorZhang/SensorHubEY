using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Servers.Commands.CASICCommands
{
    public  class TagFactory
    {
       public static Tag create(String oid,int len, String value)
       {
          Tag tag;
          if (UploadTag.isUploadTag(oid))
          {
              tag = new UploadTag(oid,len,value);
          }
          else if(oid==SystemDateTag.SYSTEM_DATE_OID)
           {
               tag = new SystemDateTag(oid, len, value);
           }
          else if (oid == SystemTimeTag.SYSTEM_TIME_OID)
          {
              tag = new SystemTimeTag(oid, len, value);
          }
          else if (oid == WakeUpTag.WAKEUP_TAG_OID)
          {
              tag = new WakeUpTag(oid, len, value);
          }
          else if (oid == CellTag.CELL_TAG_OID)
          {
              tag = new CellTag(oid, len, value);
          }
          else if(oid==SensorException0Tag.SENSOR_EXCEP0_TAG_OID)
          {
              tag = new SensorException0Tag(oid, len, value);
          }
          else if(oid==SensorException1Tag.SENSOR_EXCEP1_TAG_OID)
          {
              tag = new SensorException1Tag(oid, len, value);
          }
          else
          {
              tag = new NormalTag(oid, len, value);
          }
          return tag;
       }
    }
}
