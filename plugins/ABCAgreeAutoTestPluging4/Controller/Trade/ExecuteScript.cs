using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.Untils;
using Awp.Logging;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Controller.Trade
{
    class ExecuteScript : IExecuteScript
    {
        TradeUntils tradeUntils = new TradeUntils();

        private BrowserUntils browserUntils;

        private ILog logger = LogManager.GetLogger("ExecuteScript");

        string startTime = "";
        

        public string HandleConpoentByScriptt(JObject scriptInfo)
        {
            string result = ""; 
           
            //停止
            if (scriptInfo.Property("stopPlugin") != null && scriptInfo.GetValue("stopPlugin").ToString() != "")
            {

                Property.PerformaceMark = false;

                return StopPlugin();
            }
            //打开交易
            if (scriptInfo.Property("openTrade") != null && scriptInfo.GetValue("openTrade").ToString() != "")
            {
                return OpenTrade(scriptInfo.GetValue("openTrade").ToString()).ToString();
            }
            //关闭交易
            if (scriptInfo.Property("closeTrade") != null && scriptInfo.GetValue("closeTrade").ToString() != "")
            {
                return CloseTrade(scriptInfo.GetValue("closeTrade").ToString());
            }
            //截图
            if (scriptInfo.Property("isScreenShot") != null && scriptInfo.GetValue("isScreenShot").ToString() != "")
            {
                browserUntils = BrowserUntils.GetBrowser(scriptInfo.GetValue("transCode").ToString(), scriptInfo.GetValue("pageCode").ToString());

                //截图
                ScreenShot.Shot(browserUntils.browser);

                return  browserUntils.ExecuteScript("截图的js语句");
            }
            
            DateTime startTime = DateTime.Now;
            result = HandleCompoent(scriptInfo);
            DateTime endTime = DateTime.Now;
            string executeTime = (endTime - startTime).Milliseconds.ToString();//操作组件的时间
            scriptInfo.Add("direction", executeTime);
            scriptInfo.Add("checkResult", result);

            Property.scriptMessage.Add(scriptInfo);//将脚本中信息存储起来


            return result;
            
        }
        /// <summary>
        /// 关闭交易
        /// </summary>
        /// <param name="tradeCode"></param>
        private string CloseTrade(string tradeCode)
        {
            JObject resultFlow = new JObject();

            Property.PerformaceMark = false;

            resultFlow.Add("componentIndex", Property.scriptMessage);
            resultFlow.Add("executeResult", Property.executeResult);
            resultFlow.Add("executeStartDate", startTime);
            resultFlow.Add("executeEndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            resultFlow.Add("performance", Property.performance);//添加内存信息

            Property.performance.RemoveAll();
            //上传日志和截图信息
            UpLoadFile();
            //返回执行流水到
            AddResultFlow(resultFlow);

            return tradeUntils.CloseTrade(tradeCode).ToString();
        }

        //停止插件
        private string StopPlugin()
        {
            
            throw new NotImplementedException();
        }

        private string HandleCompoent(JObject scriptInfo)
        {
            
            string methodNanme = "";
            
            string tradeCode = scriptInfo.GetValue("tradeCode").ToString();
            string pageCode = scriptInfo.GetValue("pageCode").ToString();
            string compoentId = scriptInfo.GetValue("compoentId").ToString();
            string compoentValue = scriptInfo.GetValue("compoentValue").ToString();
            string action = scriptInfo.GetValue("action").ToString();
            string compoentType = scriptInfo.GetValue("compoentType").ToString();

            BrowserUntils browserUntils = BrowserUntils.GetBrowser(tradeCode, pageCode);

            methodNanme = compoentType + action;
      
            string className = "ABCAgreeAutoTestPluging4.Controller.Trade.ScriptExecute";

            Type type = Type.GetType(className);

            object obj = System.Activator.CreateInstance(type);//方法或构造函数的对象

            object[] parameters = new object[] { pageCode, compoentId, compoentValue, browserUntils };//调用的方法或构造函数的参数列表

            //获取方法信息
            MethodInfo method = type.GetMethod(methodNanme, new Type[] { typeof(string), typeof(string), typeof(string), typeof(BrowserUntils) });

            return method.Invoke(obj, parameters).ToString();
        }



        /// <summary>
        /// 打开交易
        /// </summary>
        /// <param name="transCode"></param>
        /// <returns></returns>
        public bool OpenTrade(string transCode)
        {
            Property.PerformaceMark = true;

            startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            bool isOpened = true;

            //if (!CheckTrade(transCode)) {
            try
            {
                if (true)
                {
                    if (tradeUntils.OpenTrade(transCode).Equals(0))
                    {
                        isOpened = true;
                    }
                    else if (tradeUntils.OpenTrade(transCode).Equals(-1))
                    {
                        isOpened = false;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Info("打开交易时发生异常", e);
            }
            return isOpened;
        }

        private void AddResultFlow(JObject tradeMessage)
        {
            List<FileInfo> imageFileList = ReadFile(Property.LocalPath + @"\" + Property.currentTimePath + @"\image");
            List<FileInfo> logFileList = ReadFile(Property.LocalPath + @"\" + Property.currentTimePath + @"\log");

            string abExecuteLog = logFileList[0].FullName.ToString().Replace(Property.LocalPath, Property.SftpPath);

            List<string> picList = new List<string>();

            for (int i = 0; i < imageFileList.Count; i++)
            {
                picList.Add(imageFileList[i].FullName.ToString().Replace(Property.LocalPath, Property.SftpPath));
            }
            string s = string.Join(",", picList.ToArray());

            string s1 = s.Replace(",", "','");

            tradeMessage.Add("picPathList", "['" + s1 + "']");
            tradeMessage.Add("abExecuteLog", abExecuteLog);
           
            HttpServer.AddResultFlow(Property.HttpUrl, tradeMessage.ToString());
        }

        /// <summary>
        /// 上传日志截图
        /// </summary>
        private void UpLoadFile()
        {

            List<FileInfo> fileList = ReadFile(Property.LocalPath + @"\" + Property.currentTimePath);

            //通过ftp或者sftp上传日志截图信息
            if (String.Equals("sftp", Property.UpLoadMethod, StringComparison.CurrentCultureIgnoreCase))
            {

                SftpUntils sftpUntils = new SftpUntils(Property.SftpIp, Property.SftpPort, Property.SftpUser, Property.SftpPassword);

                for (int i = 0; i < fileList.Count; i++)
                {

                    string remotePath = fileList[i].Directory.ToString().Replace(Property.LocalPath, "");

                    sftpUntils.UpLoadFile(fileList[i].FullName, Property.SftpPath + remotePath);
                }

            }
            else if (String.Equals("ftp", Property.UpLoadMethod, StringComparison.CurrentCultureIgnoreCase))
            {
                FtpUntils sftpUntils = new FtpUntils(Property.FtpIp, Property.FtpPort, Property.ABCFtpUser, Property.ABCFtpPassword);

                for (int i = 0; i < fileList.Count; i++)
                {
                    string remotePath = fileList[i].Directory.ToString().Replace(Property.LocalPath, "");

                    sftpUntils.UploadFile(fileList[i], Property.SftpPath + remotePath);
                }
            }
        }
        /// <summary>
        /// 读取本地路径下保存的截图和日志文件信息
        /// </summary>
        /// <returns></returns>
        private List<FileInfo> ReadFile(string path)
        {
            List<FileInfo> uploadFilesInfo = new List<FileInfo>();
            try
            {

                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                //根据文件目录获取该目录下文件夹数据
                DirectoryInfo[] dirs = directoryInfo.GetDirectories();

                //判断改目录下是否有文件夹了 如果没有则循环获取里面文件信息
                if (dirs.Count() == 0)
                {
                    //获取文件路径集合
                    string[] paths = Directory.GetFiles(path);

                    //循环paths 将每个文件信息放入List里
                    foreach (string filepath in paths)
                    {
                        FileInfo file = new FileInfo(filepath); //获取单个文件

                        uploadFilesInfo.Add(file);
                    }
                    //将文件信息拼成json返回  
                    //json = Newtonsoft.Json.JsonConvert.SerializeObject(files);
                }
            }
            catch (IOException io)
            {
                if (io.InnerException is System.IO.FileNotFoundException)
                {

                    //logger.Info("找不到文件");
                }
            }
            return uploadFilesInfo;

        }
    }
}
