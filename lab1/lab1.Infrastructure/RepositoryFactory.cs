using lab1.Application;

namespace lab1.Infrastructure;

public static class RepositoryFactory
{
    public static ISessionRepository GetSessionRepository()
        => SessionRepository.Instance;
}
