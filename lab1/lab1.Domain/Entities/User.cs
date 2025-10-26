using lab1.Domain.Enums;

namespace lab1.Domain.Entities;

public class User(string username, Role role)
{
    public string Username => username;
    public Role Role => role;
    public bool IsAdmin => Role == Role.Admin;
}
