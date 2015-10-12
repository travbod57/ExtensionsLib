using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExtensionsLib.System.Configuration
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method to get the executing path of an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The executing path of the assembly</returns>
        public static string GetExecutingPath(this Assembly assembly)
        {
            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
