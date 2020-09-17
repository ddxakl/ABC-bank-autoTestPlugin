using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Awp.Composition;
using Awp.Logging;

namespace ABCAgreeAutoTestPluging4.Transdata
{
    [Part]
    class ABCPlugin
    {
        private ILog logger = LogManager.GetLogger("ABCPlugin");

        ABCSocketServer aBCSocketServer = new ABCSocketServer();

        [PartActivationMethod]
        public void Start() {

            logger.Info("插件启动。。。。。");
            
            aBCSocketServer.StarPlugin();

        }
        [PartDeactivationMethod]
        public void End()
        {
            aBCSocketServer.EndPlugin();

        }


    }
}
