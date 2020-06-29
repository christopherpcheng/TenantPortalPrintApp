using PrintApp.Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class HTTPTools
    {
        public static HTTPTools Instance { get; private set; }

        private HTTPTools() { }

        static HTTPTools() { Instance = new HTTPTools(); }

        // API method:
        public string DownloadFile(string fileLink)
        {
            string tmpFullFile = string.Empty;
            try
            {
                string tmpFilePath = Path.GetTempPath();
                string tmpFileName = Path.GetRandomFileName();
                tmpFullFile = Path.Combine(tmpFilePath, tmpFileName);


                WebClient myWebClient = new WebClient();
                Globals.Log($"Downloading File \"{fileLink}\" from \"\" .......\n\n");
//                myWebClient.DownloadFile(fileLink, tmpFullFile);
                Globals.Log($"Successfully Downloaded File \"{fileLink}\" from \"\"");
                Globals.Log($"\nDownloaded file as:\n\t" + tmpFullFile);

            }
            catch (Exception ex)
            {
                Globals.Log($"Failed to download file: {ex.Message}");
            }
            return tmpFullFile;
        }
    }
}
