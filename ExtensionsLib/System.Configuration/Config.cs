using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExtensionsLib.System.Configuration
{
    /// <summary>
    /// A config helper class which deals with fetching values from the appSettings section of the app or web config file
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Returns true if a key exists in the application config file
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>True if the key exists</returns>
        public static bool KeyExists(string key)
        {
            return !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// Gets a string value given its key
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>The config value</returns>
        public static string TryGetStringValue(string key)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                return ConfigurationManager.AppSettings[key];
            else
                return String.Empty;
        }

        /// <summary>
        /// Gets a string value given its key
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>The config value</returns>
        public static string GetStringValue(string key)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                throw new ConfigurationErrorsException("Could not find an appSettings key called '" + key + "' in the application config file.");

            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Gets an integer value given its key
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>The config value</returns>
        public static int GetIntValue(string key)
        {
            var stringValue = GetStringValue(key);
            int val = 0;
            if (!Int32.TryParse(stringValue, out val))
                throw new ConfigurationErrorsException(string.Format("The value '{0}' for appSettings key '{1}' is not a valid integer.",
                    stringValue, key));

            return val;
        }

        /// <summary>
        /// Gets a boolean value given its key
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>The config value</returns>
        public static bool GetBoolValue(string key)
        {
            var stringValue = GetStringValue(key);
            bool val = false;
            if (!Boolean.TryParse(stringValue, out val))
                throw new ConfigurationErrorsException(string.Format("The value '{0}' for appSettings key '{1}' is not a valid boolean.",
                    stringValue, key));

            return val;
        }

        /// <summary>
        /// Gets the server path for a file. The path is expected to be relative to the web root folder
        /// </summary>
        /// <param name="key">The appSettings key</param>
        /// <returns>The config value</returns>
        public static string GetServerPath(string key)
        {
            string value = GetStringValue(key);

            if (value[0] == '\\' || value[0] == '/')
                value = value.Substring(1, value.Length - 2);

            string path = String.Empty;

            if (value.StartsWith("C:"))
                path = value;
            else
                path = Assembly.GetExecutingAssembly().GetExecutingPath() + @"\..\" + value;

            if (!File.Exists(path))
                throw new ConfigurationErrorsException(string.Format("The file '{0}' could not be found using appSettings key '{1}'",
                    path, key));

            return path;
        }
    }
}
