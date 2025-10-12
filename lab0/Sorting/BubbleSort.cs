namespace lab0.Sorting;

public class BubbleSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        List<double> sorted = [.. numbers];
        int n = sorted.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (sorted[j] > sorted[j + 1])
                    (sorted[j], sorted[j + 1]) = (sorted[j + 1], sorted[j]);

        return sorted;
    }
}
