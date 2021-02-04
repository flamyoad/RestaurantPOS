using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlamyPOS.Utilities
{
    public class ValidateCurrency : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex rgx = new Regex(@"/^[0-9]+.{1}[0-9]{1,2}$/");

            var str = value as string;
            if (str == null)
            {
                return new ValidationResult(false, "No data");
            }
            else if(rgx.IsMatch(str))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Invalid format");
            }
        }
    }
}
