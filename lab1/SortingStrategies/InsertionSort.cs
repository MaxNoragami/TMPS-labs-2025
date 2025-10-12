using System.Globalization;

namespace lab1.SortingStrategies;

public class InsertionSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        List<double> sorted = [.. numbers];
        int n = sorted.Count;
        for (int i = 1; i < n; i++)
        {
            double key = sorted[i];
            int j = i - 1;
            while (j >= 0 && sorted[j] > key)
            {
                sorted[j + 1] = sorted[j];
                j--;
            }
            sorted[j + 1] = key;
        }
        return sorted;
    }
}
