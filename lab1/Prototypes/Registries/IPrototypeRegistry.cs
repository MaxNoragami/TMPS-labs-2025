using lab1.Entities;

namespace lab1.Prototypes.Registries;

public interface IPrototypeRegistry
{
    void Register(string key, IPrototype prototype);
    IPrototype GetPrototype(string key);
}
