using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCATPluging.UseRate;
using Awp.Logging;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Controller.Rate
{
    public class GetPerformance
    {

        public void GetTerminalPerformance()
        {
            GetRate getRate = new GetRate();

            while (Property.PerformaceMark)
            {
                string cpuRate = Math.Round(getRate.GetCpuRate(), 2).ToString();

                string memeryRate = Math.Round(getRate.GetMemeryRate(), 2).ToString();

                JObject jObject = new JObject
                {
                      { "menery",memeryRate },

                      { "cpu", cpuRate },
                };
                Property.performance.Add(jObject);

                Thread.Sleep(1000);
            }
        }

    }
}
