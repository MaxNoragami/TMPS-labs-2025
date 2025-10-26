using lab1.Domain.Entities;
using lab1.Domain.Products;

namespace lab1.Application;

public class MenuService(IPrototypeRegistry menuRegistry)
{
    private readonly IPrototypeRegistry _menuRegistry = menuRegistry;

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
