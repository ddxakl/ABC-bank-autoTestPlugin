using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using ABCAgreeAutoTestPluging4.Controller.Rate;
using ABCAgreeAutoTestPluging4.Controller.Trade;
using ABCAgreeAutoTestPluging4.TradeController;
using Awp.Logging;
using Newtonsoft.Json.Linq;

namespace ABCAgreeAutoTestPluging4.Transdata
{
    public class ABCSocketServer
    {
        private ILog logger = LogManager.GetLogger("ABCSocketServer");

        private Socket recordSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Socket scriptSocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// 启动插件
        /// </summary>
        public void StarPlugin()
        {
            try
            {
                if (PortInUse(Property.recordPort) && PortInUse(Property.scriptPort))
                {
                    recordSocket.Bind(new IPEndPoint(IPAddress.Parse(Property.ip), Property.recordPort));

                    recordSocket.Listen(Property.maxConnectCount);

                    Thread thread = new Thread(new ThreadStart(StartSocketServer))
                    {
                        IsBackground = Property.IsBackgrond
                    };
                    thread.Start();

                    scriptSocket.Bind(new IPEndPoint(IPAddress.Parse(Property.ip), Property.scriptPort));

                    scriptSocket.Listen(Property.maxConnectCount);

                    Thread scriptThread = new Thread(new ThreadStart(StartScriptSocketServer))
                    {
                        IsBackground = Property.IsBackgrond
                    };
                    scriptThread.Start();

                }
            }
            catch (Exception e)
            {
                logger.Info("发生异常。。。。", e);
            }
        }

        private void StartScriptSocketServer()
        {
            while (Property.PluginFlag)
            {
                logger.Info("启动监听成功，等待接收数据");

                Socket socket = scriptSocket.Accept();

                if (socket != null)
                {
                    logger.Info("接收到数据");

                    NetworkStream networkStream = new NetworkStream(socket);

                    byte[] recBytesLenth = new byte[8];//设置每次读取的最大字节数

                    networkStream.Read(recBytesLenth, 0, recBytesLenth.Length);//读取json报文长度

                    string msgLen = "";

                    msgLen += Encoding.UTF8.GetString(recBytesLenth, 0, recBytesLenth.Length);

                    int len = System.Convert.ToInt32(msgLen);

                    byte[] recstr = new byte[len];

                    networkStream.Read(recstr, 0, recstr.Length);//读取报文

                    string recmsg = Encoding.UTF8.GetString(recstr, 0, recstr.Length);

                    JArray jArray = JArray.Parse(Convert.ToString(recmsg));

                    Property.PerformaceMark = true;
                    
                    new Thread(new ThreadStart(GetPerformance)).Start();

                    IExecuteScript executeScript = new ExecuteScript();

                    for (int i = 0; i < jArray.Count; i++)
                    {
                        logger.Info("接收到的数据是" + JObject.Parse(jArray[i].ToString()));
                        
                        executeScript.HandleConpoentByScriptt(JObject.Parse(jArray[i].ToString()));
                    }
                    
                    socket.Close();
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Test()
        {

            while (Property.PluginFlag)
            {

                logger.Info("flag为" + Property.PluginFlag);

                logger.Info("测试开始。。。。。。。。。。。。。。");

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 停止插件
        /// </summary>
        public void EndPlugin()
        {
            Property.PluginFlag = false;
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        public void StartSocketServer()
        {

            while (Property.PluginFlag)
            {
                logger.Info("启动监听成功，等待接收数据");

                Socket socket = recordSocket.Accept();

                if (socket != null)
                {
                    logger.Info("接收到数据");

                    NetworkStream networkStream = new NetworkStream(socket);

                    byte[] recBytesLenth = new byte[8];//设置每次读取的最大字节数

                    networkStream.Read(recBytesLenth, 0, recBytesLenth.Length);//读取json报文长度

                    string msgLen = "";

                    msgLen += Encoding.UTF8.GetString(recBytesLenth, 0, recBytesLenth.Length);

                    int len = System.Convert.ToInt32(msgLen);

                    byte[] recstr = new byte[len];

                    networkStream.Read(recstr, 0, recstr.Length);//读取报文

                    string recmsg = Encoding.UTF8.GetString(recstr, 0, recstr.Length);

                    JArray jArray = JArray.Parse(Convert.ToString(recmsg));

                    //执行组件操作，开始获取
                    Property.PerformaceMark = true;

                    new Thread(new ThreadStart(GetPerformance)).Start();

                    for (int i = 0; i < jArray.Count; i++)
                    {
                        
                        IHandleTradeMessage handleTradeMessage = new HandleRecordTradeMessage();

                        logger.Info("接收到的数据是" + JObject.Parse(jArray[i].ToString()));

                        handleTradeMessage.HandleTradeMessage(JObject.Parse(jArray[i].ToString()));
                        
                    }

                    socket.Close();
                }

            }
        }

        private void GetPerformance()
        {

            GetPerformance getPerformance = new GetPerformance();
            getPerformance.GetTerminalPerformance();
        }

        /// <summary>
        /// 判断端口是占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool PortInUse(int port)
        {
            bool inUse = true;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = false;
                    break;
                }
            }
            return inUse;
        }

    }
}
