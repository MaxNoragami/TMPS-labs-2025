# Laboratory Work #1: Creational Design Patterns

**Author:** Maxim Alexei, *FAF-232*


## Objectives

1. Study and understand the Creational Design Patterns
2. Choose a domain, define its main classes/models/entities and choose the appropriate instantiation mechanisms
3. Use some creational design patterns for object instantiation in a sample project

---

## Theory

In software engineering, creational design patterns are general solutions that deal with object creation mechanisms, trying to create objects in a manner suitable to the situation. The basic form of object creation could result in design problems or added complexity to the design. Creational design patterns solve this problem by optimizing, hiding, or controlling the object creation process.

### Singleton Pattern

The Singleton pattern ensures a class has only one instance and provides a global access point to it. It makes the class itself responsible for keeping track of its sole instance, ensuring no other instance can be created while providing controlled access to that instance.

**Key characteristics:**
- Guarantees single instance existence
- Provides controlled global access
- Lazy initialization (created only when needed)
- Thread-safe implementation considerations

### Builder Pattern

The Builder pattern allows us to construct complex objects step by step, producing different types and representations using the same construction code. Unlike other creational patterns, Builder doesn't require products to have a common interface, making it possible to produce different products using the same construction process.

**Key characteristics:**
- Separates object construction from representation
- Constructs objects step-by-step
- Eliminates telescoping constructors
- Allows same construction process for different representations

**Director component:**
- Defines the order of building steps
- Hides construction details from client
- Enables reuse of construction routines
- Allows creating specific product configurations

### Prototype Pattern

The Prototype pattern lets us copy existing objects without making code dependent on their classes. It uses a common interface for all objects that support cloning, allowing objects to clone themselves, thus avoiding issues with accessing private fields and tight coupling to concrete classes.

**Key characteristics:**
- Clones objects without coupling to concrete classes
- Reduces initialization code repetition
- Alternative to inheritance for object configurations
- Registry implementation for frequently-used prototypes

### Factory Method Pattern

The Factory Method pattern provides an interface for creating objects but allows subclasses to alter the type of objects that will be created. It defines a method that should be used for creating objects instead of direct constructor calls.

**Key characteristics:**
- Delegates object instantiation to subclasses
- Provides flexibility in object creation
- Follows Open/Closed Principle
- Decouples client code from concrete classes

### Abstract Factory Pattern

The Abstract Factory pattern provides an interface for creating families of related or dependent objects without specifying their concrete classes. It's essentially a factory of factories, allowing the creation of objects that follow a general pattern.

**Key characteristics:**
- Creates families of related objects
- Ensures product compatibility
- Isolates concrete classes from client
- Makes exchanging product families easy

### Object Pooling Pattern

The Object Pooling pattern uses a set of initialized objects kept ready to use (a "pool") rather than allocating and destroying them on demand. This is particularly useful when object creation is expensive and the rate of instantiation is high.

**Key characteristics:**
- Reuses expensive-to-create objects
- Improves performance by reducing object creation overhead
- Manages object lifecycle efficiently
- Limits resource consumption

---

## About the Project

For this project I have decided to implement a **Pizza Restaurant Management System** in honor of "Papa's Pizzeria" series, by demonstrating creational design patterns in a situation that's quite common in real life. The system allows users to create custom pizzas and calzones, manage personal and global menus, and handle user sessions with different permission levels.

#### Some features

The application simulates a pizza restaurant where:
- Admins can manage the global menu with standard recipes
- Users can create custom dishes and maintain personal menus
- Dishes can be cloned and customized
- Pre-configured recipes are available through a director

#### Architecture

The project follows **Clean Architecture** principles with clear separation of concerns across four layers:

1. **Domain Layer** - Contains core business entities and interfaces
2. **Application Layer** - Implements business logic and use cases
3. **Infrastructure Layer** - Handles data persistence and external concerns
4. **Presentation Layer** - Manages user interaction and display

#### Used Design Patterns

The implementation incorporates three creational design patterns:

1. **Builder Pattern** - For step-by-step construction of complex Pizza and Calzone objects with various configurations (size, dough type, sauces, toppings)

