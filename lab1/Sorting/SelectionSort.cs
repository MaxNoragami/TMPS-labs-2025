namespace lab1.Sorting;

public class SelectionSort : INumberSort
{
    public List<double> Sort(List<double> numbers)
    {
        List<double> sorted = [ .. numbers];
        int n = sorted.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
                if (sorted[j] < sorted[minIndex])
                    minIndex = j;

            if (minIndex != i)
                (sorted[i], sorted[minIndex]) = (sorted[minIndex], sorted[i]);
        }
        return sorted;
    }
}
