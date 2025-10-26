using lab1.Entities;
using lab1.Prototypes;
using lab1.Prototypes.Registries;

namespace lab1.Services;

public class CustomMenuService(User user)
{
    private readonly User _user = user;
    private readonly IPrototypeRegistry _customRegistry = RegistryFactory.CreateUserRegistry();
    
    public void RegisterDish(string key, IPrototype prototype)
        => _customRegistry.Register(key, prototype);

    public IPrototype GetDish(string key)
        => _customRegistry.GetPrototype(key);
}
