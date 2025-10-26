using lab1.Domain.Entities;
using lab1.Domain.Products;
using lab1.Infrastructure;

namespace lab1.Application;

public class MenuService
{
    private readonly IPrototypeRegistry _menuRegistry = RegistryFactory.GetMenuRegistry();

    public IPrototype GetDish(string key)
        => _menuRegistry.GetPrototype(key);

    public void RegisterDish(string key, IPrototype prototype, User user)
    {
        if (!user.IsAdmin)
            throw new UnauthorizedAccessException(
                $"User '{user.Username}' is no Admin, so no permission to modify the global menu");

        _menuRegistry.Register(key, prototype);
    }
}
