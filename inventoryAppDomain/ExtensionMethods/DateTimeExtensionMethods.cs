using System;
using System.Threading;

namespace inventoryAppDomain.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime dt) =>
            dt.FirstDayOfWeek().AddDays(6);
    }
}