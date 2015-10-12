using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ExtensionsLib.System.Data.SqlServer.SqlDataReaderExtensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Extension method for DateTime.GetValueOrDefault() to get a Minimum DateTime value of 1/1/1950
        /// </summary>
        /// <param name="dateTime">The DateTime</param>
        /// <returns>The value in the given param, or 1/1/1950</returns>
        public static DateTime GetValueOrSQLMinValue(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value;
            }
            else
            {
                return SQLMinValue;
            }
        }

        /// <summary>
        /// Extension method for DateTime.GetValueOrDefault() to get a Minimum DateTime value of 1/1/1950
        /// </summary>
        /// <param name="dateTime">The DateTime</param>
        /// <returns>The value in the given param, or 1/1/1950</returns>
        public static DateTime SQLMinValue
        {
            get
            {
                return DateTime.ParseExact("01/01/1950", "d", new CultureInfo("en-GB"));
            }
        }
    }

    public static class SqlDataReaderExtensions
    {
        /// <summary>
        /// Extension method for getting an Int32 given the field name
        /// </summary>
        /// <param name="dr">The SqlDataReader</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>Int32 for the given field name</returns>
        public static Int32 GetInt32(this SqlDataReader dr, string fieldName)
        {
            try
            {
                return dr.GetInt32(dr.GetOrdinal(fieldName));
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is IndexOutOfRangeException || ex is SqlNullValueException)
                    return 0;
                else
                    throw ex;
            }
        }

        /// <summary>
        /// Extension method for getting a nullable DateTime given the field name
        /// </summary>
        /// <param name="dr">The SqlDataReader</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>Nullable DateTime for the given field name</returns>
        public static DateTime? GetDateTime(this SqlDataReader dr, string fieldName)
        {
            try
            {
                string value = dr[fieldName].ToString();
                DateTime dateTime;
                DateTime.TryParse(value, out dateTime);
                if (dateTime == DateTimeExtensions.SQLMinValue)
                    return null;
                else
                    return dateTime;
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is IndexOutOfRangeException || ex is SqlNullValueException)
                    return null;
                else
                    throw ex;
            }
        }

        /// <summary>
        /// Extension method for getting a DateTime represented as a String given the field name
        /// </summary>
        /// <param name="dr">The SqlDataReader</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>String for the given DateTime field name</returns>
        public static String GetDateTimeAsString(this SqlDataReader dr, string fieldName)
        {
            try
            {
                string fieldValueAsString = dr[fieldName].ToString();
                if (String.IsNullOrEmpty(fieldValueAsString))
                    return null;
                else
                {
                    DateTime? fieldValueAsDateTime = GetDateTime(dr, fieldName); // dr.GetDateTime(dr.GetOrdinal(fieldName));
                    if (fieldValueAsDateTime == null)
                        fieldValueAsString = null;
                    return fieldValueAsString;
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is IndexOutOfRangeException || ex is SqlNullValueException)
                    return null;
                else
                    throw ex;
            }
        }

        /// <summary>
        /// Extension method for getting a Decimal given the field name
        /// </summary>
        /// <param name="dr">The SqlDataReader</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>Decimal for the given field name</returns>
        public static Decimal GetDecimal(this SqlDataReader dr, string fieldName)
        {
            try
            {
                return dr.GetDecimal(dr.GetOrdinal(fieldName));
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is IndexOutOfRangeException || ex is SqlNullValueException)
                    return 0;
                else
                    throw ex;
            }
        }

        /// <summary>
        /// Extension method for getting a Boolean given the field name
        /// </summary>
        /// <param name="dr">The SqlDataReader</param>
        /// <param name="fieldName">The field name</param>
        /// <returns>Nullable Boolean for the given field name</returns>
        public static Boolean? GetBoolean(this SqlDataReader dr, string fieldName)
        {
            try
            {
                return dr.GetBoolean(dr.GetOrdinal(fieldName));
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is IndexOutOfRangeException || ex is SqlNullValueException)
                    return null;
                else
                    throw ex;
            }
        }
    }
}
