
using System;
using System.Windows;
using ABC.Cube.Client.Service;
using Awp.Logging;
using CefSharp;
using CefSharp.WinForms;

namespace ABCAgreeAutoTestPluging4.test
{
    class BrowserTest
    {
        private AutoTestHelper autoTestHelper = new AutoTestHelper();

        private ILog logger = LogManager.GetLogger("BrowserTest");
        
        public  void ExecuteJs(string tranCode,string js) {

            try {

                logger.Info("准备获取浏览器对象-------------");

                ChromiumWebBrowser browser = Application.Current.Dispatcher.Invoke(new GetBrowser_DG(GetBrowser), tranCode) as ChromiumWebBrowser;

                logger.Info("获取到浏览器对象");

                logger.Info(browser);

                //logger.Info(browser.Address);
                
                //logger.Info(browser.IsBrowserInitialized);

                browser.ExecuteScriptAsync("alert(12312312321321312313)");

                var task = browser.EvaluateScriptAsync(js);

                object result = task.Result.Result;

                logger.Info(result);
            } catch (Exception e) {
                logger.Info("获取到的异常是："+e);
            }
            
            
        }

        private delegate ChromiumWebBrowser GetBrowser_DG(string transCode);

        private ChromiumWebBrowser GetBrowser(string transCode) {

            ChromiumWebBrowser browser = null;

            logger.Info("===============");

            try {

                logger.Info("-------------------");

                browser = autoTestHelper.TransactionContext("", transCode, out string message) as ChromiumWebBrowser;

                logger.Info("获取到的浏览器对象的地址：");

                logger.Info(browser.Address);
            } catch (Exception e) {

                logger.Info("捕捉到异常"+e);
            }
            
            return browser;
        }
    }
}
