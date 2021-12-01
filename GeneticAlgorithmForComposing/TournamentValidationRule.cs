using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeneticAlgorithmForComposing
{
    class TournamentValidationRule : ValidationRule
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string tournamentValue = value as string;
            
            int tournament = 0;
            if (int.TryParse(tournamentValue, out tournament)){
                if ((tournament < MinValue) || (tournament > MaxValue)){
                    return new ValidationResult(false, "Rozmiar turnieju powinien zawierać się w przedziale od " + MinValue + " do " + MaxValue + ".");
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
