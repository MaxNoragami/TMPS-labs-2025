using lab1.Application;
using lab1.Domain.Products;

namespace lab1.Infrastructure;

internal sealed class PrototypeRegistry : IPrototypeRegistry
{
    private readonly Dictionary<string, IPrototype> _prototypes = new();

    public void Register(string key, IPrototype prototype)
        => _prototypes[key.Trim().ToLower()] = prototype;

    public IPrototype GetPrototype(string key)
        => _prototypes.TryGetValue(key.Trim().ToLower(), out IPrototype? prototype)
            ? prototype.Clone()
            : throw new ArgumentException("Prototype not found");

    public List<string> GetAllKeys()
        => _prototypes.Keys.ToList();
}
