using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionsLib.System
{
    public static class DateTimeExtensions
    {
        public static int Compare(string dateTime1_AsString, string dateTime2_AsString)
        {
            DateTime dateTime1, dateTime2;
            DateTime.TryParse(dateTime1_AsString, out dateTime1);
            DateTime.TryParse(dateTime2_AsString, out dateTime2);
            int result = DateTime.Compare(dateTime1, dateTime2);
            return result;
        }
    }
}
