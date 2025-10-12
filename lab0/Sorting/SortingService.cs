namespace lab0.Sorting;

public class SortingService
{
    private INumberSort _numberSort = new DefaultSort();

    public void SetNumberSortAlgo(INumberSort numberSort)
        => _numberSort = numberSort;

    public List<double> Sort(List<double> numbers)
        => _numberSort.Sort(numbers);
}
