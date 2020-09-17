using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ABC.Cube.Client.Service;
using Awp.Logging;
using CefSharp;
using CefSharp.WinForms;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class BrowserUntils 
    {
        private static ILog logger = LogManager.GetLogger("BrowserUntils");

        public ChromiumWebBrowser browser;
        
        private AutoTestHelper autoTestHelper = new AutoTestHelper();

        private BrowserUntils(string transCode, string pageCode) {

             Application.Current.Dispatcher.Invoke(new GetBrowser_DG(GetBrowser), transCode);
        }

        public static BrowserUntils GetBrowser(string transCode,string pageCode) {

            logger.Info("准备获取浏览器对象");

            return new BrowserUntils( transCode,  pageCode);
        }


        public delegate void GetBrowser_DG(string transCode);
        
        private void GetBrowser(string transCode) {


            browser = autoTestHelper.TransactionContext("", transCode, out string message) as ChromiumWebBrowser;

            logger.Info("获取到对象");
        }


        /// <summary>
        /// 执行js语句
        /// </summary>
        /// <param name="JSCode"></param>
        /// <returns></returns>
        public string ExecuteScript(string JSCode) {
            
            var task = browser.EvaluateScriptAsync(JSCode);

            object result = task.Result.Result;

            logger.Info(result);
            
            return result.ToString();
        }
    }
}
