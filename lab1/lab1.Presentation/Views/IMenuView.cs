using lab1.Domain.Enums;

namespace lab1.Presentation.Views;

public interface IMenuView
{
    void DisplayWelcome();
    void DisplayMainMenu();
    void DisplayUserInfo(string username, string role);
    void DisplayMessage(string message);
    void DisplayError(string error);
    void DisplayDish(string dish);
    void DisplayMenuItems(Dictionary<string, string> items, string menuType);

    int GetMenuChoice();
    string GetInput(string prompt);
    FoodSize GetFoodSize();
    DoughType GetDoughType();
    SauceType? GetSauceType();
    CheeseType? GetCheeseType();
    List<Extras> GetExtras();
    bool GetConfirmation(string prompt);
}