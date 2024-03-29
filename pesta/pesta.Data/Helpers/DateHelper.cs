﻿using System;
using System.Globalization;

namespace pesta.Data.Model.Helpers
{
    public static class DateHelper
    {
        private static string[] formats = new string[0];
        private const string format = "yyyy-MM-dd'T'HH:mm:ss.fffK";

        public static string Rfc3339DateTimeFormat
        {
            get
            {
                return format;
            }
        }

        public static string[] Rfc3339DateTimePatterns
        {
            get
            {
                if (formats.Length > 0)
                {
                    return formats;
                }
                formats = new string[11];

                // Rfc3339DateTimePatterns
                formats[0] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK";
                formats[1] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffK";
                formats[2] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffK";
                formats[3] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffK";
                formats[4] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK";
                formats[5] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffK";
                formats[6] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fK";
                formats[7] = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";

                // Fall back patterns
                formats[8] = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK"; // RoundtripDateTimePattern
                formats[9] = DateTimeFormatInfo.InvariantInfo.UniversalSortableDateTimePattern;
                formats[10] = DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern;

                return formats;
            }
        }

        public static DateTime Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            DateTime result;
            if (TryParse(s, out result))
            {
                return result;
            }
            throw new FormatException(String.Format(null, "{0} is not a valid RFC 3339 string representation of a date and time.", s));
        }
        public static string ToString(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("utcDateTime");
            }

            return utcDateTime.ToString(Rfc3339DateTimeFormat, DateTimeFormatInfo.InvariantInfo);
        }
       
        public static bool TryParse(string s, out DateTime result)
        {
            bool wasConverted = false;
            result = DateTime.MinValue;

            if (!String.IsNullOrEmpty(s))
            {
                DateTime parseResult;
                if (DateTime.TryParseExact(s, Rfc3339DateTimePatterns, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal, out parseResult))
                {
                    result = DateTime.SpecifyKind(parseResult, DateTimeKind.Utc);
                    wasConverted = true;
                }
            }

            return wasConverted;
        }
    }
}