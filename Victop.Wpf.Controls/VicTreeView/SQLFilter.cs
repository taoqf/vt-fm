using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Victop.Wpf.Controls
{
    public sealed class SQLFilter
    {
        private const string FilterString = "'|;|cd|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|join|or|--|★";

        private SQLFilter()
        {
        }

        public static bool IntFilter(string stringFilter)
        {
            return Regex.IsMatch(stringFilter, @"^[-]?\d*$");
        }

        public static bool NumFilter(string stringToFilter)
        {
            return Regex.IsMatch(stringToFilter, @"^-?\d*[.]?\d*$");
        }

        public static bool PlusIntFilter(string stringToFilter)
        {
            return Regex.IsMatch(stringToFilter, @"^\d*$");
        }

        public static void ReplaceSqlFilter(ref string stringToReplace)
        {
            foreach (string str in "'|;|cd|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|join|or|--|★".Split(new char[] { '|' }))
            {
                if (stringToReplace.Contains(str))
                {
                    stringToReplace = stringToReplace.Replace(str, "");
                }
            }
        }

        public static bool SqlFilter(string stringToFilter)
        {
            return (string.IsNullOrEmpty(stringToFilter) || "'|;|cd|exec|insert|select|delete|update|count|*|%|chr|mid|master|truncate|char|declare|join|or|--|★".Split(new char[] { '|' }).All<string>(s => !stringToFilter.Contains(s)));
        }
    }
}
