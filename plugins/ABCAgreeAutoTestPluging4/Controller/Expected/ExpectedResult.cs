using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.Untils;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Controller.Expected
{
    class ExpectedResult
    {
        BrowserUntils browserUntils;

        public ExpectedResult(BrowserUntils browserUntils)
        {
            this.browserUntils = browserUntils;
        }

        /// <summary>
        /// 检查组件的预期结果
        /// </summary>
        /// <param name="tradeMessage"></param>
        /// <returns></returns>
        public bool CheckExpectedResult(JObject tradeMessage) {

            bool result = false;

            //string pageCode = tradeMessage.GetValue("pageCode").ToString();

            //string itemCode = tradeMessage.GetValue("itemCode").ToString();

            //string currentValue = browserUntils.ExecuteScript(" window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$options.propsData.value");
            
            //JToken resultMapList = tradeMessage.GetValue("resultMap");

            //foreach (JObject resultMap in resultMapList) {

            //    string expression = resultMap.GetValue("expression").ToString();

            //    string expectResultCode = resultMap.GetValue("expectResultCode").ToString();

            //    string className = "ABCAgreeAutoTestPluging4.Controller.Expected.HandleExpectedResult";

            //    Type type = Type.GetType(className);

            //    object obj = System.Activator.CreateInstance(type);//方法或构造函数的对象

            //    object[] parameters = new object[] { currentValue, expression};//调用的方法或构造函数的参数列表

            //    //获取方法信息
            //    MethodInfo method = type.GetMethod(expectResultCode, new Type[] {typeof(string), typeof(string) });

            //    result = bool.Parse(method.Invoke(obj, parameters).ToString());

            //}
            
            JToken resultMapList = tradeMessage.GetValue("resultMap");

            foreach (JObject resultMap in resultMapList)
            {
                string expression = resultMap.GetValue("expression").ToString();

                string expectResultCode = resultMap.GetValue("expectResultCode").ToString();

                string className = "ABCAgreeAutoTestPluging4.Controller.Expected.HandleExpectedResult";

                Type type = Type.GetType(className);

                object obj = System.Activator.CreateInstance(type);//方法或构造函数的对象

                object[] parameters = new object[] { tradeMessage, expression, browserUntils };//调用的方法或构造函数的参数列表

                //获取方法信息
                MethodInfo method = type.GetMethod(expectResultCode, new Type[] { typeof(string), typeof(string) });

                result = bool.Parse(method.Invoke(obj, parameters).ToString());

            }

            return result;
        }
    }
}
