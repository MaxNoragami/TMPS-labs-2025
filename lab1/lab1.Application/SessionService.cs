using lab1.Domain.Entities;

namespace lab1.Application;

public class SessionService(ISessionRepository sessionRepository)
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public void Login(User user, Func<IPrototypeRegistry> registryFactory)
    {
        CurrentUser = user;

        if (!_sessionRepository.HasUser(user.Username))
        {
            var registry = registryFactory();
            _sessionRepository.SaveUserRegistry(user.Username, registry);
        }    
    }

    public void Logout()
        => CurrentUser = null;

    public IPrototypeRegistry GetCurrentUserRegistry()
    {
        if (CurrentUser == null)
            throw new InvalidOperationException("No user logged in");

        var registry = _sessionRepository.GetUserRegistry(CurrentUser.Username);
        if (registry == null)
            throw new InvalidOperationException($"Registry not found for user '{CurrentUser.Username}'");

        return registry;
    }
}
