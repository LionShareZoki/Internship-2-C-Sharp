var products = new Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>()
{
    {"Riza 500g",(DateOnly.FromDateTime(DateTime.Now.AddDays(600)), 26, 7, 3.2m)},
    {"Brasno 1kg",(DateOnly.FromDateTime(DateTime.Now.AddDays(356)), 26, 7, 2.1m)},
    {"Coca Cola 500ml",(DateOnly.FromDateTime(DateTime.Now.AddDays(-100)), 26, 7, 1m)},
};

var mainMenuItems = new List<(int Id, string Name)>
{
    (1, "Artikli"),
    (2, "Radnici"),
    (3, "Racuni"),
    (4, "Statistika"),
    (5, "Izlaz iz aplikacije")
};

static void ProductsMenu(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, int Price)>? products)
{
    var productMenuItems = new List<(int Id, string Name)>
    {
        (1, "Unos artikla"),
        (2, "Brisanje artikla"),
        (3, "Uredivanje artikla"),
        (4, "Ispis artikla"),
        (5, "Glavni izbornik")
    };

    switch (DisplayMenuAndPick(productMenuItems))
    {
        case 1:
            AddProductAction(products);
            break;
        case 2:
            DeleteProductAction(products);
            break;
        case 3:
            UpdateProductAction(products);
            break;
        case 4:
            PrintProductAction(products);
            break;
        case 5:
            Console.WriteLine("glavni izb");
            break;

    }

    static void AddProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, int Price)>? products)
    {


        Console.WriteLine("Unesite ime proizvoda:");
        var name = Console.ReadLine();

        Console.WriteLine("Unesite datum isteka proizvoda (yyyy-MM-dd):");
        if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOfExpiry))
        {
            Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
            return;
        }

        Console.WriteLine("Unesite dostupnu količinu proizvoda:");
        if (!int.TryParse(Console.ReadLine(), out int availableAmount))
        {
            Console.WriteLine("Invalid input for available amount. Please enter a valid integer.");
            return;
        }

        Console.WriteLine("Unesite cijenu proizvoda");
        if(!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Invalid input for price. Please enter a valid intefer");
            return;
        }

        products.Add(name, (dateOfExpiry, availableAmount, 0, price));

        Console.WriteLine($"{name} usješno je dodan u proizvode");
       
        Console.ReadLine();
    }

    static void DeleteProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, int Price)>? products)
    {
        var deleteProductMenuItems = new List<(int Id, string Name)>
    {
        (1, "Po imenu artikla"),
        (2, "Sve one kojima je istekao datum trajanja"),
    };

        switch (DisplayMenuAndPick(deleteProductMenuItems))
        {
            case 1:
                Console.WriteLine("Izaberite proizvod koji želite ukloniti");
                var name = Console.ReadLine();

                products.Remove(name);

                Console.WriteLine($"{name} uspješno je uklonjen iz proizvoda");
                Console.ReadKey();
                break;
            case 2:
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

                foreach (var item in products)
                {
                    if(item.Value.DateOfExpiry <= currentDate) products.Remove(item.Key);
                    
                }
                Console.WriteLine("Proizvodi s isteklim datumom trajanja uspješno su uklonjeni");
                Console.ReadKey();

                break;
        }



    }
}

static void WorkersMenu()
{
    throw new NotImplementedException();
}

static void BillsMenu()
{
    throw new NotImplementedException();
}

static void StatisticsMenu()
{
    throw new NotImplementedException();
}



static int DisplayMenuAndPick(List<(int Id, string Name)> menuItems)
{
    while (true)
    {
        Console.Clear();
        foreach (var menuItem in menuItems)
        {
            Console.WriteLine($"{menuItem.Id} {menuItem.Name}");
        }
        Console.Write("Select one of the options: ");

        var userInput = Console.ReadLine();
        int? parsedInput = null;

        try
        {
            parsedInput = int.Parse(userInput);
        }
        catch (Exception)
        {
            Console.WriteLine($"Invalid input: Cannot parse '{userInput}'.");
            Console.ReadKey();
            continue;
        }

        if (!menuItems.Any(item => item.Id == parsedInput))
        {
            Console.WriteLine($"Invalid input: Id {parsedInput} does not exist.");
            Console.ReadKey();
            continue;
        }

        return parsedInput.Value;
    }

}

while (true)
{
    switch (DisplayMenuAndPick(mainMenuItems))
    {
        case 1:
            ProductsMenu(products);
            break;
        case 2:
            WorkersMenu();
            break;
        case 3:
            BillsMenu();
            break;
        case 4:
            StatisticsMenu();
            break;
        case 5:
            return;
        default:
            throw new InvalidOperationException("This should never happen.");
    }
}