﻿using System;

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

                string param = fileLink;
#if DEBUG                
                param = fileLink.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTP);
#else
                param = param.Replace(Globals.PROTOCOL_APP, Globals.PROTOCOL_HTTPS);
#endif
                

                //Globals.TAGGINGAPI = "http://mobilegroupinc.com/index.php/tenantportalapi/API/tenants/update_billing_status";

                if (HTTPTools.Instance.ValidateURL(param))
                {
                    Uri uri = new Uri(param);

                    //Globals.TAGGINGAPI = uri.Scheme + Uri.SchemeDelimiter + uri.Host + Globals.TAGGINGAPI_PATH;
                    Globals.TAGGINGAPI = uri.GetLeftPart(UriPartial.Authority) + Globals.TAGGINGAPI_PATH;
                    Globals.Log($"Constructed Tagging API as {Globals.TAGGINGAPI}");

                    Globals.Log($"Found file {param}");
                    //Globals.Log($"Stripped:{param}");
                    string filename = System.IO.Path.GetFileName(uri.LocalPath);
                    Globals.Log($"To download file: {param}");
                    Globals.URLToFile = param;
                    Globals.Log($"Storing to Globals: {Globals.URLToFile}");
#if _WINDOWS
                    Globals.FileStreamToPrint = HTTPTools.Instance.DownloadFileStream(Globals.URLToFile);
                    Globals.FileToPrint = "MEMORY BUFFER";
#else
                    Globals.FileToPrint = HTTPTools.Instance.DownloadFile(Globals.URLToFile);
#endif
                    Globals.Log($"Processed: {Globals.FileToPrint}");

                    return true;
                }
                else
                {
                    Globals.OK = false;
                    Globals.Message = "BAD URI";
                    Globals.Log($"Err:Bad URI");
                }
            }
            catch (Exception e)
            {
                Globals.OK = false;
                Globals.Message = "RETRIEVAL PROBLEM:"+ e.Message;
                Globals.Log($"Exception: {e.Message}");
            }
            return false;

        }
    }
}
