namespace lab1.Application;

public interface ISessionRepository
{
    void SaveUserRegistry(string username, IPrototypeRegistry registry);
    IPrototypeRegistry? GetUserRegistry(string username);
    bool HasUser(string username);
}