using lab1.Domain.Builders;
using lab1.Domain.Products;
using lab1.Presentation.Views;

namespace lab1.Presentation.Handlers;

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