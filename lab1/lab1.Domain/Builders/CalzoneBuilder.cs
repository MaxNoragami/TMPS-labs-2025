using lab1.Domain.Enums;
using lab1.Domain.Products;

namespace lab1.Domain.Builders;

public class CalzoneBuilder : IFoodBuilder
{
    private string _name = string.Empty;
    private FoodSize _size;
    private DoughType _dough;
    private SauceType? _sauce;
    private CheeseType? _cheese;
    private List<Extras> _extras = [];


    private CalzoneBuilder() { }

    public static IFoodBuilder Empty() => new CalzoneBuilder();
    public void Reset()
    {
        _name = string.Empty;
        _size = default;
        _dough = default;
        _sauce = null;
        _cheese = null;
        _extras = [];
    }

    public IPrototype Cook()
    {
        var pizza = new Calzone()
        {
            Name = _name,
            Size = _size,
            Dough = _dough,
            Sauce = _sauce,
            Cheese = _cheese,
            Fillings = [.. _extras]
        };

        Reset();

        return pizza;
    }

    public IFoodBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public IFoodBuilder SetSize(FoodSize size)
    {
        _size = size;
        return this;
    }

    public IFoodBuilder SetDough(DoughType dough)
    {
        _dough = dough;
        return this;
    }

    public IFoodBuilder AddSauce(SauceType sauce)
    {
        _sauce = sauce;
        return this;
    }

    public IFoodBuilder AddCheese(CheeseType cheese)
    {
        _cheese = cheese;
        return this;
    }

    public IFoodBuilder AddExtras(Extras extras)
    {
        _extras.Add(extras);
        return this;
    }
}
