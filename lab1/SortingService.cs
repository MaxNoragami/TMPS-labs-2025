using lab1.SortingStrategies;

namespace lab1;

public class SortingService
{
    private INumberSort _numberSort = new DefaultSort();

    public void SetNumberSortAlgo(INumberSort numberSort)
        => _numberSort = numberSort;

    public List<double> Sort(List<double> numbers)
        => _numberSort.Sort(numbers);
}
