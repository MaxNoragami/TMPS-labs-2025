
using lab1.Input.Validators;

namespace lab1.Input.Handlers;

public class CommandInputHandler(IInputValidator validator, string[] args) : IInputHandler
{
    private readonly IInputValidator _validator = validator;
    private readonly string[] _args = args;

    public string GetAlgorithmChoice()
    {
        var algorithm = GetArgumentValue("--sort-algorithm", "-s");
        return _validator.ValidateAlgorithm(algorithm ?? string.Empty);
    }

    public List<double> GetNumbers()
    {
        var input = GetArgumentValue("--input", "-i");
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Numbers input is required. Use --input or -i");

        return _validator.ValidateNumbers(input);
    }

    private string? GetArgumentValue(string longForm, string shortForm)
    {
        for (int i = 0; i < _args.Length - 1; i++)
            if (_args[i].Equals(longForm, StringComparison.OrdinalIgnoreCase) ||
                _args[i].Equals(shortForm, StringComparison.OrdinalIgnoreCase))
            {
                return _args[i + 1];
            }
        return null;
    }
}
