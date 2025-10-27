using lab1.Domain.Products;

namespace lab1.Application;

public class CustomMenuService(IPrototypeRegistry customRegistry)
{
    private readonly IPrototypeRegistry _customRegistry = customRegistry;

    public void RegisterDish(string key, IPrototype prototype)
        => _customRegistry.Register(key, prototype);

    public IPrototype GetDish(string key)
        => _customRegistry.GetPrototype(key);
}
