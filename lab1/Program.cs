using lab1.Builders;
using lab1.Enums;

var pizza = PizzaBuilder.Empty()
    .SetName("Margherita")
    .SetSize(FoodSize.Large)
    .SetDough(DoughType.Sicilian)
    .AddCheese(CheeseType.Gorgonzola)
    .AddToppings(Toppings.Artichokes)
    .AddToppings(Toppings.Spinach)
    .AddToppings(Toppings.Zucchini)
    .Cook();

Console.WriteLine(pizza.ToString());
Console.WriteLine(pizza.GetType());