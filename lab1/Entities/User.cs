using lab1.Enums;
using lab1.Prototypes;
using lab1.Prototypes.Registries;

namespace lab1.Entities;

public class User(string username, Role role)
{
    public string Username => username;
    public Role Role => role;
    public bool IsAdmin => Role == Role.Admin;
}
