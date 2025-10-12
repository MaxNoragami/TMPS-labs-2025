using System.Text.RegularExpressions;

namespace lab1.Input.Validators.NumberInputValidators;

public class RegexInputValidator : INumberInputValidator
{
    public List<double> ValidateNumbers(string input)
    {
        var numberMatches = Regex.Matches(input, @"(?<![A-Za-z0-9.-])-?[0-9]+(\.[0-9]+)?(?![A-Za-z0-9.])");
        
        var numbers = new List<double>();
        foreach (Match match in numberMatches)
            if (match.Success && double.TryParse(match.Value, out var number))
                numbers.Add(number);

        return numbers;
    }
}
