using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class Globals
    {

        public static Globals Instance { get; private set; }

        public static string FileToPrint { get; set; } = string.Empty;
        public static Stream FileStreamToPrint { get; set; }
        public static string URLToFile { get; set; } = string.Empty;
        public static string PrinterToUse { get; set; } = string.Empty;

        public static string PROTOCOL_APP = "rtenant-portal://";
        public static string PROTOCOL_HTTP = "http://";
        public static string PROTOCOL_HTTPS = "https://";

        public static string PRINTJOB_NAME = "Billing";
#if DEBUG
        //public static string TAGGINGAPI = "http://mobilegroupinc.com/index.php/tenantportalapi/API/tenants/update_billing_status";
        public static string TAGGINGAPI = string.Empty;
        public static string TAGGINGAPI_PATH = "/index.php/tenantportalapi/API/tenants/update_billing_status";

#else
        //public static string TAGGINGAPI = "http://10.88.42.41/tenantportalapi/API/tenants/update_billing_status";
        //public static string TAGGINGAPI = "https://tenantsportal.robinsonsland.com/tenantportalapi/API/tenants/update_billing_status";
        public static string TAGGINGAPI = string.Empty;
        public static string TAGGINGAPI_PATH = "/tenantportalapi/API/tenants/update_billing_status";
#endif
        
        public static string LIVEDOMAIN = "tenantsportal.robinsonsland.com";
        //public static string STAGINGDOMAIN = "10.88.42.41";
        public static string STAGINGDOMAIN = "rlcmlappv2qas01.robinsonsland.com";
        public static string TESTDOMAIN = "mobilegroupinc.com";
        

        public static string PARAM1 = "tenant_id";
        public static string PARAM2 = "ts_invoice_no";
        public static string PARAMVERSION = "v";

        public static string ParamValue1 { get; set; } = string.Empty;
        public static string ParamValue2 { get; set; } = string.Empty;
        public static string ParamVersion { get; set; } = string.Empty;

#if DEBUG
        public static string APISUCCESS = "";
#else
        public static string APISUCCESS = "\"success\"";
#endif

        public static string LOGPATH_DEBUG_OSX = "/Users/mobilegroupinc/Desktop/prt/log-.txt";
        public static string LOGPATH_DEBUG_WIN = "C:/Kit/log-.txt";

        public static int SLEEPTIMER = 10000;
        public static int PROCTIMER = 10000;

        public static string[] BADPRINTERS =
        {
#if !DEBUG            
            "pdf",
            "xps",
            "onenote"
#endif
        };

        public static int PRINTER_STATUS_OFFLINE = 0x80;
        public static int PRINTER_STATUS_ERROR = 2;
        public static int PRINTER_STATUS_PAPER_JAM = 8;
        public static int PRINTER_STATUS_PAPER_OUT = 0x10;
        public static int PRINTER_STATUS_NOT_AVAILABLE = 0x1000;
        public static int PRINTER_STATUS_NO_TONER = 0x40000;
        public static int PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x400;

        public static string Message { get; set; }
        public static bool OK { get; set; } = true;

        private Globals() { }
        static Globals()
        {
            Instance = new Globals();
        }

        public static void Log(string text)
        {
            Console.WriteLine(text);
            Serilog.Log.Information(text);
        }

        public static bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
        public static bool IsOSX()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }
        public static bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        

        public static DateTime GetBuildDate(Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }
            }

            return default;
        }

        public static void DestroyFile()
        {
            try
            {
#if _WINDOWS
                Globals.FileStreamToPrint?.Dispose();
#else
                File.Delete(Globals.FileToPrint);
#endif
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
