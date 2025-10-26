using lab1.Builders;
using lab1.Entities;
using lab1.Enums;
using lab1.Services;

var admin = new User("Alice", Role.Admin);
var user = new User("Bob", Role.User);

var menuService = new MenuService();
var userCustomMenu = new CustomMenuService(user);

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

menuService.RegisterDish("margherita", margherita, admin);

try
{
    menuService.RegisterDish("another-margherita", margherita, user);
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

var anotherMargherita = menuService.GetDish("margherita");
Console.WriteLine($"User ordered: {anotherMargherita.ToString()}");
Console.WriteLine($"Same instance? {anotherMargherita.Equals(margherita)}");

userCustomMenu.RegisterDish("calzone-americano", calzoneAmericano);

var anotherCalzone = userCustomMenu.GetDish("calzone-americano");
Console.WriteLine($"User ordered: {anotherCalzone.ToString()}");
Console.WriteLine($"Same instance? {anotherCalzone.Equals(calzoneAmericano)}");
