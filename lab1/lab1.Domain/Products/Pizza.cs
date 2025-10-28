using lab1.Domain.Enums;

namespace lab1.Domain.Products;

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

    public override string ToString()
    {
        var toppings = Toppings.Any() ? string.Join(", ", Toppings) : "No toppings";
        var sauce = Sauce.HasValue ? Sauce.Value.ToString() : "No sauce";
        var cheese = Cheese.HasValue ? Cheese.Value.ToString() : "No cheese";

        return $"{Name} Pizza:\n" +
               $"  Size: {Size}\n" +
               $"  Dough: {Dough}\n" +
               $"  Sauce: {sauce}\n" +
               $"  Cheese: {cheese}\n" +
               $"  Toppings: {toppings}";
    }

}
