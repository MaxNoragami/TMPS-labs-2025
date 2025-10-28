using lab1.Domain.Enums;

namespace lab1.Presentation.Views;

public class ConsoleMenuView : IMenuView
{
    public void DisplayWelcome()
    {
        Console.Clear();
        Console.WriteLine();
        PrintPapaLouie();
        Console.WriteLine("\n╔════════════════════════════════════╗");
        Console.WriteLine("║   Welcome to Papa's Pizzeria!      ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.WriteLine();
    }

    public void DisplayMainMenu()
    {
        Console.WriteLine("\n┌─────────── Main Menu ────────────┐");
        Console.WriteLine("│ 1. View Menus                    │");
        Console.WriteLine("│ 2. Order from Menu               │");
        Console.WriteLine("│ 3. Create Custom Dish            │");
        Console.WriteLine("│ 4. Add to Custom Menu            │");
        Console.WriteLine("│ 5. Add to Global Menu (Admin)    │");
        Console.WriteLine("│ 6. Switch User                   │");
        Console.WriteLine("│ 7. Exit                          │");
        Console.WriteLine("└──────────────────────────────────┘");
    }

    public void DisplayUserInfo(string username, string role)
    {
        Console.WriteLine($"Current User: {username} ({role})");
    }

    public void DisplayMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {message}");
        Console.ResetColor();
    }

