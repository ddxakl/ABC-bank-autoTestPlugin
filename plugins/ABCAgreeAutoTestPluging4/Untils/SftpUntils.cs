using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABCAgreeAutoTestPluging4.CommonProperty;
using Renci.SshNet;

namespace ABCAgreeAutoTestPluging4.Untils
{
    class SftpUntils
    {

        public static SftpClient sftpClient;

        public SftpUntils(string SftpIp,int SftpPort,string SftpUser,string SftpPassword) {
            sftpClient = new SftpClient(SftpIp, SftpPort, SftpUser, SftpPassword);

        }
        
        public bool Connected { get { return sftpClient.IsConnected; } }
        //连接服务器sftp
        public bool ConnectSftp()
        {
            try
            {
                if (!Connected)
                {
                    sftpClient.Connect();
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("连接SFTP失败，原因：{0}", ex.Message));

            }

        }
        //检查创建路径
        public void CreatDir(string remotePath)
        {
            //sftpClient.ChangeDirectory("/home/tomcat/");
            //sftpClient.ChangeDirectory(SSATProperty.severRootPath);
            
            string[] dirs = remotePath.Split('/');

            for (int i = 0; i < dirs.Length; i++)
            {
                string currentDir = sftpClient.WorkingDirectory;//初始化默认为：/home/tomcat

                string targetDir = currentDir + "/" + dirs[i];

                this.CheckDir(targetDir);

                sftpClient.ChangeDirectory(targetDir);

            }

        }

        //检查路径是否存在,如果不存在就新建路径
        public void CheckDir(string targetDir)
        {
            try
            {
                if (!sftpClient.Exists(targetDir))
                {
                    sftpClient.CreateDirectory(targetDir);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("新建路径失败，原因：{0}", ex.Message));
            }
        }
        //将本地文件上传
        private bool UpLoad(string localPath)
        {
            //将路径切换到需要上传的路劲下
            //sftpClient.ChangeDirectory(remotePath);
            
            FileStream fileStream = new FileStream(localPath, FileMode.Open);

            sftpClient.BufferSize = 4 * 1024;

            sftpClient.UploadFile(fileStream, Path.GetFileName(localPath));

            return true;
        }
        /// <summary>
        /// 传入参数上传至服务器指定路径下
        /// </summary>
        /// <param name="localPath">包含文件信息名的路径信息</param>
        /// <param name="remotePath">传入的路径为sftp服务器默认路径之后的路径</param>
        public void UpLoadFile(string localPath, string remotePath)
        {
            
            this.ConnectSftp();

            this.CreatDir(remotePath);//传入的路径为默认路径之后的路径

            this.UpLoad(localPath);
        }
    }
}
