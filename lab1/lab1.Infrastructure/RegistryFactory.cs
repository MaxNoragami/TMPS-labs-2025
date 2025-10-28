using lab1.Application;

namespace lab1.Infrastructure;

public static class RegistryFactory
{
    public static IPrototypeRegistry GetMenuRegistry()
        => MenuRegistry.Instance;

    public static IPrototypeRegistry CreateUserRegistry()
        => new PrototypeRegistry();
}
