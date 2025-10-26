using lab1.Domain.Builders;
using lab1.Domain.Entities;
using lab1.Domain.Enums;
using lab1.Application;
using lab1.Infrastructure;


var admin = new User("Alice", Role.Admin);
var user = new User("Bob", Role.User);

var menuService = new MenuService(RegistryFactory.GetMenuRegistry());
var sessionService = new SessionService(RepositoryFactory.GetSessionRepository());

var margherita = PizzaBuilder.Empty()
.SetName("Margherita")
    .SetSize(FoodSize.Medium)
    .AddSauce(SauceType.Tomato)
    .AddCheese(CheeseType.Provolone)
    .AddExtras(Extras.Spinach)
    .Cook();

var calzoneAmericano = CalzoneBuilder.Empty()
    .SetName("CalzoneAmericano")
    .SetSize(FoodSize.Large)
    .AddSauce(SauceType.Alfredo)
    .AddExtras(Extras.Sausage)
    .Cook();


// Log in as Alice
sessionService.Login(admin, RegistryFactory.CreateUserRegistry);

menuService.RegisterDish("margherita", margherita, admin);

var adminCustomMenu = new CustomMenuService(sessionService.GetCurrentUserRegistry());
adminCustomMenu.RegisterDish("calzone-americano", calzoneAmericano);
sessionService.Logout();

// Log in as bob
sessionService.Login(user, RegistryFactory.CreateUserRegistry);

try
{
    menuService.RegisterDish("another-margherita", margherita, user);
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

var anotherMargherita = menuService.GetDish("margherita");

var userCustomMenu = new CustomMenuService(sessionService.GetCurrentUserRegistry());
userCustomMenu.RegisterDish("user-pizza", anotherMargherita);
userCustomMenu.RegisterDish("user-calzone", calzoneAmericano);
sessionService.Logout();

// relogin as user
sessionService.Login(user, RegistryFactory.CreateUserRegistry);
var userCustomMenuAgain = new CustomMenuService(sessionService.GetCurrentUserRegistry());
var userPizza = userCustomMenuAgain.GetDish("user-pizza");
Console.Write($"{userPizza.ToString()}");
