using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.Untils;

namespace ABCAgreeAutoTestPluging4.Controller.Trade
{
    class ScriptExecute
    {
        public void AButtonClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {

            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ACheckBoxClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {

            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ARadioClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {

            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void SpanClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
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


        public void ACheckBoxGroupClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {

            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".$children" + "[" + (Convert.ToInt32(itemValue) - 1) + "].$el.firstChild.click();");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ATabClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".setActiveItemByIndex(" + itemValue + ")");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void AListBoxClick(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "').$refs." + itemCode + ".setActiveItemByIndex(" + itemValue + ")");

            ScreenShot.Shot(browserUntils.browser);

        }


        public void ATextInputSetValue(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ACurrencyInputSetValue(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ADateInputSetValue(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void APasswordSetValue(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            ScreenShot.Shot(browserUntils.browser);

        }
        public void ATextAreaSetValue(string pageCode, string itemCode, string itemValue, BrowserUntils browserUntils)
        {
            //可以触发vue文本框的校验
            browserUntils.ExecuteScript("window.findVueByName('" + pageCode + "')." + "$refs." + itemCode + ".$emit('input','" + itemValue + "')");

            ScreenShot.Shot(browserUntils.browser);

        }


    }
}
