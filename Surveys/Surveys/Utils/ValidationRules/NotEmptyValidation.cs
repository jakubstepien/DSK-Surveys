using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Surveys.Utils.ValidationRules
{
    class NotEmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = value != null && !string.IsNullOrWhiteSpace(value.ToString());
            return new ValidationResult(result, "Pole nie może być puste.");
        }
    }
}
