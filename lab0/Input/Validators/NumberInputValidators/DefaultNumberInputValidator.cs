namespace lab0.Input.Validators.NumberInputValidators;

public class DefaultNumberInputValidator : INumberInputValidator
{
    public List<double> ValidateNumbers(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        var sanitizedInput = input.Trim().Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);

        var numbers = new List<double>();
        foreach (var element in sanitizedInput)
            if (double.TryParse(element, out var number))
                numbers.Add(number);

        return numbers;
    }
}
