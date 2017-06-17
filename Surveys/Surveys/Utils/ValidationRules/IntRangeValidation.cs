using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Surveys.Utils.ValidationRules
{
    class IntRangeValidation : ValidationRule
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public IntRangeValidation()
        {

        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int minutes = 0;
            var result = false;
            if (value != null && int.TryParse(value.ToString(), out minutes))
            {
                result = minutes >= Min && minutes <= Max;
            }
            return new ValidationResult(result,"");
            
        }
    }
}
