using lab1.Builders;
using lab1.Enums;
using lab1.Prototypes;

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

var menu = new PrototypeRegistry();
var chef = new PrototypeFactory(menu);

menu.Register("Margherita", pizza);
menu.Register("AmericanCalzone", calzone);

var pizza2 = chef.Create("margherita ");
var calzone2 = chef.Create("  aMericanCalzone");

Console.WriteLine(pizza2.ToString());
Console.WriteLine(pizza2.GetType());

Console.WriteLine(calzone2.ToString());
Console.WriteLine(calzone2.GetType());