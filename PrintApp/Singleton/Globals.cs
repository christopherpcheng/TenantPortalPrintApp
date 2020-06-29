using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class Globals
    {
        public static Globals Instance { get; private set; }
        public static ListBox DebugListBox { get; set; }

        public static string FileToPrint { get; set; } = string.Empty;
        public static string URLToFile { get; set; } = string.Empty;
        public static string PrinterToUse { get; set; } = string.Empty;

        public static string PROTOCOL_APP = "rtenant-portal://";
        public static string PROTOCOL_HTTP = "http://";
        public static string PROTOCOL_HTTPS = "https://";

        public static string[] BADPRINTERS =
        {
#if !DEBUG            
            "pdf",
            "xps",
            "onenote"
#endif
        };

        private Globals() { }
        static Globals()
        {
            Instance = new Globals();
        }

        public static void Log(string text)
        {
            Console.WriteLine(text);
            if (DebugListBox != null)
            {
                DebugListBox.Items.Add("");
            }

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


    }
}
