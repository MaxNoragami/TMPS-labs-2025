using lab1.Enums;
using lab1.Products;

namespace lab1.Builders;

public class PizzaBuilder : IPizzaBuilder
{
    private string _name = string.Empty;
    private FoodSize _size;
    private DoughType _dough;
    private SauceType? _sauce;
    private CheeseType? _cheese;
    private List<Toppings> _toppings = [];


    private PizzaBuilder() { }

    public static IPizzaBuilder Empty() => new PizzaBuilder();
    public void Reset()
    {
        _name = string.Empty;
        _size = default;
        _dough = default;
        _sauce = null;
        _cheese = null;
        _toppings = [];
    }

    public IProduct Cook()
    {
        var pizza = new Pizza()
        {
            Name = _name,
            Size = _size,
            Dough = _dough,
            Sauce = _sauce,
            Cheese = _cheese,
            Toppings = [.. _toppings]
        };

        Reset();

        return pizza;
    }

    public IPizzaBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public IPizzaBuilder SetSize(FoodSize size)
    {
        _size = size;
        return this;
    }

    public IPizzaBuilder SetDough(DoughType dough)
    {
        _dough = dough;
        return this;
    }

    public IPizzaBuilder AddSauce(SauceType sauce)
    {
        _sauce = sauce;
        return this;
    }

    public IPizzaBuilder AddCheese(CheeseType cheese)
    {
        _cheese = cheese;
        return this;
    }

    public IPizzaBuilder AddToppings(Toppings toppings)
    {
        _toppings.Add(toppings);
        return this;
    }
}
