using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    class GenerationValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        //Rozmiar podzielny przez 2
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string generationsValue = value as string;

            int generations = 0;
            if (int.TryParse(generationsValue, out generations)){
                if ((generations < MinValue) || (generations > MaxValue)){
                    return new ValidationResult(false, "Liczba generacji powinna zawierać się w przedziale od " + MinValue + " do " + MaxValue + ".");
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
