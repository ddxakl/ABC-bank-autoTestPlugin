using System;
using System.Data;
using System.Text.RegularExpressions;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.TradeController;
using ABCAgreeAutoTestPluging4.Untils;
using Awp.Logging;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Controller.Expected
{
    class HandleExpectedResult : IHandleExpectedResult
    {
        private ILog logger = LogManager.GetLogger("HandleExpectedResult");
        private string currentValue;

        private DataTable datatable = new DataTable();

        /// <summary>
        /// 检查组件返回的结果
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckCompoentResult(JObject tradeMessage, string expression, BrowserUntils browserUntils)
        {
            Match match = null;
            
            string checkExpression = "";
            
            string pageCode = tradeMessage.GetValue("tradeCode").ToString();

            if (expression.Contains("or") || expression.Contains("and"))
            {
                match = Regex.Match(expression, @"\{(.*)\}.*\{(.*)\}");
            }
            else
            {
                match = Regex.Match(expression, @"\{(.*)\}");
            }
            for (int i = 1; i < match.Groups.Count; i++) {

                string compoentInfo = match.Groups[i].Value;
                
                string[] compoentList = compoentInfo.Split('.');
                
                //判断是否在同一个页面,获取需要的检查的组件当前的值
                if (compoentList[1].Equals(pageCode)) {

                    string currentValue = browserUntils.ExecuteScript(" window.findVueByName('" + pageCode + "').$refs." + compoentList[2] + ".$options.propsData.value");

                    checkExpression = expression.Replace("{"+ compoentInfo+"}", currentValue);
                }
                else {
                    string currentValue = Property.relationValue.GetValue(compoentInfo.Replace(".","")).ToString();

                    checkExpression = expression.Replace("{"+ currentValue+"}", currentValue);
                }
            }

            logger.Info("需要检查的表达式的值为："+checkExpression);

            return bool.Parse(datatable.Compute(checkExpression, "").ToString());
        }
        /// <summary>
        /// 弹窗提示信息正则匹配
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckDialogRegexResult(JObject tradeMessage, string expression, BrowserUntils browserUntils)
        {
            currentValue = browserUntils.ExecuteScript("获取弹出框信息的js语句");

            Regex regex = new Regex(expression.Substring(1, expression.Length - 1));

            return regex.IsMatch(currentValue);
        }
        /// <summary>
        /// 弹窗提示信息判断
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckDialogResult(JObject tradeMessage, string expression, BrowserUntils browserUntils)
        {
            currentValue = browserUntils.ExecuteScript("获取弹出框信息的js语句");

            if (expression.IndexOf("val") != -1)
            {
                expression = expression.Replace("val", currentValue);

                return bool.Parse(datatable.Compute(expression, "").ToString());
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查表格结果
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckTableResult(JObject tradeMessage, string expression, BrowserUntils browserUntils)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查对比数字
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckNumberResult(string currentValue, string expression)
        {
            if (expression.IndexOf("value") != -1)
            {
                expression = expression.Replace("value", currentValue);

                return bool.Parse(datatable.Compute(expression, "").ToString());
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查字符串的值
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool CheckStringResult(string currentValue, string expression)
        {
            if (currentValue.Equals(expression))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
