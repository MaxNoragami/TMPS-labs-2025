using lab1.Application;

namespace lab1.Infrastructure;

internal sealed class SessionRepository : ISessionRepository
{
    private static readonly SessionRepository _instance = new();
    private readonly Dictionary<string, IPrototypeRegistry> _userRegistries = new();
    private readonly object _lock = new();
    public static SessionRepository Instance => _instance;

    private SessionRepository() { }

    public IPrototypeRegistry? GetUserRegistry(string username)
    {
        lock (_lock)
            return _userRegistries.TryGetValue(username, out var registry)
                ? registry
                : null;
    }

    public bool HasUser(string username)
    {
        lock (_lock)
            return _userRegistries.ContainsKey(username);
    }

    public void SaveUserRegistry(string username, IPrototypeRegistry registry)
    {
        lock (_lock)
            _userRegistries[username] = registry;
    }
}
