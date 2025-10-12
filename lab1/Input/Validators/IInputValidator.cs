using lab1.Input.Validators.AlgorithmInputValidators;
using lab1.Input.Validators.NumberInputValidators;

namespace lab1.Input.Validators;

public interface IInputValidator : INumberInputValidator, IAlgorithmInputValidator { }
