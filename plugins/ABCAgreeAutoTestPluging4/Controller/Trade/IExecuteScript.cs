using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Controller.Trade
{
    interface IExecuteScript
    {
         String HandleConpoentByScriptt(JObject scriptInfo);
    }
}
