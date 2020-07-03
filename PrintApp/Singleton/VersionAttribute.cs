using System;
using System.Collections.Generic;
using System.Text;

namespace PrintApp.Singleton
{
    [System.AttributeUsage(System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    sealed class VersionAttribute : System.Attribute
    {
        public string AppVersion { get; }
        public VersionAttribute(string version)
        {
            this.AppVersion = version;
        }
    }
}
