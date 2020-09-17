using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class FtpUntils
    {

        public WebClient webClient;

        private string ftpIp;

        private int ftpPort;

        private string ftpUser;

        private string ftpPassword;

        //初始化ftp服务相关数据
        public FtpUntils(string ftpIp, int ftpPort, string ftpUser, string ftpPassword)
        {
            this.ftpIp = ftpIp;
            this.ftpPort = ftpPort;
            this.ftpUser = ftpUser;
            this.ftpPassword = ftpPassword;
        }

        //上传指定文件下的文件
        public void UploadFile(FileInfo fileInfo, string remotePath)
        {

            WebClient webClient = new WebClient
            {
                Credentials = new NetworkCredential(ftpUser, ftpPassword)
            };

            if (MkDir("ftp://" + Property.FtpIp + "/" + remotePath))
            {

                webClient.UploadFile("ftp://" + Property.FtpIp + "/" + remotePath + fileInfo.Name, fileInfo.FullName);
            }

        }

        //在服务器上创建相应的文件路径
        public bool MkDir(string remotePath)
        {
            if (!CheckPath(remotePath)) {
                return true;
            }
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(remotePath);

            ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);

            ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();

                response.Close();
            }
            catch (Exception e)
            {

                ftpWebRequest.Abort();
                return false;
            }
            ftpWebRequest.Abort();

            return true;
        }

        public bool CheckPath(string remotePath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remotePath);//"ftp://ftp.domain.com/doesntexist.txt"

            request.Credentials = new NetworkCredential(ftpUser, ftpPassword);

            request.Method = WebRequestMethods.Ftp.GetFileSize;
            
            try{
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;

                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)

                {
                    return false;
                }

            }
            return true;
        }
    }
}
