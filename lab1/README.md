# Laboratory Work #0: SOLID Principles

## **Purpose**

Design and implement a simple project that demonstrates object-oriented design following at least three out of the SOLID principles.

## **Theory**

SOLID principles are five object-oriented design principles that help create maintainable, scalable, and flexible software. The acronym stands for: Single Responsibility Principle, Open-Closed Principle, Liskov Substitution Principle, Interface Segregation Principle, and Dependency Inversion Principle. Coined by Robert C. Martin, these principles provide guidelines for writing code that is easier to understand and modify over time. 

#### **SRP (Single Responsibility Principle)**
*A class should have just one reason to change.*

Each class should be responsible for a single part of the functionality, keeping that responsibility entirely encapsulated. This reduces complexity and makes code navigation easier as the program grows.

#### **OCP (Open/Closed Principle)**
*Classes should be open for extension, but closed for modification.*

*Practices to achieve it:*
- **Use Abstraction:**
	- Define interfaces or base classes that represent the common behavior or contracts.
	- Program to interface or base class, open to accept different implementations.
- **Apply inheritance and polymorphism:**
	- Use inheritance to create derived classes that inherit from a base class or implement an interface.
	- Polymorphism allows us to work with objects of different derived classes through a common interface, allowing for extension without modification of existing code.
- **Utilize composition over inheritance:**
	- Rely on objects that are composed of other objects, as this allows for adding new behavior by composing different objects together, without modifying existent classes.
- **Apply design patterns:**
	- Design patterns like Strategy, Factory, and Decorator, as they provide flexible and extensible structures for our code.

#### **LSP (Liskov Substitution Principle)**
*Objects of a superclass should be replaceable with objects of its subclasses without breaking functionality.*

Subclasses should extend base behavior rather than replacing it entirely, ensuring compatibility with code that works with the superclass.

*Set of formal requirements for subclases, and specifically for their methods, for when trying to adhere to _LSP_:*
- Parameter types in a method of *subclass* should **match** or be **more abstract**.
- The return type in a *subclass* should **match** or be a **subtype** of the return type in the method of the superclass.
- Method in subclass should NOT throw types of exceptions which the base method is not expected to throw.
- Subclass should NOT **strengthen** pre-conditions
	- For instance, in case the there is a superclass method that expects an `int` as a parameter, and if it is overridden, it shouldn't be enforced that that `int` should be positive only, and throw exceptions in case it ain't.
- Subclass should NOT **weaken** pre-conditions
	- For instance, in case there is a superclass method that works with the DB and it closes DB the connections upon returning the value, then there must NOT be created a subclass overriding it and making it leave connections open, as it might lead to ghost connections.
- Invariants of the superclass must be preserved, so new fields can be added to a class rather than messing around with existing ones.
- Subclass shouldn't change values of `private` fields of the superclass
	- In some other programming languages like Python or Javascript `private` methods are accessible via reflection mechanisms.

#### **ISP (Interface Segregation Principle)**
*Clients shouldn't be forced to depend on methods they don't use.*

Interfaces should be narrow and specific, allowing client classes to implement only the behaviors they actually need.

#### **DIP (Dependency Inversion Principle)**
*High-level classes shouldn't depend on low-level classes. Both should depend on abstractions.*

Both high-level and low-level classes should depend on abstractions (interfaces), not concrete implementations, allowing for flexible and testable code.

## **Project Idea**

The project implements a flexible number sorting application that can:
- Accept input via command line arguments or interactive terminal input
- Validate numbers using different validation strategies
- Sort numbers using various algorithms (Bubble, Insertion, Selection, Quick, or Default)
- Handle different input formats and provide algorithm selection

This way the architecture can demonstrates all SOLID principles through:
- Separated concerns for input handling, validation, and sorting
- Extensible sorting algorithms without modifying existing code
- Interchangeable input handlers and validators
- Focused interfaces for specific responsibilities
- Dependency injection and abstraction-based design

## **Usage**

### **Command Line Usage:**
```bash
dotnet run --input "3.14 42 1.5 -7 99" --sort-algorithm bubble
dotnet run -i "10,20,5,15" -s quick
```

### **Interactive Terminal Usage:**
```bash
dotnet run
# Enter numbers when prompted
# Enter sorting algorithm name when prompted
```

## **Implemented SOLID Principles**

### **1. Single Responsibility Principle (SRP)**

Each class has a single, well-defined responsibility:

#### **Sorting Classes**
```csharp
// Each sorting algorithm has one responsibility: implementing a specific sorting strategy
public class BubbleSort : INumberSort
{
    public List<double> Sort(List<double> numbers) // Only responsible for bubble sort logic
    {
        List<double> sorted = [.. numbers];
        int n = sorted.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (sorted[j] > sorted[j + 1])
                    (sorted[j], sorted[j + 1]) = (sorted[j + 1], sorted[j]);
        return sorted;
    }
}
```

#### **Input Handlers**
```csharp
// CommandInputHandler: Only responsible for parsing command line arguments
public class CommandInputHandler(IInputValidator validator, string[] args) : IInputHandler
{
    // Single responsibility: Extract values from command line arguments
    private string? GetArgumentValue(string longForm, string shortForm) { ... }
}

// TerminalInputHandler: Only responsible for interactive console input
public class TerminalInputHandler(IInputValidator validator) : IInputHandler
{
    // Single responsibility: Get input from console interaction
}
```

