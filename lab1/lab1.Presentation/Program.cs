using lab1.Application;
using lab1.Domain.Entities;
using lab1.Domain.Enums;
using lab1.Infrastructure;
using lab1.Presentation.Processes;
using lab1.Presentation.Handlers;
using lab1.Presentation.Views;

var menuRegistry = RegistryFactory.GetMenuRegistry();
var sessionRepository = RepositoryFactory.GetSessionRepository();

var menuService = new MenuService(menuRegistry);
var sessionService = new SessionService(sessionRepository);
var dishDirector = new DishDirector();

var view = new ConsoleMenuView();
var dishInputHandler = new DishInputHandler(view);

var users = new List<User>
{
    new User("Alice", Role.Admin),
    new User("Bob", Role.User),
    new User("Charlie", Role.User)
};

var admin = users.First(u => u.IsAdmin);
sessionService.Login(admin, RegistryFactory.CreateUserRegistry);

var margherita = dishDirector.ConstructMargherita();
var pepperoni = dishDirector.ConstructPepperoni();
var hawaiian = dishDirector.ConstructHawaiian();
var calzoneAmericano = dishDirector.ConstructCalzoneAmericano();

menuService.RegisterDish("margherita", margherita, admin);
menuService.RegisterDish("pepperoni", pepperoni, admin);
menuService.RegisterDish("hawaiian", hawaiian, admin);
menuService.RegisterDish("calzone-americano", calzoneAmericano, admin);

sessionService.Logout();

var mainProcess = new MainProcess(
    view,
    menuService,
    sessionService,
    dishInputHandler,
    dishDirector,
    RegistryFactory.CreateUserRegistry,
    users
);

mainProcess.Run();