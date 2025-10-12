namespace lab1.Sorting;

public class DefaultSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        numbers.Sort();
        return numbers;
    }
}

