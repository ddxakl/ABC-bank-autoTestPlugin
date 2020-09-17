using System;
using ABC.Cube.Client.Service;
using Awp.Logging;
using System.Windows.Threading;
using System.Windows;
using CefSharp.WinForms;
using CefSharp;
using ABC.Cube.Client.Core.Context;
using ABC.Cube.Client.Common.Interface;
using System.Collections.Generic;
using System.Threading;
using ABCAgreeAutoTestPluging4.CommonProperty;

namespace ABCAgreeAutoTestPluging4.Untils
{
    public class TradeUntils
    {
        private ILog logger = LogManager.GetLogger("TradeUntils");

        AutoTestHelper autoTestHelper = new AutoTestHelper();

        public  int OpenTrade(string transCode) {

            Property.currentTimePath = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            int result =0;

            try {

                logger.Info("ui线程调用接口打开交易");
                
                Application.Current.Dispatcher.Invoke(new OpenTransaction_DG(OpenTransaction), transCode);

            } catch (Exception e) {
                logger.Info("捕捉到的异常是"+e);
            }
            return result;
        }
        
        public int CloseTrade(string transCode)
        {
            int result = 0;

            try
            {
                Application.Current.Dispatcher.Invoke(new CloseTransaction_DG(CloseTransaction), transCode);
            }
            catch (Exception e)
            {
                logger.Info("捕捉到的异常是" + e);
            }
            return result;
        }

        public void ExecuteJs(string transCode) {
            logger.Info("执行js");
            try
            {
                Application.Current.Dispatcher.Invoke(new TransactionContext_DG(TransactionContext), transCode);
            }
            catch (Exception e)
            {
                logger.Info("捕捉到的异常是" + e);
            }
        }

        private delegate int OpenTransaction_DG(string transCode);

        private int OpenTransaction(string transCode)
        {
            return autoTestHelper.OpenTransaction("", transCode, out string message);
        }
        private delegate int CloseTransaction_DG(string transCode);

        private int CloseTransaction(string transCode)
        {
            return autoTestHelper.CloseTransaction("", transCode, out string message);
        }

        private delegate void TransactionContext_DG(string transCode);

        private void TransactionContext(string transCode)
        {

            Thread.Sleep(6000);

            logger.Info("等待6秒，等到浏览器完全加载");

            try {
                logger.Info("开始获取浏览器对象");
                
                CubeContext cube = autoTestHelper.TransactionContext("", transCode, out string message) as CubeContext;
                
                logger.Info("获取浏览器对象方法的返回值"+cube.ToString());
                logger.Info("----------------");

                logger.Info(cube.ActivePage);
                logger.Info("----------------");
                logger.Info(cube.ActiveCubePage);
                logger.Info("----------------");

                try {
                    List<object> list = cube.ActiveCubePages;

                    logger.Info("获取到的交易的页面数"+list.Count);

                    for (int i =0;i<list.Count;i++) {
                        logger.Info(list[i]);
                    }

                } catch (Exception a) {
                    logger.Info(a);
                }

                logger.Info("/////////////");

                logger.Info(cube.RootPage);

                logger.Info("----------------"+cube.RootPage.Container.Content.Content);
                
                ChromiumWebBrowser chromium = cube.RootPage.Container.Content.Content as ChromiumWebBrowser;
                
                logger.Info("获取到的浏览器对象的地址为" + chromium.Address);

                logger.Info("开始执行js");

                chromium.ExecuteScriptAsync("alert('11111111111111')");

            } catch (Exception e) {
                logger.Info("获取浏览器对象时遇到异常"+e);
            }
            
        }


        public string GetCurrentTrade() {
            
            string transCode = "";

            return transCode;
        }
    }
}
