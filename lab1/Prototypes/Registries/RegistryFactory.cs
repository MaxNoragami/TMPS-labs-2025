namespace lab1.Prototypes.Registries;

public static class RegistryFactory
{
    public static IPrototypeRegistry GetMenuRegistry()
        => MenuRegistry.Instance;

    public static IPrototypeRegistry CreateUserRegistry()
        => new PrototypeRegistry();
}
