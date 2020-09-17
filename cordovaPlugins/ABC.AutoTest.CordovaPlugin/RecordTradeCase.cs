using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ABC.AutoTest.CordovaPlugin;
using Awp.Logging;
using Awp.SS.Cordova;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json.Linq;

namespace ABCATPlugin.TradeRecording
{
    /// <summary>
    /// 获取录制之后的组件信息，并按一定的格式组装输出到指定位置
    /// </summary>
    [CordovaPlugin]
    public class RecordTradeCase : AttributedCordovaPlugin
    {
        private readonly ILog Logger = LogManager.GetLogger(typeof(RecordTradeCase));

        private ChromiumWebBrowser chromiumWebBrowser;

        public bool flag = false;

        public List<string> recordMsg = new List<string>();


        /// <summary>
        /// 开始录制之后，改变flag的状态，控制录制起止
        /// </summary>
        [CordovaPluginAction]
        public void SetStartFlag(CallbackContext callback) {

            this.flag = true;

            Logger.Info("此时flag的值为true");
        }
        /// <summary>
        /// 开始录制之后，改变flag的状态，控制录制起止
        /// </summary>
        [CordovaPluginAction]
        public void SetEndFlag(CallbackContext callback)
        {
            this.flag = false;

            Logger.Info("录制结束，保存录制结果");

            GetTradeRecord();

            recordMsg.Clear();

            Logger.Info("此时flag的值为false");

        }
        /// <summary>
        /// 每次打开新的页面时，查询是否在录制状态
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CordovaPluginAction]
        public bool GetFlag(CallbackContext callback) {
            
            callback.Success(this.flag);
            
            return this.flag;
        }
        /// <summary>
        /// 获取录制的组件信息
        /// </summary>
        /// <param name="recContext"></param>
        /// <param name="callback"></param>
        [CordovaPluginAction]
        public void GetRecContext(string recContext , CallbackContext callback) {

            if (this.flag) {

                Logger.Info("获取到的录制的组件信息"+ recContext);

                JObject JrecContext = JObject.Parse(recContext);

                if (JrecContext.Property("tradeCode") == null) {

                    //此处需要查找通过调用方法获取tradeCode交易码
                    
                    JrecContext.Add("tradeCode","");
                }
                recordMsg.Add(JrecContext.ToString());

                recContext = null;
            }
         
        }

        public void GetTradeRecord() {
            
            string currentTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            
            Logger.Info("交易的返回值是" + recordMsg.ToArray());

            string s = string.Join(",", recordMsg.ToArray());
            
            string recordTradeMsg = "["+s+"]";

            Logger.Info("交易的返回值是" + recordTradeMsg);

            Logger.Info("录制的脚本需要保存的路径是："+ ABCCordovaProperty.localRecordPath);
            
            this.CheckPath(ABCCordovaProperty.localRecordPath);
            
            File.WriteAllText(ABCCordovaProperty.localRecordPath + currentTime + ".json", recordTradeMsg, new UTF8Encoding(false));

        }

        /// <summary>
        /// 判断路径是否存在，如果不存在则新建路径
        /// </summary>
        /// <param name="path"></param>
        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Logger.Info("文件路径不存在!");

                Directory.CreateDirectory(path);  //创建目录
            }
        }

    }
}