2. **Prototype Pattern** - For cloning existing dishes (complex objects) from menus, allowing users to customize copies without affecting originals

3. **Singleton Pattern** - For managing global menu registry and session repository, ensuring single instances throughout the application lifecycle

---

## Implementation

### 1. Builder Pattern Implementation

The Builder pattern is implemented through the `IFoodBuilder` interface and its concrete implementations for Pizza and Calzone construction.

#### Builder Interface

```csharp
public interface IFoodBuilder
{
    IFoodBuilder SetName(string name);
    IFoodBuilder SetSize(FoodSize size);
    IFoodBuilder SetDough(DoughType dough);
    IFoodBuilder AddSauce(SauceType sauce);
    IFoodBuilder AddCheese(CheeseType cheese);
    IFoodBuilder AddExtras(Extras extras);
    void Reset();
}
```

The interface defines the construction steps common to all food products, using a fluent interface pattern that returns `IFoodBuilder` to enable method chaining.

#### Concrete Builder - PizzaBuilder

```csharp
public class PizzaBuilder : IFoodBuilder
{
    private string _name = string.Empty;
    private FoodSize _size;
    private DoughType _dough;
    private SauceType? _sauce;
    private CheeseType? _cheese;
    private List<Extras> _extras = [];

    private PizzaBuilder() { }

    public static IFoodBuilder Empty() => new PizzaBuilder();

    public Pizza Cook()
    {
        var pizza = new Pizza()
        {
            Name = _name,
            Size = _size,
            Dough = _dough,
            Sauce = _sauce,
            Cheese = _cheese,
            Toppings = [.. _extras]
        };
        Reset();
        return pizza;
    }

    public IFoodBuilder SetName(string name)
    {
        _name = name;
        return this;
    }
    
    // Other builder methods follow the same pattern...
}
```

The `PizzaBuilder` encapsulates the construction logic, maintaining internal state and providing a `Cook()` method that produces the final product. The constructor is private, forcing clients to use the static `Empty()` factory method. After building, `Reset()` is called to prepare the builder for reuse.

#### Director Class

```csharp
public class DishDirector
{
    private IFoodBuilder? _builder;

    public void SetBuilder(IFoodBuilder builder)
        => _builder = builder;

    public void ConstructMargherita()
    {
        if (_builder == null)
            throw new InvalidOperationException("Builder not set. Call SetBuilder() first.");

        _builder.SetName("Margherita")
                .SetSize(FoodSize.Medium)
                .SetDough(DoughType.Classic)
                .AddSauce(SauceType.Tomato)
                .AddCheese(CheeseType.Provolone)
                .AddExtras(Extras.Spinach);
    }

    public void ConstructPepperoni()
    {
        if (_builder == null)
            throw new InvalidOperationException("Builder not set. Call SetBuilder() first.");

        _builder.SetName("Pepperoni")
                .SetSize(FoodSize.Large)
                .SetDough(DoughType.Classic)
                .AddSauce(SauceType.Tomato)
                .AddCheese(CheeseType.Cheddar)
                .AddExtras(Extras.Pepperoni)
                .AddExtras(Extras.Olives);
    }

    public void ConstructHawaiian()
    {
        if (_builder == null)
            throw new InvalidOperationException("Builder not set. Call SetBuilder() first.");

        _builder.SetName("Hawaiian")
                .SetSize(FoodSize.Medium)
                .SetDough(DoughType.Classic)
                .AddSauce(SauceType.Tomato)
                .AddCheese(CheeseType.Provolone)
                .AddExtras(Extras.Ham)
                .AddExtras(Extras.Pineapple);
    }

    public void ConstructCalzoneAmericano()
    {
        if (_builder == null)
            throw new InvalidOperationException("Builder not set. Call SetBuilder() first.");

        _builder.SetName("Americano")
                .SetSize(FoodSize.Large)
                .SetDough(DoughType.Sicilian)
                .AddSauce(SauceType.Alfredo)
                .AddCheese(CheeseType.Cheddar)
                .AddExtras(Extras.Sausage)
                .AddExtras(Extras.Bacon);
    }
}
```

The `DishDirector` encapsulates pre-defined recipes and construction sequences. It hides the building complexity from clients and provides a convenient API for creating standard dishes. The director demonstrates how the same builder can be used to create different product configurations.

