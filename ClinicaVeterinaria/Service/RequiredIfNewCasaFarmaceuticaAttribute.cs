using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Attributes
{
    public class RequiredIfNewCasaFarmaceutica : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequiredIfNewCasaFarmaceutica(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Proprietà {_comparisonProperty} non trovata.");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (comparisonValue != null && !string.IsNullOrEmpty(comparisonValue.ToString()) && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
