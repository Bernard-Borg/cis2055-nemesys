using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    //Attribute to prevent dates which are not in a valid range from being submitted (investigation/reports)
    public class DateRangeAttribute : ValidationAttribute
    {
        private static readonly TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        private static readonly DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);

        private static readonly string minimumDate = dateTime.AddYears(-1).ToString("yyyy-MM-dd HH:mm");
        private static readonly string maximumDate = dateTime.ToString("yyyy-MM-dd HH:mm");

        public DateRangeAttribute()
        {
            ErrorMessage = $"Date should be between {minimumDate} and {maximumDate}";
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var date = value as DateTime?;

            if (date == null)
            {
                return new ValidationResult("Date is required");
            }

            if (date < dateTime.AddYears(-1) || date > dateTime)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}