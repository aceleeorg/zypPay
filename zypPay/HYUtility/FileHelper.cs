using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace HYUtility
{
    public class StreamStaticDataSource : IStaticDataSource
    {
        private Stream _stream;

        public StreamStaticDataSource(Stream stream)
        {
            _stream = stream;
        }

        #region IStaticDataSource Members

        public Stream GetSource()
        {
            return _stream;
        }

        #endregion
    }
    public class FileHelper
    {
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));//两位16进制
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return null;//
               // throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 备份指定文件夹下指定文件到指定目录
        /// </summary>
        /// <param name="filePath">文件夹</param>
        /// <param name="fileNames">文件名，多个文件名以;分隔</param>
        /// <param name="backupName">备份后的名称</param>
        /// <param name="backupPath">备份到的文件夹</param>
        /// <returns></returns>
        public static string BackUpFiles(string filePath,string fileNames,string backupName, string backupPath)
        {
            //bool flag = false;

            //准备写入内容
            //MemoryStream contentStream = new MemoryStream();
            //StreamWriter contentWriter = new StreamWriter(contentStream);
          
            //contentWriter.Flush();
            //contentStream.Seek(0, SeekOrigin.Begin);

            string zipFilename =backupPath+"\\"+ backupName+".zip";

            FileStream zipStream = null;
            ZipFile zipFile = null;
            try
            {
                if (!File.Exists(zipFilename))
                {
                    zipStream = File.Open(zipFilename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    zipFile = ZipFile.Create(zipStream);
                }
                else
                {
                    zipStream = File.Open(zipFilename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    zipFile = new ZipFile(zipStream);
                }
                zipFile.BeginUpdate();
                foreach (string fileName in fileNames.Split(';'))
                {
                    string fullName = filePath + "\\" + fileName;
                    zipFile.Add(fullName);
                }

                //zipFile .Add (
                //zipFile.Add(new StreamStaticDataSource(contentStream), DateTime.Now.ToString("HHmmss") + ".txt");
            }
            catch
            { 
            
            }
            finally
            {
                if (zipFile != null)
                {
                    if (zipFile.IsUpdating)
                        zipFile.CommitUpdate();
                    zipFile.Close();
                }
                if (zipStream != null)
                    zipStream.Close();
            }


            return GetMD5HashFromFile(zipFilename);
        }
    }
}
