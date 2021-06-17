using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    //Attribute to validate longitude and latitude (must be added to longitude property)
    public class LatLngAttribute : ValidationAttribute
    {
        private readonly double _latitudeMin;
        private readonly double _latitudeMax;
        private readonly double _longitudeMin;
        private readonly double _longitudeMax;

        public LatLngAttribute(double latitudeMin, double latitudeMax, double longitudeMin, double longitudeMax)
        {
            _latitudeMin = latitudeMin;
            _latitudeMax = latitudeMax;
            _longitudeMin = longitudeMin;
            _longitudeMax = longitudeMax;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var longitude = value as double?;
            double? latitude = (double?)validationContext.ObjectType.GetProperty("Latitude").GetValue(validationContext.ObjectInstance);

            if (latitude != null && longitude != null)
            {
                if (latitude < _latitudeMin || latitude > _latitudeMax || longitude < _longitudeMin || longitude > _longitudeMax)
                {
                    return new ValidationResult(ErrorMessage);
                } else
                {
                    return ValidationResult.Success;
                }
            } else
            {
                return new ValidationResult("Location is required");
            }
        }
    }
}
