using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Silverlight_LOB_Demo_Application.Web.Properties;

namespace Silverlight_LOB_Demo_Application.Web.UploadHandlers
{
    /// <summary>
    /// Summary description for FileUploadReceiver
    /// </summary>
    public class FileUploadReceiver : IHttpHandler
    {
        private readonly string _uploadFolder = Settings.Default.FeedBackUploadFolder; 
        

        public void ProcessRequest(HttpContext context)
        {
            using (FileStream fs = File.Create(FileTools.GetUniqueFilename(_uploadFolder, ".png")))
            {
                FileTools.SaveFile(context.Request.InputStream, fs);
            }
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