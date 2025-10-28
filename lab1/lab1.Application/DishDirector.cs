using lab1.Domain.Builders;
using lab1.Domain.Enums;
using lab1.Domain.Products;

namespace lab1.Application;

public class DishDirector
{
    public IPrototype ConstructMargherita() =>
        ConstructPizza(
            "Margherita",
            FoodSize.Medium,
            DoughType.Classic,
            SauceType.Tomato,
            CheeseType.Provolone,
            [Extras.Spinach]);

    public IPrototype ConstructPepperoni() =>
        ConstructPizza(
            "Pepperoni",
            FoodSize.Large,
            DoughType.Classic,
            SauceType.Tomato,
            CheeseType.Cheddar,
            [Extras.Pepperoni, Extras.Olives]);

    public IPrototype ConstructHawaiian() =>
        ConstructPizza(
            "Hawaiian",
            FoodSize.Medium,
            DoughType.Classic,
            SauceType.Tomato,
            CheeseType.Provolone,
            [Extras.Ham, Extras.Pineapple]);

    public IPrototype ConstructCalzoneAmericano() =>
        ConstructCalzone(
            "Americano",
            FoodSize.Large,
            DoughType.Sicilian,
            SauceType.Alfredo,
            CheeseType.Cheddar,
            [Extras.Sausage, Extras.Bacon]);

    private IPrototype ConstructPizza(
        string name,
        FoodSize size,
        DoughType dough,
        SauceType? sauce,
        CheeseType? cheese,
        List<Extras> extras)
        => ConstructDish(PizzaBuilder.Empty(), name, size, dough, sauce, cheese, extras);

    private IPrototype ConstructCalzone(
        string name,
        FoodSize size,
        DoughType dough,
        SauceType? sauce,
        CheeseType? cheese,
        List<Extras> extras)
        => ConstructDish(CalzoneBuilder.Empty(), name, size, dough, sauce, cheese, extras);

    private IPrototype ConstructDish(
        IFoodBuilder builder,
        string name,
        FoodSize size,
        DoughType dough,
        SauceType? sauce,
        CheeseType? cheese,
        List<Extras> extras)
    {
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