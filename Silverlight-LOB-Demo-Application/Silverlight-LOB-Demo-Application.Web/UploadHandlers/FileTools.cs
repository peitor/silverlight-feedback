using System;
using System.IO;

namespace Silverlight_LOB_Demo_Application.Web.UploadHandlers
{
    public static class FileTools
    {
        public static string GetUniqueFilename(string uploadFolder, string fileExtension)
        {
            string filename = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss-fff");

            filename += fileExtension;

            string filePath = Path.Combine(uploadFolder, filename);

            return filePath;
        }


        public static void SaveFile(Stream inputStream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);

            }

        }

    }
}