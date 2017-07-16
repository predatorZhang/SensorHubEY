using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorHub.Utility
{
    public class StringUtil
    {
        /*
        convert the string to DateTime
         * the src should be formatted to things like 20130402120122 with the length of 14
         * */
        public static DateTime toDateTime(string src)
        {
            if (src.Length != 14)
            {
                throw new Exception();
            }
            try
            {
                string year = src.Substring(0, 4);
                string month = src.Substring(4, 2);
                string day = src.Substring(6, 2);
                string hour = src.Substring(8, 2);
                string min = src.Substring(10, 2);
                string second = src.Substring(12, 2);
                DateTime date = Convert.ToDateTime(year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + second);
                return date;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /*
         * 将埃德尔的数据上传帧里面的数据信息，转成DateTime类型
         * src必须是16进制表示：0x100203050E代表2014年05月03日02时16分
         * */
        public static DateTime toAdlerUptime(string src)
        {
            if (src.Length != 10)
            {
                throw new Exception();
            }
            try
            {
                string year = (Int32.Parse(src.Substring(8, 2), System.Globalization.NumberStyles.HexNumber) + 2000).ToString();
                string mon = Int32.Parse(src.Substring(6, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string day = Int32.Parse(src.Substring(4, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string hor = Int32.Parse(src.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString();
                string min = Int32.Parse(src.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString();

                DateTime upTime = Convert.ToDateTime(year + "-" + mon + "-" + day + " " + hor + ":" + min + ":00");
                return upTime;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static byte YEAR
        {
            get { return byte.Parse(DateTime.Now.ToString("yy")); }
        }

        public static byte MON
        {
            get { return byte.Parse(DateTime.Now.ToString("MM")); }
        }
        public static byte DAY
        {
            get { return byte.Parse(DateTime.Now.ToString("dd")); }
        }
        public static byte HOR
        {
            get { return byte.Parse(DateTime.Now.ToString("HH")); }
        }
        public static byte MIN
        {
            get { return byte.Parse(DateTime.Now.ToString("mm")); }
        }
        public static byte SEC
        {
            get { return byte.Parse(DateTime.Now.ToString("ss")); }
        }
        public static byte WEEK
        {
            get { return byte.Parse("0" + ((int)DateTime.Now.DayOfWeek).ToString()); }
        }

        public static String To16HexString(String src)
        {
            if (src.Length == 4)
            {
                return src;
            }
            String temp = "";
            for (int i = 0; i < 4 - src.Length; i++)
            {
                temp = temp + "0";
            }
            return temp + src;
        }
    }
}
