using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class ExeCuteLog
    {
        //按指定格式组装日志
        public static void AppendExecuteLog(string log)
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            Property.executeLog.Append(currentTime + "-" + log);

            Property.executeLog.Append("\r\n\r\n");

        }

        /// <summary>
        /// 将executeLog中的内容按照特定的格式写入到指定的文件中
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="executeLog"></param>
        /// <returns></returns>
        public static string  WriteExecuteLog(string logPath ,string executeLog)
        {
            string logFileInfo = logPath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);  //创建目录
            }
            System.IO.File.WriteAllText(logFileInfo, executeLog, Encoding.Default);

            return logPath;
        }


    }
}
