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

var sortingService = new SortingService();

switch (algorithmChoice)
{
    case "bubble":
        Console.WriteLine("Using BubbleSort...");
        sortingService.SetNumberSortAlgo(new BubbleSort());
        break;
    case "insertion":
        Console.WriteLine("Using InsertionSort...");
        sortingService.SetNumberSortAlgo(new InsertionSort());
        break;
    case "selection":
        Console.WriteLine("Using SelectionSort...");
        sortingService.SetNumberSortAlgo(new SelectionSort());
        break;
    case "quick":
        Console.WriteLine("Using QuickSort...");
        sortingService.SetNumberSortAlgo(new QuickSort());
        break;
    default:
        Console.WriteLine("Using DefaultSort algorithm...");
        break;
}

var result = sortingService.Sort(numbers);

Console.WriteLine($"Result: {string.Join(", ", result)}");
