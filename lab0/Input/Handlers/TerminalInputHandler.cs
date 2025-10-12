using lab0.Input.Validators;

namespace lab0.Input.Handlers;

public class TerminalInputHandler(IInputValidator validator) : IInputHandler
{
    private readonly IInputValidator _validator = validator;

    public string GetAlgorithmChoice()
    {
        string? algorithmChoice = null;

        Console.Write("+ Choice: ");
        while (algorithmChoice == null)
            algorithmChoice = Console.ReadLine();

        return _validator.ValidateAlgorithm(algorithmChoice);
    }

    public List<double> GetNumbers()
    {
        string? numbersInput = null;

        while (string.IsNullOrWhiteSpace(numbersInput))
        {
            Console.Write("+ Numbers: ");
            numbersInput = Console.ReadLine();
        }

        return _validator.ValidateNumbers(numbersInput);
    }
}
