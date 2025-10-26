using lab1.Domain.Entities;
using lab1.Domain.Products;
using lab1.Infrastructure;

namespace lab1.Application;

public class CustomMenuService(User user)
{
    private readonly User _user = user;
    private readonly IPrototypeRegistry _customRegistry = RegistryFactory.CreateUserRegistry();
    
    public void RegisterDish(string key, IPrototype prototype)
        => _customRegistry.Register(key, prototype);

    public IPrototype GetDish(string key)
        => _customRegistry.GetPrototype(key);
}
