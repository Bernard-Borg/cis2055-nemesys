using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateRangeAttribute : RangeAttribute
    {
        private static string minimumDate = DateTime.UtcNow.AddYears(-1).ToString();
        private static string maximumDate = DateTime.UtcNow.ToString();

        public DateRangeAttribute(): base(typeof(DateTime), minimumDate, maximumDate)
        {
            ErrorMessage = $"Date should be between {minimumDate} and {maximumDate}";
        }
    }
}
