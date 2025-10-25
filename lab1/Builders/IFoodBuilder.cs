using lab1.Enums;
using lab1.Prototypes;

namespace lab1.Builders;

public interface IFoodBuilder
{
    IFoodBuilder SetName(string name);
    IFoodBuilder SetSize(FoodSize size);
    IFoodBuilder SetDough(DoughType dough);
    IFoodBuilder AddSauce(SauceType sauce);
    IFoodBuilder AddCheese(CheeseType cheese);
    IFoodBuilder AddExtras(Extras extras);
    IPrototype Cook();
    void Reset();
}
