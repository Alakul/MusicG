using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    public class MutationValidationRule : ValidationRule
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string mutationValue = value as string;

            double mutation = 0;
            if (double.TryParse(mutationValue, out mutation) || double.TryParse(mutationValue, NumberStyles.Any, cultureInfo, out mutation))
            {
                if ((mutation <= MinValue) || (mutation >= MaxValue)){
                    return new ValidationResult(false, "Prawdopodobienstwo powinno zawierać się w przedziale od " + MinValue + " do " + MaxValue + ".");
                }
                else {
                    return new ValidationResult(true, null);
                }
            }
            else {
                return new ValidationResult(false, "Podaj liczbę rzeczywistą dodatnią!");
            }
        }
    }
}
