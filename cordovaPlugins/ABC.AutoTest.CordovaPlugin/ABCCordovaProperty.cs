using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.AutoTest.CordovaPlugin
{
    class ABCCordovaProperty
    {
        static string configPath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString() + ".config";

        private static Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
        {
            ExeConfigFilename = configPath

        }, ConfigurationUserLevel.None);

        //本地存储录制好的脚本的路径
        public static string localRecordPath = configuration.AppSettings.Settings["localRecordPath"].Value;
    }
}
