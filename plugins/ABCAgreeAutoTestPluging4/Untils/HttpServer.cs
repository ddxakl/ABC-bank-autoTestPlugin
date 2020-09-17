using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Awp.Logging;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class HttpServer
    {
        private static ILog logger = LogManager.GetLogger("HttpServer");

        /// <summary>
        /// 通过http接口发送和接收信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="executeFlow"></param>
        /// <returns></returns>
        public static String AddResultFlow(string url, string executeFlow)
        {
            //创建一个请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ProtocolVersion = new Version(1, 0);
            //post请求方式
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/json";

            //设置参数，并进行url编码

            //将json字符串转化为字节utf-8
            byte[] payLoad = Encoding.UTF8.GetBytes(executeFlow);

            request.ContentLength = payLoad.Length;

            //将字符串转为gbk编码的字节
            //byte[] payLoad = Encoding.GetEncoding("GBK").GetBytes(executeFlow);
            //设置请求的contentLength
            request.ContentLength = payLoad.Length;

            //发送请求，获取请求流
            Stream writer;

            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception e)
            {
                writer = null;
               logger.Info("回传流水数据失败" + e);
            }
            //将请求参数写入流
            writer.Write(payLoad, 0, payLoad.Length);

            writer.Close();//关闭请求流

            //获取返回的数据
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = e.Response as HttpWebResponse;
                //logger.Info("获取响应信息失败，出现异常" + e);
            }

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string recRespose = reader.ReadToEnd();

            reader.Close();

            return recRespose;
        }
    }
}
