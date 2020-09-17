using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.Controller.Expected;
using ABCAgreeAutoTestPluging4.test;
using ABCAgreeAutoTestPluging4.Untils;
using Awp.Logging;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.TradeController
{
    class HandleRecordTradeMessage : IHandleTradeMessage
    {
        private ILog logger = LogManager.GetLogger("HandleRecordTradeMessage");

        private string transCode;

        private BrowserUntils browserUntils;

        private TradeUntils tradeUntils = new TradeUntils();

        public bool CheckCompoent(string tansCode, string pageCode, string itemCode)
        {
            BrowserUntils.GetBrowser(tansCode, pageCode).ExecuteScript("");

            throw new NotImplementedException();
        }

        public bool CheckPage(string tansCode, string pageCode)
        {
            BrowserUntils.GetBrowser(tansCode, pageCode).ExecuteScript("");

            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查当前交易是否打开
        /// </summary>
        /// <param name="transCode"></param>
        /// <returns></returns>
        public bool CheckTrade(string transCode)
        {
            if (transCode.Equals(tradeUntils.GetCurrentTrade()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 操作组件信息
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <returns></returns>
        public JObject HandleCompoent(JObject tradeMessage)
        {


            string itemCode = tradeMessage.GetValue("itemCode").ToString();

            string pageCode = tradeMessage.GetValue("pageCode").ToString();

            string relation = tradeMessage.GetValue("relation").ToString();

            string executeTime = "";

            bool result = true;

            logger.Info("开始操作组件");

            browserUntils = BrowserUntils.GetBrowser(transCode, pageCode);

            logger.Info("获取浏览器对象");

            if (CheckPage(transCode, pageCode) && CheckCompoent(transCode, pageCode, itemCode))
            {
                if (tradeMessage.GetValue("isscreen").ToString().Equals("Y"))
                {
                    //截图
                    ScreenShot.Shot(browserUntils.browser);
                }
                if (relation != "")
                {
                    //执行表达式相关操作
                    HandleRelationExpression(relation);
                }
                if (tradeMessage.Property("resultMap") != null && tradeMessage.GetValue("resultMap").ToString() != "")
                {
                    //处理预期结果
                    ExpectedResult expectedResult = new ExpectedResult(browserUntils);

                    result = expectedResult.CheckExpectedResult(tradeMessage);
                }

                DateTime startTime = DateTime.Now;
                ExecuteCompoent(tradeMessage);
                DateTime endTime = DateTime.Now;
                executeTime = (endTime - startTime).Milliseconds.ToString();//操作组件的时间

                tradeMessage.Add("direction", executeTime);
                tradeMessage.Add("checkResult", result);

            }
            else
            {
                Property.executeResult = false;
            }
            return tradeMessage;
        }
        /// <summary>
        /// 根据关系表达式进行操作
        /// </summary>
        /// <param name="relation">表达式</param>
        private String HandleRelationExpression(string relation)
        {
            string itemValue = "";

            string relationType = relation.Substring(1, 3);//获取表达式的操作类型

            JArray jRelationArray = HandleRelation(relation);

            if (relationType.Equals("set"))
            {
                string relationKey = ((JObject)jRelationArray[0]).GetValue("relationKey").ToString();

                //获取保存在上下文中传递值
                itemValue = Property.relationValue.GetValue(relationKey).ToString();
            }
            else if (relationType.Equals("get"))
            {

                for (int i = 0; i < jRelationArray.Count; i++)
                {
                    string relationKey = ((JObject)jRelationArray[i]).GetValue("relationKey").ToString();
                    string relationValue = ((JObject)jRelationArray[i]).GetValue("relationValue").ToString();
                    //保存需要传递的数据(全局的json对象保存获取到)
                    Property.relationValue.Add(relationKey, relationValue);
                }
            }
            return itemValue;
        }

        /// <summary>
        /// 解析传递表达式,获取需要的传递的值
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        private JArray HandleRelation(string relation)
        {
            JArray jRelationArray = new JArray();

            string tempStr = relation.Replace(" ", "");

            string code = tempStr.Substring(3, tempStr.Length - 5);

            string[] codeArray = code.Split(',');

            for (int i = 1; i < codeArray.Length; i++)
            {
                string[] compoentInfoList = codeArray[i].Split('.');

                JObject json = new JObject
                {
                    {"realtionKey" ,compoentInfoList[0] + compoentInfoList[1] + compoentInfoList[2] },

                    { "relationValue",GetRelationValue(compoentInfoList[0], compoentInfoList[1], compoentInfoList[2])}
                };

                jRelationArray.Add(json);
            }
            return jRelationArray;
        }
        private string GetRelationValue(string tradeCode, string pageCode, string compoentId)
        {
            return BrowserUntils.GetBrowser(tradeCode, pageCode).ExecuteScript("function foo(){return window.findVueByName('" + pageCode + "').$refs." + compoentId + ".value};foo();");

        }

        public void HandleResult()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 处理接收到的报文信息
        /// </summary>
        /// <param name="tradeMessage"></param>
        public void HandleTradeMessage(JObject tradeMessage)
        {
            logger.Info("接收到的案例信息是" + tradeMessage);

            if (tradeMessage.Property("transCode") != null)
            {
                transCode = tradeMessage.GetValue("transCode").ToString();

                logger.Info("打开交易码为"+transCode+"的交易");

                OpenTrade(transCode);

                logger.Info("交易打开成功");

                ExeCuteLog.AppendExecuteLog("打开交易【" + transCode + "】");

               // JArray itemList = (JArray)tradeMessage.GetValue("itemList");

                JArray sceneStepList = (JArray)tradeMessage.GetValue("sceneStepList");

                logger.Info("sceneStepList---" + sceneStepList);

                JObject sceneStep = (JObject)sceneStepList[0];

                JArray itemList = (JArray)sceneStep.GetValue("stepItemList");

                logger.Info("itemList" + itemList);

                tradeUntils.ExecuteJs(transCode);




                JArray transFlowItemList = new JArray();

                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                logger.Info("处理组件信息");

                try
                {
                    foreach (JObject itemInfo in itemList)
                    {
                        if (Property.executeResult)
                        {
                            transFlowItemList.Add(HandleCompoent(itemInfo));
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Info("获取到的异常" + e);
                }

                logger.Info("组件操作完毕");

                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //将日志文件写入本地文件
                ExeCuteLog.WriteExecuteLog(Property.LocalPath + @"\" + Property.currentTimePath + @"\log\", Property.executeLog.ToString());
                //清除存放日志信息
                Property.executeLog.Remove(0, Property.executeLog.Length);

                UpLoadFile();

                tradeMessage.Add("componentIndex", transFlowItemList);
                tradeMessage.Add("executeResult", Property.executeResult);
                tradeMessage.Add("executeStartDate", startTime);
                tradeMessage.Add("executeEndDate", endTime);

                //关闭获取内存信息的线程
                Property.PerformaceMark = false;

                AddResultFlow(tradeMessage);

            }
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
            tradeMessage.Add("performance", Property.performance);//添加内存信息

            Property.PerformaceMark = false;

            Property.performance.RemoveAll();
            
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
        /// 打开交易
        /// </summary>
        /// <param name="transCode"></param>
        /// <returns></returns>
        public bool OpenTrade(string transCode)
        {
            bool isOpened = true;

            //if (!CheckTrade(transCode)) {
            try {
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
            } catch (Exception e) {
                logger.Info("打开交易时发生异常",e);
            }
            return isOpened;
        }

        /// <summary>
        /// 解析组件执行具体操作
        /// </summary>
        /// <param name="tradeMessage"></param>
        private bool ExecuteCompoent(JObject tradeMessage)
        {
            string itemCode = tradeMessage.GetValue("itemCode").ToString();

            string itemName = tradeMessage.GetValue("itemName").ToString();

            string pageCode = tradeMessage.GetValue("pageCode").ToString();

            string itemValue = tradeMessage.GetValue("itemValue").ToString();

            string objectType = tradeMessage.GetValue("objectType").ToString();

            string className = "ABCAgreeAutoTestPluging4.Controller.Trade.CompoentExecute";

            Type type = Type.GetType(className);

            object obj = System.Activator.CreateInstance(type);//方法或构造函数的对象

            object[] parameters = new object[] { pageCode, itemCode, itemValue, browserUntils };//调用的方法或构造函数的参数列表

            //获取方法信息
            MethodInfo method = type.GetMethod(objectType, new Type[] { typeof(string), typeof(string), typeof(string), typeof(BrowserUntils) });

            return bool.Parse(method.Invoke(obj, parameters).ToString());

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