#### Usage in Presentation Layer

```csharp
public class DishInputHandler(IMenuView view)
{
    private readonly IMenuView _view = view;

    public Pizza CreatePizza()
    {
        var builder = PizzaBuilder.Empty();
        return BuildPizza(builder);
    }

    public Calzone CreateCalzone()
    {
        var builder = CalzoneBuilder.Empty();
        return BuildCalzone(builder);
    }

    private Pizza BuildPizza(PizzaBuilder builder)
    {
        var name = _view.GetInput("Enter Pizza name");
        var size = _view.GetFoodSize();
        var dough = _view.GetDoughType();
        var sauce = _view.GetSauceType();
        var cheese = _view.GetCheeseType();
        var extras = _view.GetExtras();

        builder.SetName(name)
               .SetSize(size)
               .SetDough(dough);

        if (sauce.HasValue)
            builder.AddSauce(sauce.Value);

        if (cheese.HasValue)
            builder.AddCheese(cheese.Value);

        foreach (var extra in extras)
            builder.AddExtras(extra);

        return builder.Cook();
    }

    private Calzone BuildCalzone(CalzoneBuilder builder)
    {
        var name = _view.GetInput("Enter Calzone name");
        var size = _view.GetFoodSize();
        var dough = _view.GetDoughType();
        var sauce = _view.GetSauceType();
        var cheese = _view.GetCheeseType();
        var extras = _view.GetExtras();

        builder.SetName(name)
               .SetSize(size)
               .SetDough(dough);

        if (sauce.HasValue)
            builder.AddSauce(sauce.Value);

        if (cheese.HasValue)
            builder.AddCheese(cheese.Value);

        foreach (var extra in extras)
            builder.AddExtras(extra);

        return builder.Cook();
    }
}
```

The presentation layer uses builders directly for custom user-created dishes, demonstrating the flexibility of the pattern - the director is used for standard recipes, while direct builder usage allows full user customization.

### 2. Prototype Pattern Implementation

The Prototype pattern is implemented through the `IPrototype` interface and registry system that stores and clones dish prototypes.

#### Prototype Interface

```csharp
public interface IPrototype
{
    public IPrototype Clone();
}
```

The interface defines the contract for all cloneable objects, requiring only a single `Clone()` method.

#### Concrete Prototype - Pizza

```csharp
public class Pizza : IPrototype
{
    public string Name { get; init; } = string.Empty;
    public FoodSize Size { get; init; }
    public DoughType Dough { get; init; }
    public SauceType? Sauce { get; init; }
    public CheeseType? Cheese { get; init; }
    public List<Extras> Toppings { get; init; } = [];

    public IPrototype Clone()
        => new Pizza()
        {
            Name = Name,
            Size = Size,
            Dough = Dough,
            Sauce = Sauce,
            Cheese = Cheese,
            Toppings = [.. Toppings]
        };
}
```

The `Pizza` class implements deep cloning by creating a new instance and copying all field values. The `Toppings` list is cloned using the collection expression `[.. Toppings]` to avoid shared references. The `init` keyword ensures immutability after construction.

#### Prototype Registry

```csharp
public interface IPrototypeRegistry
{
    void Register(string key, IPrototype prototype);
    IPrototype GetPrototype(string key);
    List<string> GetAllKeys();
}
```

The registry interface defines operations for storing and retrieving prototypes by key.

#### Prototype Registry Implementation

```csharp
internal sealed class PrototypeRegistry : IPrototypeRegistry
{
    private readonly Dictionary<string, IPrototype> _prototypes = new();

    public void Register(string key, IPrototype prototype)
        => _prototypes[key.Trim().ToLower()] = prototype;

    public IPrototype GetPrototype(string key)
        => _prototypes.TryGetValue(key.Trim().ToLower(), out IPrototype? prototype)
            ? prototype.Clone()
            : throw new ArgumentException("Prototype not found");

    public List<string> GetAllKeys()
        => _prototypes.Keys.ToList();
}
```

The registry stores prototype instances and returns clones when requested, ensuring the original prototypes remain unmodified. Keys are normalized (trimmed and lowercased) for case-insensitive lookup.

