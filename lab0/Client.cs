using lab0.Input.Handlers;
using lab0.Input.Validators;
using lab0.Input.Validators.NumberInputValidators;
using lab0.Sorting;

var validator = new CompositeInputValidator(numberValidator: new RegexInputValidator());

IInputHandler inputHandler = args.Length > 0
    ? new CommandInputHandler(validator, args)
    : new TerminalInputHandler(validator);

var numbers = inputHandler.GetNumbers();
Console.WriteLine($"Identified numbers: {string.Join(", ", numbers)}");

if (inputHandler is TerminalInputHandler)
    Console.WriteLine("Choose a sorting algorithm:\n\tBubble\n\tInsertion\n\tSelection\n\tQuick");

var algorithmChoice = inputHandler.GetAlgorithmChoice();

var factory = new SortAlgorithmFactory();
var sortingService = new SortingService();

var (usedAlgorithm, algorithm) = factory.CreateAlgorithm(algorithmChoice);
sortingService.SetNumberSortAlgo(algorithm);

Console.WriteLine($"Using {usedAlgorithm} sort...");

var result = sortingService.Sort(numbers);

Console.WriteLine($"Result: {string.Join(", ", result)}");
