using lab1.Domain.Products;
using lab1.Application;

namespace lab1.Infrastructure;

internal sealed class MenuRegistry : IPrototypeRegistry
{
    private static readonly MenuRegistry _instance = new();
    private readonly Dictionary<string, IPrototype> _prototypes = new();
    private readonly object _lock = new();
    public static MenuRegistry Instance => _instance;

    private MenuRegistry() { }

    public void Register(string key, IPrototype prototype)
    {
        lock (_lock)
            _prototypes[key.Trim().ToLower()] = prototype;
    }

    public IPrototype GetPrototype(string key)
    {
        lock (_lock)
            return _prototypes.TryGetValue(key.Trim().ToLower(), out IPrototype? prototype)
                ? prototype.Clone()
                : throw new ArgumentException("Prototype not found");
    }

    public List<string> GetAllKeys()
    {
        lock (_lock)
            return _prototypes.Keys.ToList();
    }
}
