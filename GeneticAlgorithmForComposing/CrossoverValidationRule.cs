using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    class CrossoverValidationRule : ValidationRule
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string crossoverValue = value as string;

            double crossover = 0;
            if (double.TryParse(crossoverValue, out crossover) || double.TryParse(crossoverValue, NumberStyles.Any, cultureInfo, out crossover)){
                if ((crossover <= MinValue) || (crossover >= MaxValue)){
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
