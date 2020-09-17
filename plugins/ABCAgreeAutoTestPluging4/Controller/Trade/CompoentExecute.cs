using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.Untils;

namespace ABCAgreeAutoTestPluging4.Controller.Trade
{
    class CompoentExecute
    {
        public void AButton(string pageCode, string itemCode, string itemValue,BrowserUntils browserUntils) {

            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void Span(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {

            if (int.TryParse(itemValue, out int value))
            {
                browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + "[" + (value - 1) + "].click();");
            }
            else
            {
                browserUntils.ExecuteScript("function foo(){var list = window.findVueByName('" + pageCode + "').$refs." + itemCode + ";" +
                "for (var i = 0; i < list.length; i++){if (list[i].innerText == '" + itemValue + "'){list[i].click();} } } foo();");
            }
       
        }
        public void ARadioButton(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$children" + "[" + (Convert.ToInt32(itemValue) - 1) + "].$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void AText(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            browserUntils.ExecuteScript("window.focusEvent('" + pageCode + "','" + itemCode + "');");

            System.Threading.Thread.Sleep(1000);//执行操作之后延时时间
            
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            browserUntils.ExecuteScript("window.blurEvent('" + pageCode + "','" + itemCode + "');");
            
        }
        public void ATab(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".setActiveItemByIndex(" + itemValue + ")");
        }
    }
}
