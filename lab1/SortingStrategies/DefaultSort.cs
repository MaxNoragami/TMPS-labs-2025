
namespace lab1.SortingStrategies;

public class DefaultSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        numbers.Sort();
        return numbers;
    }
}

