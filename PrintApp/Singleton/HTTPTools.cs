using PrintApp.Singleton;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace PrintApp.Singleton
{
    public sealed class HTTPTools
    {
        public static HTTPTools Instance { get; private set; }

        private HTTPTools() { }

        static HTTPTools() { Instance = new HTTPTools(); }

        public bool ValidateURL(string param)
        {
            if (param.StartsWith(Globals.PROTOCOL_HTTP) || param.StartsWith(Globals.PROTOCOL_HTTPS))
            {
                Uri uri = new Uri(Uri.EscapeUriString(param));
                //Uri uri = new Uri(param);

                if (!param.Contains(".pdf"))
                {
                    Globals.Log("VALIDATIONERR: Not a PDF");
                    return false;
                }
                /*
                                if (!param.Contains("mobilegroupinc.com/") &&
                                    !param.Contains("tenantsportal.robinsonsland.com/")
                                    )
                                {
                                    return false;
                                }
                */
                if (param.Contains("&&") ||
                    param.Contains(";") ||
                    param.Contains("@") ||
                    param.Contains("'") ||
                    param.Contains("<") ||
                    param.Contains(">") ||
                    param.Contains("\"") ||
                    param.Contains(" "))
                {
                    Globals.Log("VALIDATIONERR: Illegal character found");
                    return false;
                }

                if (uri.IsWellFormedOriginalString() && Uri.IsWellFormedUriString(
                           Uri.UnescapeDataString(param),
                           UriKind.Absolute))
                {
                    return true;
                }
                else
                {
                    Globals.Log("VALIDATIONERR: Not Well formed string");
                }

            }
            else
            {
                Globals.Log("VALIDATIONERR: Not HTTP/HTTPS");
            }
            return false;

        }

        // API method:
        public string DownloadFile(string fileLink)
        {
            string tmpFullFile;
            try
            {
                string tmpFilePath = Path.GetTempPath();
                string tmpFileName = Path.GetRandomFileName();
                tmpFullFile = Path.Combine(tmpFilePath, tmpFileName);


                WebClient myWebClient = new WebClient();
                Globals.Log($"Downloading File \"{fileLink}\" from \"\" .......\n\n");
                myWebClient.DownloadFile(fileLink, tmpFullFile);
                Globals.Log($"Successfully Downloaded File \"{fileLink}\" from \"\"");
                Globals.Log($"\nDownloaded file as:\n\t" + tmpFullFile);

                throw new Exception();

            }
            catch (Exception ex)
            {
                tmpFullFile = string.Empty;
                Globals.Log($"Failed to download file: {ex.Message}");
            }
            return tmpFullFile;
        }

        public static string ParseQueryString(string url)
        {
            string result = string.Empty;

            string querystring = string.Empty;
            int iqs = url.IndexOf("?");
            if (iqs == -1)
            {
                Globals.Log("ERR:No Query String found");
            }
            else if (iqs >= 0)
            {
                querystring = (iqs < url.Length - 1) ? url.Substring(iqs + 1) : string.Empty;
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);
                foreach (string s in qscoll.AllKeys)
                {
                    Globals.Log($"{s} - {qscoll[s]}");
                    if (s == Globals.PARAM1)
                    {
                        Globals.ParamValue1 = qscoll[s];
                    }
                    else if (s == Globals.PARAM2)
                    {
                        Globals.ParamValue2 = qscoll[s];
                    }

                }
            }

            return result;
        }

        public static void CallPostAPI()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection
                    {
                        { Globals.PARAM1, "1" },
                        { Globals.PARAM2, "1" }
                    };
                    byte[] responsebytes = client.UploadValues(Globals.TAGGINGAPI, "POST", reqparm);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);
                    Console.WriteLine(responsebody);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERR:{ex.Message}");
                }

            }
        }
        public static void CallGetAPI()
        {
            WebClient webClient = new WebClient();
            webClient.QueryString.Add("tenant_id", "1");
            webClient.QueryString.Add("ts_invoice_no", "2");
            try
            {
                string result = webClient.DownloadString("http://mobilegroupinc.com/index.php/tenantportalapi/API/tenants/update_billing_status");
                Console.WriteLine(result);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERR:{ex.Message}");
            }


        }

        public static bool CallTaggingAPI(string tenant_id, string ts_invoice_no)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    Globals.Log($"POST to {Globals.TAGGINGAPI}");
                    Globals.Log($"POST value1 {Globals.PARAM1}={Globals.ParamValue1}");
                    Globals.Log($"POST value2 {Globals.PARAM2}={Globals.ParamValue2}");
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add(Globals.PARAM1, tenant_id);
                    reqparm.Add(Globals.PARAM2, ts_invoice_no);
                    byte[] responsebytes = client.UploadValues(Globals.TAGGINGAPI, "POST", reqparm);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);
                    Globals.Log(responsebody);

                    if (responsebody.Contains(Globals.APISUCCESS))
                        return true;
                    else
                        return false;


                }
                catch (Exception ex)
                {
                    Globals.Log($"ERR:{ex.Message}");
                }
                return false;
            }
        }

    }
}
