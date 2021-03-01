using System;
using System.ComponentModel.DataAnnotations;

namespace TciPM.Blazor.Shared.Utils
{
    public class ValidArrayAttribute : ValidationAttribute
    {
        public string[] ValidValues { get; set; }
        public string[] InvalidValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!IsValid(value))
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            return null;
        }

        public override bool IsValid(object value)
        {
            string strVal = value.ToString();
            if (InvalidValues != null && InvalidValues.Length > 0)
            {
                foreach (var item in InvalidValues)
                    if (item.Equals(strVal))
                        return false;
            }
            if (ValidValues != null && ValidValues.Length > 0)
            {
                bool found = false;
                foreach (var item in ValidValues)
                {
                    if (item.Equals(strVal))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return false;
            }
            return true;
        }
    }
}
