using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Silverlight_LOB_Demo_Application.Web.UploadHandlers
{
    /// <summary>
    /// Summary description for FileUploadReceiver
    /// </summary>
    public class FileUploadReceiver : IHttpHandler
    {
         private const string UploadFolder = @"C:\temp\Upload";


        public void ProcessRequest(HttpContext context)
        {
            using (FileStream fs = File.Create(FileTools.GetUniqueFilename(UploadFolder, ".png")))
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