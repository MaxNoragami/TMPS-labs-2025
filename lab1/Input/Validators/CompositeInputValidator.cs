using lab1.Input.Validators.AlgorithmInputValidators;
using lab1.Input.Validators.NumberInputValidators;

namespace lab1.Input.Validators;

public class CompositeInputValidator(
        INumberInputValidator? numberValidator = null, 
        IAlgorithmInputValidator? algorithmValidator = null
    ) : IInputValidator
{
    private readonly INumberInputValidator _numberValidator = numberValidator 
        ?? new DefaultNumberInputValidator();
    private readonly IAlgorithmInputValidator _algorithmValidator = algorithmValidator 
        ?? new DefaultAlgorithmInputValidator();

    public string ValidateAlgorithm(string algorithm)
        => _algorithmValidator.ValidateAlgorithm(algorithm);

    public List<double> ValidateNumbers(string input)
        => _numberValidator.ValidateNumbers(input);
}
