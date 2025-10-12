using lab1;
using lab1.SortingStrategies;

string? numbersInput = null;

while (string.IsNullOrWhiteSpace(numbersInput))
{
    Console.Write("+ Numbers: ");
    numbersInput = Console.ReadLine();
}

var sanitizedInput = numbersInput.Trim().Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);

var numbers = new List<double>();
foreach (var element in sanitizedInput)
{
    var isNum = double.TryParse(element, out var number);
    if (!isNum)
        continue;
    numbers.Add(number);
}
    
Console.WriteLine(string.Join(",", numbers));

Console.WriteLine("Choose a sorting algorithm:\nBubble\nInsertion\nSelection\nQuick");

string? algorithmChoice = null;

Console.Write("+ Choice: ");
while (algorithmChoice == null)
    algorithmChoice = Console.ReadLine()?.Trim().ToLower();

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

Console.WriteLine("Result: {0}", string.Join(", ", result));