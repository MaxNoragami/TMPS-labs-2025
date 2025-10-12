namespace lab1.Sorting;

public class QuickSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        List<double> sorted = [.. numbers];
        QuickSortRecursive(sorted, 0, sorted.Count - 1);
        return sorted;
    }

    private void QuickSortRecursive(List<double> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            QuickSortRecursive(list, left, pivotIndex - 1);
            QuickSortRecursive(list, pivotIndex + 1, right);
        }
    }

    private int Partition(List<double> list, int left, int right)
    {
        double pivot = list[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
            if (list[j] <= pivot)
            {
                i++;
                (list[i], list[j]) = (list[j], list[i]);
            }
        (list[i + 1], list[right]) = (list[right], list[i + 1]);
        return i + 1;
    }
}
