using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    class MeasureValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string measureValue = value as string;

            int measure = 0;
            if (int.TryParse(measureValue, out measure)){
                if ((measure < MinValue) || (measure > MaxValue)){
                    return new ValidationResult(false, "Liczba taktów powinna zawierać się w przedziale od " + MinValue + " do " + MaxValue + ".");
                }
                else {
                    return new ValidationResult(true, null);
                }
            }
            else {
                return new ValidationResult(false, "Podaj liczbę całkowitą!");
            }
        }
    }
}