**Justification**: Each sorting class implements only one specific sorting algorithm. Input handlers are separated based on their input source (command line vs. terminal). Validators have distinct responsibilities for different types of validation.

### **2. Open/Closed Principle (OCP)**

The system is open for extension but closed for modification:

#### **Adding New Sorting Algorithms**
```csharp
// New algorithms can be added without modifying existing code
public class QuickSort : INumberSort // Extends without modifying
{
    public List<double> Sort(List<double> numbers)
    {
        List<double> sorted = [.. numbers];
        QuickSortRecursive(sorted, 0, sorted.Count - 1);
        return sorted;
    }
    // Implementation details...
}
```

The `SortingService` can accommodate new algorithms without modification:
```csharp
public class SortingService
{
    private INumberSort _numberSort = new DefaultSort();
    
    // Open for extension - can accept any INumberSort implementation
    public void SetNumberSortAlgo(INumberSort numberSort) 
        => _numberSort = numberSort;
}
```

#### **Adding New Input Validators**
```csharp
// RegexInputValidator extends validation capability without changing existing validators
public class RegexInputValidator : INumberInputValidator
{
    public List<double> ValidateNumbers(string input)
    {
        var numberMatches = Regex.Matches(input, @"(?<![A-Za-z0-9.-])-?[0-9]+(\.[0-9]+)?(?![A-Za-z0-9.])");
        // Advanced regex-based validation logic
    }
}
```

**Justification**: The `INumberSort` interface allows new sorting algorithms to be added without modifying existing code. The strategy pattern used in `SortingService` enables algorithm switching at runtime. New validators can be created by implementing existing interfaces.

### **3. Dependency Inversion Principle (DIP)**

High-level classes depend on abstractions, not concretions:

#### **SortingService depends on abstraction**
```csharp
public class SortingService  // High-level class
{
    private INumberSort _numberSort;  // Depends on abstraction, not concrete class
    
    public void SetNumberSortAlgo(INumberSort numberSort) // Accepts abstraction
        => _numberSort = numberSort;
        
    public List<double> Sort(List<double> numbers)
        => _numberSort.Sort(numbers);  // Uses abstraction
}
```

#### **Input Handlers depend on abstraction**
```csharp
public class CommandInputHandler(IInputValidator validator, string[] args) : IInputHandler
{
    private readonly IInputValidator _validator = validator;  // Depends on abstraction + Injected dependency on abstraction
}
```

**Justification**: High-level classes like `SortingService` and input handlers depend on interfaces rather than concrete classes. Dependencies are injected through constructors, allowing for easy testing and flexibility. The main application composes these abstractions, inverting the traditional dependency flow.

## **Results**

The implementation successfully demonstrates quite well at least 3 of the SOLID principles:

1. **SRP**: Each class has a single, clear responsibility
2. **OCP**: New sorting algorithms and validators can be added without modifying existing code
3. **DIP**: High-level classes depend on abstractions, enabling flexible dependency injection

The application can handle various input methods, multiple sorting algorithms, and different validation strategies, all while maintaining clean separation of concerns and extensibility.

### **Example Output:**

##### Using the Command Line Input Handler
```shell
> dotnet run -i "42 3.14 -7 99 1.5" -s bubble
Identified numbers: 42, 3.14, -7, 99, 1.5
Using BubbleSort...
Result: -7, 1.5, 3.14, 42, 99
```

##### Using the Interactive Terminal Input Handler
```shell
> dotnet run
+ Numbers: weoisorew34wr, rwerewrwqw 432423.432. 32 1 33. 32.324, 231.0 , 230 0 0 22 - 32 , -1234,,
Identified numbers: 32, 1, 32.324, 231, 230, 0, 0, 22, 32, -1234
Choose a sorting algorithm:
        Bubble
        Insertion
        Selection
        Quick
+ Choice: quick
Using QuickSort...
Result: -1234, 0, 0, 1, 22, 32, 32, 32.324, 230, 231
```

## **Conclusions**

To sum up, through the development of this number sorting application, I gained valuable hands-on experience implementing SOLID principles. This simple pproject helped me to successfully demonstrate three core SOLID principles while providing hints of the remaining two.

The Single Responsibility Principle became the foundation of my design approach. By ensuring each class had only one reason to change, I created a system where sorting algorithms, input handlers, and validators each focus on their specific domain. This separation made the codebase significantly easier to understand and maintain. When I needed to add a new sorting algorithm like `QuickSort`, I didn't have to worry about breaking existing functionality because each component was self-contained and focused.

The Open/Closed Principle proved to be particularly powerful in practice. My implementation allows for seamless extension without modification of existing code. Adding new sorting algorithms requires only implementing the `INumberSort` interface, and the `SortingService` can immediately work with them through the strategy pattern. This extensibility became evident when I could introduce different validation mechanisms without touching the core input handling logic. The system grows naturally without breaking existing functionality.

Dependency Inversion Principle transformed how I think about class relationships. Instead of high-level classes depending on concrete implementations, I inverted these dependencies by introducing abstractions. The `SortingService` depends on the `INumberSort` interface rather than specific sorting classes, and input handlers receive their validators through constructor injection. This inversion made the system more flexible, as I can easily swap implementations without changing the high-level business logic.

Overall, this laboratory work reinforced my understanding that SOLID principles are not just theoretical concepts but practical tools that lead to more maintainable, flexible, and robust software architecture. The investment in proper design pays well when extending functionality or debugging issues.