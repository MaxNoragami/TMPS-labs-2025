using lab1.Builders;
using lab1.Enums;

var pizza = PizzaBuilder.Empty()
    .SetName("Margherita")
    .SetSize(FoodSize.Large)
    .SetDough(DoughType.Sicilian)
    .AddCheese(CheeseType.Gorgonzola)
    .AddExtras(Extras.Artichokes)
    .AddExtras(Extras.Spinach)
    .AddExtras(Extras.Zucchini)
    .Cook();

var calzone = CalzoneBuilder.Empty()
    .SetName("SausageAndSpinach")
    .SetSize(FoodSize.Medium)
    .SetDough(DoughType.GlutenFree)
    .AddSauce(SauceType.Alfredo)
    .AddExtras(Extras.Sausage)
    .AddExtras(Extras.Spinach)
    .Cook();

Console.WriteLine(pizza.ToString());
Console.WriteLine(pizza.GetType());

Console.WriteLine(calzone.ToString());
Console.WriteLine(calzone.GetType());