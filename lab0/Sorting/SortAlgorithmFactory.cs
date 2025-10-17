
namespace lab0.Sorting;

public class SortAlgorithmFactory : ISortAlgorithmFactory
{
    private readonly Dictionary<string, Func<INumberSort>> _algorithms = [];

    public SortAlgorithmFactory()
    {
        Register("bubble", () => new BubbleSort());
        Register("insertion", () => new InsertionSort());
        Register("selection", () => new SelectionSort());
        Register("quick", () => new QuickSort());
    }

    public void Register(string name, Func<INumberSort> algorithmFactory)
        => _algorithms[name.ToLower().Trim()] = algorithmFactory;

    public (string, INumberSort) CreateAlgorithm(string algorithmName)
    {
        var key = algorithmName?.ToLower().Trim() ?? string.Empty;

        if (_algorithms.TryGetValue(key, out var factory))
            return (key, factory());

        return ("default", new DefaultSort());
    }
}
