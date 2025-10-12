namespace lab1.Input.Validators;

public interface IInputValidator
{
    List<double> ValidateNumbers(string input);
    string ValidateAlgorithm(string algorithm);
}