    public void DisplayError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ Error: {error}");
        Console.ResetColor();
    }

    public void DisplayDish(string dish)
    {
        Console.WriteLine("\n" + dish);
    }

    public void DisplayMenuItems(Dictionary<string, string> items, string menuType)
    {
        Console.WriteLine($"\n┌─── {menuType} Menu ───");
        if (!items.Any())
        {
            Console.WriteLine("│ (No items available)");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine($"│ • {item.Key}");
            }
        }
        Console.WriteLine("└────────────────────────");
    }

    public int GetMenuChoice()
    {
        Console.Write("\nEnter your choice: ");
        if (int.TryParse(Console.ReadLine(), out int choice))
            return choice;
        return -1;
    }

    public string GetInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public FoodSize GetFoodSize()
    {
        Console.WriteLine("\nSelect Size:");
        Console.WriteLine("1. Small");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. Large");
        Console.Write("Choice: ");

        return Console.ReadLine() switch
        {
            "1" => FoodSize.Small,
            "2" => FoodSize.Medium,
            "3" => FoodSize.Large,
            _ => FoodSize.Medium
        };
    }

    public DoughType GetDoughType()
    {
        Console.WriteLine("\nSelect Dough:");
        Console.WriteLine("1. Classic");
        Console.WriteLine("2. Gluten Free");
        Console.WriteLine("3. Sicilian");
        Console.Write("Choice: ");

        return Console.ReadLine() switch
        {
            "1" => DoughType.Classic,
            "2" => DoughType.GlutenFree,
            "3" => DoughType.Sicilian,
            _ => DoughType.Classic
        };
    }

    public SauceType? GetSauceType()
    {
        Console.WriteLine("\nSelect Sauce (or press Enter to skip):");
        Console.WriteLine("1. Tomato");
        Console.WriteLine("2. Alfredo");
        Console.WriteLine("3. BBQ");
        Console.Write("Choice: ");

        return Console.ReadLine() switch
        {
            "1" => SauceType.Tomato,
            "2" => SauceType.Alfredo,
            "3" => SauceType.BBQ,
            _ => null
        };
    }

    public CheeseType? GetCheeseType()
    {
        Console.WriteLine("\nSelect Cheese (or press Enter to skip):");
        Console.WriteLine("1. Cheddar");
        Console.WriteLine("2. Gorgonzola");
        Console.WriteLine("3. Parmesan");
        Console.WriteLine("4. Provolone");
        Console.Write("Choice: ");

        return Console.ReadLine() switch
        {
            "1" => CheeseType.Cheddar,
            "2" => CheeseType.Gorgonzola,
            "3" => CheeseType.Parmesan,
            "4" => CheeseType.Provolone,
            _ => null
        };
    }

    public List<Extras> GetExtras()
    {
        var extras = new List<Extras>();
        Console.WriteLine("\nAdd Extras (enter number, press Enter when done):");

        var extrasArray = Enum.GetValues<Extras>();
        for (int i = 0; i < extrasArray.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {extrasArray[i]}");
        }

        while (true)
        {
            Console.Write("Add extra (or press Enter to finish): ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                break;

            if (int.TryParse(input, out int choice) && choice > 0 && choice <= extrasArray.Length)
            {
                extras.Add(extrasArray[choice - 1]);
                Console.WriteLine($"Added: {extrasArray[choice - 1]}");
            }
        }

        return extras;
    }

    public bool GetConfirmation(string prompt)
    {
        Console.Write($"{prompt} (y/n): ");
        var response = Console.ReadLine()?.Trim().ToLower();
        return response == "y" || response == "yes";
    }

    private void PrintPapaLouie()
    {
        Console.WriteLine("\r\n\r\n                                    **#*******                                  \r\n                                **::::::::::::::**                              \r\n                               ::::::           :::*                            \r\n                              :::::: . . . . . . .::*                           \r\n                             *::::.::...... . .    .:*                          \r\n                             *::::::::::::::::::. . :#  *****                   \r\n                              #::::::::.:::::::::::::*:::::::::+                \r\n                               *:::::.:::.:.:.::::::::::      .::               \r\n                                 #*#:::::::::::.:.::-::: . . . .::              \r\n                                    *#:::::::::::::::.:::::. .  ::*             \r\n                                  *:::::::::::::::::::::::::::::::*             \r\n                                 =::::::::-:::...::::::.:::::::::#*#*           \r\n                                 ===:::--:::  . .   :::::::::::::*:::::*        \r\n                                 **===*::: .:.   : ::::-::::::::::::  .::*      \r\n                                    *:::: ..   .  :::=::=::::::::::: . .::      \r\n                                   *:::. : . .: .::=:::=*:::::::::::::::::*     \r\n                                  =:::  :    ..:::=:::==*:::::::::::::.::=      \r\n                                *::::  : .....::-::::*=*=:::::::#::::::*#       \r\n                               #:::: .. . :  ::=:::=*=*===:::::::*              \r\n                      @@%    *+@@@@+=====**.::=:::===*====:::::::*              \r\n                   @%%@   *=-----%%%@=--====**:::+==**======::::-#              \r\n                  %%%@  *---------+%%%@----====*+==*  *=========*               \r\n                 @%%@  =-----------%%%@------===*=*    *=======*                \r\n                 @@@  ----:-:-:-:-:-=@=-------===*                              \r\n                     =-----------------:-------===*                             \r\n                    *---:-%%%--#%:%--:---:------===                             \r\n                    *-----%:=%-%::%=---:--------===*                            \r\n                    =-----=%%:--%%*--:---:-:----===*                            \r\n                    *-----::--:--%@@@@@---------===*                            \r\n                    *---@%%%%%=@%%%%%#%%@-:-----===                             \r\n                     +=@%%%%%%@%%%%%%%%%%@-----===*                             \r\n                     -@%%%%#%%@%%%%%%%%%%%@----==*                              \r\n              @@%  *@%%%%#%%%%@@@%%%%%%%%%%%%@@@%%%@                            \r\n              @@%%%%%%%%%%%%@@*-*@@@%%%%%%%%%%%%%@@                             \r\n               @@@@%%%%%@@@@@==----@@@@@@@@@@@@@@                               \r\n                  @@@@@@@@   ***========+**                                     \r\n                                      ====*                                     \r\n                                      =--=*      @@@                            \r\n                                    *%=---#@@@%%%%%@                            \r\n                                    %@@@@@@%@%#%@@                              \r\n                                   *@@%%%%%@@%%%%@                              \r\n                                  *===::::::== %@%*                             \r\n                                  =::==.=.:::**  %                              \r\n                                 *:::== -::::::*                                \r\n\r\n");
    }
}