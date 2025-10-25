using lab1.Enums;

namespace lab1.Prototypes;

public class Calzone : IPrototype
{
    public string Name { get; init; } = string.Empty;
    public FoodSize Size { get; init; }
    public DoughType Dough { get; init; }
    public SauceType? Sauce { get; init; }
    public CheeseType? Cheese { get; init; }
    public List<Extras> Fillings { get; init; } = [];

    public IPrototype Clone()
        => new Calzone()
        {
            Name = Name,
            Size = Size,
            Dough = Dough,
            Sauce = Sauce,
            Cheese = Cheese,
            Fillings = [.. Fillings]
        };

    public override string ToString()
    {
        var fillings = Fillings.Any() ? string.Join(", ", Fillings) : "No fillings";
        var sauce = Sauce.HasValue ? Sauce.Value.ToString() : "No sauce";
        var cheese = Cheese.HasValue ? Cheese.Value.ToString() : "No cheese";

        return $"{Name} Calzone:\n" +
               $"  Size: {Size}\n" +
               $"  Dough: {Dough}\n" +
               $"  Sauce: {sauce}\n" +
               $"  Cheese: {cheese}\n" +
               $"  Toppings: {fillings}";
    }

}
