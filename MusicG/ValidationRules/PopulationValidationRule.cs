using System.Globalization;
using System.Windows.Controls;

namespace MusicG
{
    public class PopulationValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string populationSize = value as string;

            int population = 0;
            if (int.TryParse(populationSize, out population)){
                if ((population < MinValue) || (population > MaxValue)){
                    return new ValidationResult(false, "Liczba osobników w populacji powinna zawierać się w przedziale od " + MinValue + " do " + MaxValue + ".");
                }
                else {
                    if (population % 2 != 0){
                        return new ValidationResult(false, "Podaj liczbę parzystą!");
                    }
                    else {
                        return new ValidationResult(true, null);
                    }
                }
            }
            else{
                return new ValidationResult(false, "Podaj liczbę całkowitą!");
            }
        }
    }
}
