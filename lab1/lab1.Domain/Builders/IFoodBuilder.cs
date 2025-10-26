using lab1.Domain.Enums;
using lab1.Domain.Products;

namespace lab1.Domain.Builders;

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
