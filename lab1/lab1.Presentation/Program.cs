using lab1.Application;
using lab1.Domain.Builders;
using lab1.Domain.Entities;
using lab1.Domain.Enums;
using lab1.Infrastructure;
using lab1.Presentation.Processes;
using lab1.Presentation.Handlers;
using lab1.Presentation.Views;

// Initialize infrastructure
var menuRegistry = RegistryFactory.GetMenuRegistry();
var sessionRepository = RepositoryFactory.GetSessionRepository();

// Initialize services
var menuService = new MenuService(menuRegistry);
var sessionService = new SessionService(sessionRepository);

// Initialize presentation layer
var view = new ConsoleMenuView();
var dishFactory = new DishCreator(view);

// Create available users
var users = new List<User>
{
    new User("Alice", Role.Admin),
    new User("Bob", Role.User),
    new User("Charlie", Role.User)
};

// Seed initial menu with some dishes
var admin = users.First(u => u.IsAdmin);
sessionService.Login(admin, RegistryFactory.CreateUserRegistry);

var margherita = PizzaBuilder.Empty()
    .SetName("Margherita")
    .SetSize(FoodSize.Medium)
    .SetDough(DoughType.Classic)
    .AddSauce(SauceType.Tomato)
    .AddCheese(CheeseType.Provolone)
    .AddExtras(Extras.Spinach)
    .Cook();

var pepperoni = PizzaBuilder.Empty()
    .SetName("Pepperoni")
    .SetSize(FoodSize.Large)
    .SetDough(DoughType.Classic)
    .AddSauce(SauceType.Tomato)
    .AddCheese(CheeseType.Cheddar)
    .AddExtras(Extras.Pepperoni)
    .AddExtras(Extras.Olives)
    .Cook();

var hawaiian = PizzaBuilder.Empty()
    .SetName("Hawaiian")
    .SetSize(FoodSize.Medium)
    .SetDough(DoughType.Classic)
    .AddSauce(SauceType.Tomato)
    .AddCheese(CheeseType.Provolone)
    .AddExtras(Extras.Ham)
    .AddExtras(Extras.Pineapple)
    .Cook();

var calzoneAmericano = CalzoneBuilder.Empty()
    .SetName("Americano")
    .SetSize(FoodSize.Large)
    .SetDough(DoughType.Sicilian)
    .AddSauce(SauceType.Alfredo)
    .AddCheese(CheeseType.Cheddar)
    .AddExtras(Extras.Sausage)
    .AddExtras(Extras.Bacon)
    .Cook();

menuService.RegisterDish("margherita", margherita, admin);
menuService.RegisterDish("pepperoni", pepperoni, admin);
menuService.RegisterDish("hawaiian", hawaiian, admin);
menuService.RegisterDish("calzone-americano", calzoneAmericano, admin);

sessionService.Logout();

// Initialize and run controller
var controller = new MainProcess(
    view,
    menuService,
    sessionService,
    dishFactory,
    RegistryFactory.CreateUserRegistry,
    users
);

controller.Run();