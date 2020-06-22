using System;
using System.Collections.Generic;
using System.Text;

namespace PrintApp.Singleton
{
    public sealed class Globals
    {
        public static Globals Instance { get; private set; }

        public static string FileToPrint { get; set; } = string.Empty;
        public static string URLToFile { get; set; } = string.Empty;
        public static string PrinterToUse { get; set; } = string.Empty;

        public static string PROTOCOL_APP = "ptapp://";
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
        }


    }
}
