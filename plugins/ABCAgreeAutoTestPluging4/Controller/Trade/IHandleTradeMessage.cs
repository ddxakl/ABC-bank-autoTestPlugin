using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.TradeController
{
    public interface IHandleTradeMessage
    {
        void HandleTradeMessage(JObject tradeMessage);
        
        bool CheckTrade(string transCode);

        bool OpenTrade(string transCode);

        bool CheckPage(string tansCode, string pageCode);

        bool CheckCompoent(string tansCode, string pageCode, string itemCode);

        JObject HandleCompoent(JObject tradeMessage);

        void HandleResult();
    }
}
