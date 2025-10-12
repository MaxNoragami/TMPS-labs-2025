using lab0.Input.Validators.AlgorithmInputValidators;
using lab0.Input.Validators.NumberInputValidators;

namespace lab0.Input.Validators;

public interface IInputValidator : INumberInputValidator, IAlgorithmInputValidator { }
