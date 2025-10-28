using lab1.Domain.Products;

namespace lab1.Application;

public interface IPrototypeRegistry
{
    void Register(string key, IPrototype prototype);
    IPrototype GetPrototype(string key);
    List<string> GetAllKeys();
}
