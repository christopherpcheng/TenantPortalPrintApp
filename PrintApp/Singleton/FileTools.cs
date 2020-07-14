using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class FileTools
    {
        public static FileTools Instance { get; private set; }

        private FileTools() { }

        static FileTools() { Instance = new FileTools(); }

        // API method:
        public bool ProcessLink(string fileLink)
        {

            try
            {
                Globals.Log($"arg: {fileLink}");
//#if DEBUG
                string param = fileLink.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTP);
//#endif
//#if !DEBUG
                param = param.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTPS);
//#endif

#if DEBUG
                if (!param.Contains(Globals.STAGINGDOMAIN))
                {
                    Globals.TAGGINGAPI = Globals.TAGGINGAPI.Replace(Globals.LIVEDOMAIN, Globals.STAGINGDOMAIN);

                }
#endif                


                if (HTTPTools.Instance.ValidateURL(param))
                {
                    Uri uri = new Uri(param);

                    Globals.Log($"Found file {param}");
                    //Globals.Log($"Stripped:{param}");
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    Globals.Log($"To download file: {param}");
                    Globals.URLToFile = param;
                    Globals.Log($"Storing to Globals: {Globals.URLToFile}");
                    Globals.FileToPrint = HTTPTools.Instance.DownloadFile(Globals.URLToFile);
                    Globals.Log($"Processed: {Globals.FileToPrint}");

                    return true;
                }
                else
                {
                    Globals.Log($"Err:Bad URI");
                }
            }
            catch (Exception e)
            {
                Globals.Log($"Exception: {e.Message}");
            }
            return false;

        }
    }
}
