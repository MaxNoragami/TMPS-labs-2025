namespace lab1.Prototypes;

public interface IPrototypeFactory
{
    IPrototype Create(string key);
}

public class PrototypeFactory(IPrototypeRegistry prototypeRegistry) : IPrototypeFactory
{
    private readonly IPrototypeRegistry _prototypeRegistry = prototypeRegistry;

    public IPrototype Create(string key)
        => _prototypeRegistry.GetPrototype(key).Clone();
}
