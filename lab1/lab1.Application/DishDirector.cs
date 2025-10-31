using lab1.Domain.Builders;
using lab1.Domain.Enums;

namespace lab1.Application;

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