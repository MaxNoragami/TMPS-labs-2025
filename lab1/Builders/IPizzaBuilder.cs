using lab1.Enums;
using lab1.Products;

namespace lab1.Builders;

public interface IPizzaBuilder
{
    IPizzaBuilder SetName(string name);
    IPizzaBuilder SetSize(FoodSize size);
    IPizzaBuilder SetDough(DoughType dough);
    IPizzaBuilder AddSauce(SauceType sauce);
    IPizzaBuilder AddCheese(CheeseType cheese);
    IPizzaBuilder AddToppings(Toppings toppings);
    IProduct Cook();
    void Reset();
}
