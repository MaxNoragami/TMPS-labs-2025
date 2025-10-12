string? numbersInput = null;

while (string.IsNullOrWhiteSpace(numbersInput))
    numbersInput = Console.ReadLine();

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

Console.WriteLine("Choose a sorting algorithm:\nBubble\nInsertion\nSelection");

string? algorithmChoice = null;

while (algorithmChoice == null)
    algorithmChoice = Console.ReadLine()?.Trim().ToLower();

var result = new List<double>();
switch (algorithmChoice)
{
    case "bubble":
        result = BubbleSort(numbers);
        break;
    case "insertion":
        result = InsertionSort(numbers);
        break;
    case "selection":
        result = SelectionSort(numbers);
        break;
    default:
        Console.WriteLine("Such algorithm is not implemented yet");
        break;
}

Console.WriteLine("Result: {0}", string.Join(", ", result));


List<double> BubbleSort(List<double> numbers)
{
    Console.WriteLine("BubbleSort was used!");
    return numbers;
}

List<double> InsertionSort(List<double> numbers)
{
    Console.WriteLine("InsertionSort was used!");
    return numbers;
}
List<double> SelectionSort(List<double> numbers)
{
    Console.WriteLine("SelectionSort was used!");
    return numbers;
}