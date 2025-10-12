namespace lab1.Input.Validators.AlgorithmInputValidators;

public class DefaultAlgorithmInputValidator : IAlgorithmInputValidator
{
    public string ValidateAlgorithm(string algorithm)
        => algorithm?.Trim().ToLower() ?? string.Empty;
}
