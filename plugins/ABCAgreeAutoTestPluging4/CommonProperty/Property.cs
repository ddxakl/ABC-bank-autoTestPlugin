using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.CommonProperty
{
    class Property
    {
        static string configPath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString() + ".config";

        private static Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
        {
            ExeConfigFilename = configPath

        }, ConfigurationUserLevel.None);

        //存放日志和截图的路径
        public static string currentTimePath="";

        //启动的socket监听的本地ip地址
        public static string ip = configuration.AppSettings.Settings["ip"].Value;
        //录制回放的监听端口
        public static int recordPort = Convert.ToInt32(configuration.AppSettings.Settings["recordPort"].Value);
        //脚本执行交易的监听
        public static int scriptPort = Convert.ToInt32(configuration.AppSettings.Settings["scriptPort"].Value);
        //socket监听最大等待连接数
        public static int maxConnectCount = Convert.ToInt32(configuration.AppSettings.Settings["maxConnectCount"].Value);

        //执行机运行内存和物理内存信息
        public static JArray performance = new JArray();

        //存储一个完整脚本案例的组件信息
        public static JArray scriptMessage = new JArray();

        //日志信息
        public static StringBuilder executeLog = new StringBuilder();


        //保存值传递的表达式对应的值
        public static JObject relationValue= new JObject();

        //配置上传服务的选择sftp或者ftp
        public static string UpLoadMethod = configuration.AppSettings.Settings["UpLoadMethod"].Value;

        //ftp
        public static string FtpIp = configuration.AppSettings.Settings["FtpIp"].Value;
        public static int FtpPort = Convert.ToInt32(configuration.AppSettings.Settings["FtpPort"].Value);
        public static string ABCFtpUser = configuration.AppSettings.Settings["ABCFtpUser"].Value;
        public static string ABCFtpPassword = configuration.AppSettings.Settings["ABCFtpPassword"].Value;

        //sftp
        public static string SftpIp = configuration.AppSettings.Settings["SftpIp"].Value;
        public static int SftpPort = Convert.ToInt32(configuration.AppSettings.Settings["SftpPort"].Value);
        public static string SftpUser = configuration.AppSettings.Settings["SftpUser"].Value;
        public static string SftpPassword = configuration.AppSettings.Settings["SftpPassword"].Value;
        //服务器默认路径下的相对路径
        public static string SftpPath = configuration.AppSettings.Settings["SftpPath"].Value;


        //本地截图日志路径，例如："C:\screen"
        public static string LocalPath = configuration.AppSettings.Settings["LocalPath"].Value;

        //控制获取内存的线程的启停
        public static bool PerformaceMark = false;

        //http服务路径HttpUrl
        public static string HttpUrl = configuration.AppSettings.Settings["HttpUrl"].Value;

        public static bool executeResult = true;

        public static bool PluginFlag = true;

        //设置前台线程还是后台线程
        public static bool IsBackgrond = Convert.ToBoolean(configuration.AppSettings.Settings["IsBackgrond"].Value);
    }
}
