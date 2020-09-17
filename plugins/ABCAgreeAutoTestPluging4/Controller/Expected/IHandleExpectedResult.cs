using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.Untils;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.TradeController
{
    interface IHandleExpectedResult
    {
        bool CheckNumberResult(string currentValue, string expression);

        bool CheckStringResult(string currentValue, string expression);

        bool CheckDialogResult(JObject tradeMessage, string expression, BrowserUntils browserUntils);

        bool CheckDialogRegexResult(JObject tradeMessage, string expression, BrowserUntils browserUntils);

        bool CheckCompoentResult(JObject tradeMessage, string expression, BrowserUntils browserUntils);

        bool CheckTableResult(JObject tradeMessage, string expression, BrowserUntils browserUntils);
        


    }
}