#### Usage in Application Layer

```csharp
public class MenuService
{
    private readonly IPrototypeRegistry _menuRegistry;

    public IPrototype GetDish(string key)
        => _menuRegistry.GetPrototype(key);

    public void RegisterDish(string key, IPrototype prototype, User user)
    {
        if (!user.IsAdmin)
            throw new UnauthorizedAccessException();

        _menuRegistry.Register(key, prototype);
    }
}
```

The `MenuService` uses the prototype registry to store and retrieve dishes. When `GetDish()` is called, the registry automatically returns a clone, allowing users to customize their orders without affecting the menu template.

```csharp
// Example: User adds a dish from global menu to personal menu
var dish = menuService.GetDish("margherita"); // Gets a clone
customService.RegisterDish("my-custom-margherita", dish);
```

### 3. Singleton Pattern Implementation

The Singleton pattern is implemented in two critical infrastructure components: the global menu registry and the session repository.

#### Singleton - MenuRegistry

```csharp
internal sealed class MenuRegistry : IPrototypeRegistry
{
    private static readonly MenuRegistry _instance = new();
    private readonly Dictionary<string, IPrototype> _prototypes = new();
    private readonly object _lock = new();
    
    public static MenuRegistry Instance => _instance;

    private MenuRegistry() { }

    public void Register(string key, IPrototype prototype)
    {
        lock (_lock)
            _prototypes[key.Trim().ToLower()] = prototype;
    }

    public IPrototype GetPrototype(string key)
    {
        lock (_lock)
            return _prototypes.TryGetValue(key.Trim().ToLower(), out IPrototype? prototype)
                ? prototype.Clone()
                : throw new ArgumentException("Prototype not found");
    }
}
```

The `MenuRegistry` implements the Singleton pattern with several key features:

1. **Private constructor** prevents external instantiation
2. **Static readonly instance** ensures thread-safe initialization using C#'s before field init semantics
3. **Public static property** provides global access point
4. **Thread synchronization** using `lock` for dictionary operations
5. **Sealed class** prevents inheritance

This implementation uses the "Initialization-on-demand holder idiom" adapted for C#, ensuring lazy initialization and thread safety without explicit locking on the getter.

#### Singleton - SessionRepository

```csharp
internal sealed class SessionRepository : ISessionRepository
{
    private static readonly SessionRepository _instance = new();
    private readonly Dictionary<string, IPrototypeRegistry> _userRegistries = new();
    private readonly object _lock = new();
    
    public static SessionRepository Instance => _instance;

    private SessionRepository() { }

    public void SaveUserRegistry(string username, IPrototypeRegistry registry)
    {
        lock (_lock)
            _userRegistries[username] = registry;
    }

    public IPrototypeRegistry? GetUserRegistry(string username)
    {
        lock (_lock)
            return _userRegistries.TryGetValue(username, out var registry)
                ? registry
                : null;
    }
}
```

The `SessionRepository` follows the same Singleton pattern, managing user-specific registries. This ensures all parts of the application work with the same session data.

#### Singleton Factory Access

```csharp
public static class RegistryFactory
{
    public static IPrototypeRegistry GetMenuRegistry()
        => MenuRegistry.Instance;

    public static IPrototypeRegistry CreateUserRegistry()
        => new PrototypeRegistry();
}
```

The factory provides a clean API for accessing the Singleton menu registry while creating new instances for user registries. This demonstrates how Singletons can coexist with regular instantiation patterns.

#### Usage Example

```csharp
// Program.cs initialization
var menuRegistry = RegistryFactory.GetMenuRegistry(); // Singleton
var sessionRepository = RepositoryFactory.GetSessionRepository(); // Singleton

var menuService = new MenuService(menuRegistry);
var sessionService = new SessionService(sessionRepository);

// All services share the same Singleton instances
```

The Singleton pattern ensures:
- **Single source of truth** for the global menu
- **Consistent session state** across the application
- **Memory efficiency** by avoiding duplicate instances
- **Controlled access** to critical shared resources

### Pattern Integration

The three patterns work together harmoniously in the system:

