using lab1.Application;
using lab1.Domain.Entities;
using lab1.Domain.Products;
using lab1.Presentation.Views;
using lab1.Presentation.Handlers;

namespace lab1.Presentation.Processes;

public class MainProcess(
        IMenuView view,
        MenuService menuService,
        SessionService sessionService,
        DishInputHandler dishInputHandler,
        DishDirector dishDirector,
        Func<IPrototypeRegistry> registryFactory,
        List<User> availableUsers)
{
    private readonly IMenuView _view = view;
    private readonly MenuService _menuService = menuService;
    private readonly SessionService _sessionService = sessionService;
    private readonly DishInputHandler _dishInputHandler = dishInputHandler;
    private readonly DishDirector _dishDirector = dishDirector;
    private readonly Func<IPrototypeRegistry> _registryFactory = registryFactory;
    private readonly List<User> _availableUsers = availableUsers;


    public void Run()
    {
        _view.DisplayWelcome();

        if (!SelectUser())
            return;

        bool running = true;
        while (running)
        {
            DisplayCurrentUserInfo();
            _view.DisplayMainMenu();

            int choice = _view.GetMenuChoice();

            try
            {
                running = HandleMenuChoice(choice);
            }
            catch (Exception ex)
            {
                _view.DisplayError(ex.Message);
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private bool HandleMenuChoice(int choice)
    {
        return choice switch
        {
            1 => HandleViewMenus(),
            2 => HandleOrderFromMenu(),
            3 => HandleCreateCustomDish(),
            4 => HandleAddToCustomMenu(),
            5 => HandleAddToGlobalMenu(),
            6 => HandleSwitchUser(),
            7 => HandleExit(),
            _ => HandleInvalidChoice()
        };
    }

    private bool HandleViewMenus()
    {
        Console.WriteLine("\n1. View Global Menu");
        Console.WriteLine("2. View Custom Menu");
        Console.WriteLine("3. View Both");
        Console.Write("Choice: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ViewGlobalMenu();
                break;
            case "2":
                ViewCustomMenu();
                break;
            case "3":
                ViewGlobalMenu();
                ViewCustomMenu();
                break;
        }

        return true;
    }

    private void ViewGlobalMenu()
    {
        var items = GetMenuItems(_menuService.GetAllDishKeys(), _menuService.GetDish);
        _view.DisplayMenuItems(items, "Global");
    }

    private void ViewCustomMenu()
    {
        var customService = new CustomMenuService(_sessionService.GetCurrentUserRegistry());
        var items = GetMenuItems(customService.GetAllDishKeys(), customService.GetDish);
        _view.DisplayMenuItems(items, "Custom");
    }

    private Dictionary<string, string> GetMenuItems(List<string> keys, Func<string, IPrototype> getDish)
    {
        var items = new Dictionary<string, string>();

        foreach (var key in keys)
        {
            try
            {
                var dish = getDish(key);
                items[key] = dish.ToString();
            }
            catch { /* Skip item that cannot be retrieved */ }
        }

        return items;
    }

    private bool HandleOrderFromMenu()
    {
        Console.WriteLine("\nOrder from:");
        Console.WriteLine("1. Global Menu");
        Console.WriteLine("2. Custom Menu");
        Console.Write("Choice: ");

        var menuChoice = Console.ReadLine();
        var dishKey = _view.GetInput("Enter dish name");

        try
        {
            IPrototype dish = menuChoice switch
            {
                "1" => _menuService.GetDish(dishKey),
                "2" => new CustomMenuService(_sessionService.GetCurrentUserRegistry()).GetDish(dishKey),
                _ => throw new ArgumentException("Invalid menu choice")
            };

            _view.DisplayMessage("Order placed successfully!");
            _view.DisplayDish(dish.ToString());
        }
        catch (ArgumentException ex)
        {
            _view.DisplayError(ex.Message);
        }

        return true;
    }

    private bool HandleCreateCustomDish()
    {
        Console.WriteLine("\nCreate:");
        Console.WriteLine("1. Pizza");
        Console.WriteLine("2. Calzone");
        Console.Write("Choice: ");

        var choice = Console.ReadLine();

        IPrototype dish = choice switch
        {
            "1" => _dishInputHandler.CreatePizza(),
            "2" => _dishInputHandler.CreateCalzone(),
            _ => throw new ArgumentException("Invalid dish type")
        };

        _view.DisplayMessage("Dish created successfully!");
        _view.DisplayDish(dish.ToString());

        if (_view.GetConfirmation("Add to custom menu?"))
        {
            var key = _view.GetInput("Enter key for dish");
            var customService = new CustomMenuService(_sessionService.GetCurrentUserRegistry());
            customService.RegisterDish(key, dish);
            _view.DisplayMessage($"Added to custom menu as '{key}'");
        }

        return true;
    }

    private bool HandleAddToCustomMenu()
    {
        var dishKey = _view.GetInput("Enter dish name from global menu");

        try
        {
            var dish = _menuService.GetDish(dishKey);
            var customKey = _view.GetInput("Enter key for your custom menu");

            var customService = new CustomMenuService(_sessionService.GetCurrentUserRegistry());
            customService.RegisterDish(customKey, dish);

            _view.DisplayMessage($"Added '{dishKey}' to your custom menu as '{customKey}'");
        }
        catch (ArgumentException ex)
        {
            _view.DisplayError(ex.Message);
        }

        return true;
    }

    private bool HandleAddToGlobalMenu()
    {
        if (_sessionService.CurrentUser == null || !_sessionService.CurrentUser.IsAdmin)
        {
            _view.DisplayError("Only administrators can modify the global menu");
            return true;
        }

        Console.WriteLine("\nAdd:");
        Console.WriteLine("1. Standard Margherita");
        Console.WriteLine("2. Standard Pepperoni");
        Console.WriteLine("3. Standard Hawaiian");
        Console.WriteLine("4. Standard Calzone Americano");
        Console.WriteLine("5. Custom Pizza");
        Console.WriteLine("6. Custom Calzone");
        Console.WriteLine("7. From Custom Menu");
        Console.Write("Choice: ");

        var choice = Console.ReadLine();

        try
        {
            IPrototype dish;
            string key;

            dish = choice switch
            {
                "1" => _dishDirector.ConstructMargherita(),
                "2" => _dishDirector.ConstructPepperoni(),
                "3" => _dishDirector.ConstructHawaiian(),
                "4" => _dishDirector.ConstructCalzoneAmericano(),
                "5" => _dishInputHandler.CreatePizza(),
                "6" => _dishInputHandler.CreateCalzone(),
                "7" => GetDishFromCustomMenu(),
                _ => throw new ArgumentException("Invalid choice")
            };

            key = _view.GetInput("Enter key for global menu");
            _menuService.RegisterDish(key, dish, _sessionService.CurrentUser);
            _view.DisplayMessage($"Added to global menu as '{key}'");
        }
        catch (Exception ex)
        {
            _view.DisplayError(ex.Message);
        }

        return true;
    }

    private IPrototype GetDishFromCustomMenu()
    {
        var customKey = _view.GetInput("Enter dish key from your custom menu");
        var customService = new CustomMenuService(_sessionService.GetCurrentUserRegistry());
        return customService.GetDish(customKey);
    }

    private bool HandleSwitchUser()
    {
        _sessionService.Logout();
        return SelectUser();
    }

    private bool HandleExit()
    {
        _view.DisplayMessage("Thank you for visiting Pizza Paradise!");
        return false;
    }

    private bool HandleInvalidChoice()
    {
        _view.DisplayError("Invalid choice. Please try again.");
        return true;
    }

    private bool SelectUser()
    {
        Console.WriteLine("\nAvailable Users:");
        for (int i = 0; i < _availableUsers.Count; i++)
        {
            var user = _availableUsers[i];
            Console.WriteLine($"{i + 1}. {user.Username} ({user.Role})");
        }
        Console.WriteLine($"{_availableUsers.Count + 1}. Exit");

        Console.Write("\nSelect user: ");
        if (int.TryParse(Console.ReadLine(), out int choice)
            && choice > 0
            && choice <= _availableUsers.Count)
        {
            var selectedUser = _availableUsers[choice - 1];
            _sessionService.Login(selectedUser, _registryFactory);
            _view.DisplayMessage($"Logged in as {selectedUser.Username}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            return true;
        }

        return false;
    }

    private void DisplayCurrentUserInfo()
    {
        if (_sessionService.CurrentUser != null)
        {
            _view.DisplayUserInfo(
                _sessionService.CurrentUser.Username,
                _sessionService.CurrentUser.Role.ToString()
            );
        }
    }
}