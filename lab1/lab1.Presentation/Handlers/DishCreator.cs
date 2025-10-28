using lab1.Domain.Builders;
using lab1.Domain.Products;
using lab1.Presentation.Views;

namespace lab1.Presentation.Handlers;

public class DishCreator(IMenuView view)
{
    private readonly IMenuView _view = view;


    public IPrototype CreatePizza()
    {
        var builder = PizzaBuilder.Empty();
        return BuildDish(builder, "Pizza");
    }

    public IPrototype CreateCalzone()
    {
        var builder = CalzoneBuilder.Empty();
        return BuildDish(builder, "Calzone");
    }

    private IPrototype BuildDish(IFoodBuilder builder, string dishType)
    {
        var name = _view.GetInput($"Enter {dishType} name");
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
        {
            builder.AddExtras(extra);
        }

        return builder.Cook();
    }
}