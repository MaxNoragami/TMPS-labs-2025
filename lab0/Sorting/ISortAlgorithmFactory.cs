namespace lab0.Sorting;

public interface ISortAlgorithmFactory
{
    (string, INumberSort) CreateAlgorithm(string algorithmName);
    void Register(string name, Func<INumberSort> algorithmFactory);
}