1. **Builder** creates the initial dish objects with complex configurations
2. **Prototype** enables efficient cloning of dishes from registries
3. **Singleton** ensures global registries are shared across the entire application

Example workflow:
```csharp
// Admin creates a standard dish using Director (Builder pattern)
var margherita = dishDirector.ConstructMargherita();

// Registers it in the global menu (Singleton pattern)
menuService.RegisterDish("margherita", margherita, admin);

// User gets a clone from the menu (Prototype pattern)
var myPizza = menuService.GetDish("margherita");

// User customizes their clone and saves to personal menu
customService.RegisterDish("my-margherita", myPizza);
```

---

## Results

The following screenshots demonstrate the functionality of the implemented Pizza Restaurant Management System.

<img src="https://files.catbox.moe/1i4jhg.png" alt="Initial view of the running program" width="500">

When the program starts, the user is prompted to log in as either an Admin or a normal User. For now, I initiate myself the available users, but that could easily be extended to a full authentication system, if needed.

<img src="https://files.catbox.moe/lgv1aq.png" alt="Viewing available dishes in both menus" width="500">

Based on the prompted menu, the user can select any option, like in this case it is viewing the available dishes in both global and its custom menus.

<img src="https://files.catbox.moe/1sufy1.png" alt="Adding custom dish to custom menu - 1" width="500">

 
<img src="https://files.catbox.moe/7umif8.png" alt="Adding custom dish to custom menu - 2" width="500">

Moreover, the user can add custom dishes to its personal menu by specifying all the required parameters step by step, thanks to the Builder pattern implementation.

<img src="https://files.catbox.moe/zkgcos.png" alt="Adding preset dish to main common menu as Admin" width="500">

As an Admin, the user, besides adding custom dishes to its custom/personal menu, it can also add other dishes to the global menu, available for all users to order from, besides their own custom menus.

<img src="https://files.catbox.moe/bqobsc.png" alt="Switching to normal User" width="500">

We can also logout from the Admin account and log back in as a normal User.

<img src="https://files.catbox.moe/0wazwb.png" alt="Viewing the menus once again, but as another user" width="500">

Here we can see that the global menu still contains the dish added by the Admin previously, while the custom menu is empty, as this is a different user.

<img src="https://files.catbox.moe/saqchy.png" alt="Ordering a dish off global menu, added by the admin, as user" width="500">

Last but not least, the normal User can order dishes off the global menu, added by the Admin, demonstrating the proper functioning of all three creational design patterns integrated within the system.

---

## Conclusions

Overall, via this laboratory work I have managed to demonstrate the implementation and integration of three fundamental creational design patterns in the rather possible scenario of a pizza restaurant management system.

Therefore, the *Builder pattern* proved essential for constructing complex food objects with numerous optional parameters. By eliminating telescoping constructors and providing a fluent interface, it made the code more readable and maintainable. The separation between the concrete builders and the director class showcased how the same construction process can produce different representations while encapsulating standard recipes.

Moreover, the *Prototype pattern* enabled efficient object cloning without coupling the code to concrete classes. The registry implementation provided a clean way to manage frequently-used dish templates, allowing users to customize clones without affecting the originals. This pattern significantly reduced initialization code and provided an elegant alternative to inheritance for managing dish variations.

Last but not least, the *Singleton pattern* ensured critical resources like the global menu registry and session repository maintained single, shared instances throughout the application lifecycle. Also, by making the implementation thread-safe ensured the proper handling of concurrent access while providing controlled global access points.

Overall, the integration of these patterns within a Clean Architecture structure highlighted quite well how design patterns complement architectural principles. Each pattern served a specific purpose while maintaining loose coupling and high cohesion across layers.

---

## References

1. Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1994). *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley Professional.

2. Refactoring Guru. *Creational Design Patterns*. Retrieved from https://refactoring.guru/design-patterns/creational-patterns

3. Refactoring Guru. *Builder Pattern*. Retrieved from https://refactoring.guru/design-patterns/builder

4. Refactoring Guru. *Prototype Pattern*. Retrieved from https://refactoring.guru/design-patterns/prototype

5. Refactoring Guru. *Singleton Pattern*. Retrieved from https://refactoring.guru/design-patterns/singleton
