using System;
using System.Collections.Generic;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class FileTools
    {
        public static FileTools Instance { get; private set; }

        private FileTools() { }

        static FileTools() { Instance = new FileTools(); }

        // API method:
        public void ProcessLink(string fileLink)
        {

            try
            {
                Globals.Log($"arg: {fileLink}");
#if DEBUG
                string param = fileLink.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTP);
#endif
#if !DEBUG
                string param = fileLink.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTPS);
#endif
                Uri uri = new Uri(param);
                //if (u.IsFile)
                //if (uri.IsFile)
                {
                    Globals.Log($"Found file {param}");
                    //Globals.Log($"Stripped:{param}");
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    Globals.Log($"To download file: {param}");
                    Globals.URLToFile = param;
                    Globals.Log($"Storing to Globals: {Globals.URLToFile}");
                    Globals.FileToPrint = HTTPTools.Instance.DownloadFile(Globals.URLToFile);
                    Globals.Log($"Processed: {Globals.FileToPrint}");
                }
            }
            catch (Exception e)
            {
                Globals.Log($"Exception: {e.Message}");
            }

        }
    }
}
