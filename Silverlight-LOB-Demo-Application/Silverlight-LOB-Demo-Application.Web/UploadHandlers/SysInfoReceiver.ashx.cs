using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Silverlight_LOB_Demo_Application.Web.UploadHandlers
{
    /// <summary>
    /// Summary description for SysInfoReceiver
    /// </summary>
    public class SysInfoReceiver : IHttpHandler
    {
        private const string UploadFolder = @"C:\temp\Upload";



        public void ProcessRequest(HttpContext context)
        {
            MemoryStream memoryStream = new MemoryStream();

            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memoryStream.Write(buffer, 0, bytesRead);
            }

            UnicodeEncoding encoding = new UnicodeEncoding();
            string content = encoding.GetString(memoryStream.ToArray());


            //TODO: if email valid, send email we received your message

            
            File.WriteAllText(FileTools.GetUniqueFilename(UploadFolder, ".txt"), content);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}