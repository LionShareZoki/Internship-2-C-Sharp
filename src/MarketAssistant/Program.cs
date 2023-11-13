var products = new Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>()
{
    {"Riza 500g",(DateOnly.FromDateTime(DateTime.Now.AddDays(600)), 40, 21, 3.1m)},
    {"Brasno 1kg",(DateOnly.FromDateTime(DateTime.Now.AddDays(356)), 27, 50, 2.2m)},
    {"Coca Cola 500ml",(DateOnly.FromDateTime(DateTime.Now.AddDays(-100)), 28, 1, 1)},
};

var employees = new Dictionary<string, DateOnly>
{
    {"Ivan Matić", DateOnly.FromDateTime(new DateTime(1999, 2, 21)) },
    {"Ivana Ivanić", DateOnly.FromDateTime(new DateTime(1989, 4, 5)) },
    {"Petar Marković", DateOnly.FromDateTime(new DateTime(2001, 10, 1)) }
};

var mainMenuItems = new List<(int Id, string Name)>
{
    (1, "Artikli"),
    (2, "Radnici"),
    (3, "Racuni"),
    (4, "Statistika"),
    (5, "Izlaz iz aplikacije")
};

static void ProductsMenu(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products)
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
            return;

    }

    static void AddProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products)
    {


        Console.WriteLine("Unesite ime proizvoda:");
        var name = Console.ReadLine();

        Console.WriteLine("Unesite datum isteka proizvoda (yyyy-MM-dd):");
        if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOfExpiry))
        {
            Console.WriteLine("Neispravan format datuma. Molim vas koristite yyyy-MM-dd.");
            return;
        }

        Console.WriteLine("Unesite dostupnu količinu proizvoda:");
        if (!int.TryParse(Console.ReadLine(), out int availableAmount))
        {
            Console.WriteLine("Neispravan unos za količinu. Molim vas koristite cijeli broj.");
            return;
        }

        Console.WriteLine("Unesite cijenu proizvoda");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Neispravan unos za cijenu. Molim vas unesite broj.");
            return;
        }

        products.Add(name, (dateOfExpiry, availableAmount, 0, price));

        Console.WriteLine($"{name} usješno je dodan u proizvode");
       
        Console.ReadLine();
    }

    static void DeleteProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products)
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
                    if (item.Value.DateOfExpiry <= currentDate) products.Remove(item.Key);
                    
                }
                Console.WriteLine("Proizvodi s isteklim datumom trajanja uspješno su uklonjeni");
                Console.ReadKey();

                break;
        }



    }

    static void UpdateProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products)
    {
        var updateProductMenuItems = new List<(int Id, string Name)>
        {
        (1, "Zasebno uređivanje proizvoda"),
        (2, "Popopust/poskupljenje na sve proizvode unutar trgovine"),
        };

        var updateProductPartiallyMenuItems = new List<(int Id, string Name)>
        {
        (1, "Promijeni količinu proizvoda"),
        (2, "Promijeni količinu prodanih proizvoda"),
        (3, "Promijeni cijenu proizvoda")
        };

        switch (DisplayMenuAndPick(updateProductMenuItems))
        {
            case 1:
                Console.WriteLine("Izaberite proizvod koji želite urediti");
                var name = Console.ReadLine();

                switch (DisplayMenuAndPick(updateProductPartiallyMenuItems))
                {
                    case 1:
                        Console.WriteLine("Unesite trenutnu dostupnu količinu proizvoda");
                        var newAmountInput = Console.ReadLine();

                        if (!string.IsNullOrEmpty(newAmountInput))
                        {
                            if (int.TryParse(newAmountInput, out var newAmount))
                            {
                                products[name] = (products[name].DateOfExpiry, newAmount, products[name].SoldAmount, products[name].Price);
                            }
                            else
                            {
                                Console.WriteLine("Neispravan unos. Molimo unesite cijeli broj za količinu.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unos ne smije biti prazan.");
                            Console.ReadKey();
                        }

                        Console.WriteLine($"Uspješno promijenjena količina {name}");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.WriteLine($"Unesite koliko {name} je prodano");
                        var newSoldAmountInput = Console.ReadLine();

                        if (!string.IsNullOrEmpty(newSoldAmountInput))
                        {
                            if (int.TryParse(newSoldAmountInput, out var newSoldAmount))
                            {
                                products[name] = (products[name].DateOfExpiry, products[name].AvailableAmount, newSoldAmount, products[name].Price);
                            }
                            else
                            {
                                Console.WriteLine("Neispravan unos. Molimo unesite cijeli broj za količinu.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unos ne smije biti prazan.");
                            Console.ReadKey();
                        }

                        Console.WriteLine($"Uspješno promijenjena količina prodanih {name}");
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine($"Unesite novu cijenu za {name}");
                        var newPriceInput = Console.ReadLine();

                        if (!string.IsNullOrEmpty(newPriceInput))
                        {
                            if (int.TryParse(newPriceInput, out var newPrice))
                            {
                                products[name] = (products[name].DateOfExpiry, products[name].AvailableAmount, products[name].SoldAmount, newPrice);
                            }
                            else
                            {
                                Console.WriteLine("Neispravan unos. Molimo unesite cijeli broj za cijenu.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unos ne smije biti prazan.");
                            Console.ReadKey();
                        }

                        Console.WriteLine($"Uspješno promijenjena cijena {name}");
                        Console.ReadKey();
                        break;
                }

                break;
            case 2:
                Console.WriteLine("Unesite promijenu cijene svih proizvoda u postotku\n(negativno u slučaju popusta, pozitivno u slučaju poskupljenja))");
                var priceChangeInput = Console.ReadLine();

                if (int.TryParse(priceChangeInput, out var priceChange) && priceChange >= 1 && priceChange <= 100)
                {
                    foreach (var item in products)
                    {
                        decimal newPrice = item.Value.Price * (1 + ((decimal)priceChange / 100));

                        products[item.Key] = (products[item.Key].DateOfExpiry, products[item.Key].AvailableAmount, products[item.Key].SoldAmount, newPrice);
                    }

                    Console.WriteLine($"Uspješno promijenjena cijena svih proizvoda za {priceChange}%");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Neispravan unos. Molimo unesite cijeli broj između 1 i 100.");
                    Console.ReadKey();
                }
                break;

        }

    }

    static void PrintProductAction(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products)
    {
        var printProductMenuItems = new List<(int Id, string Name)>

    {
        (1, "Svi artikli"),
        (2, "Svi artikli sortirani po imenu"),
        (3, "Svi artikli sortirani po datumu silazno"),
        (4, "Svi artikli sortirani po datumu uzlazno"),
        (5, "Svi artikli sortirani po količini"),
        (6, "Najprodavaniji artikl"),
        (7, "Najmanje prodavan artikl")
    };
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        switch (DisplayMenuAndPick(printProductMenuItems))
        {

            case 1:
                foreach (var item in products)
                {

                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");
                }
                Console.ReadKey();
                break;
            case 2:
                foreach (var item in products.OrderBy(x => x.Key))
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");
        }
        Console.ReadKey();
                break;
            case 3:
                var sortedProductsDescending = products.OrderByDescending(x => (x.Value.DateOfExpiry.DayNumber - currentDate.DayNumber)).ToDictionary(x => x.Key, x => x.Value);
                foreach (var item in sortedProductsDescending)
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");

                }
                Console.ReadKey();
                break;
            case 4:
                var sortedProductsAscending = products.OrderBy(x => (x.Value.DateOfExpiry.DayNumber - currentDate.DayNumber)).ToDictionary(x => x.Key, x => x.Value);
                foreach (var item in sortedProductsAscending)
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");

                }
                Console.ReadKey();
                break;
            case 5:
                var sortedProductsByAvailableAmount = products.OrderBy(x => x.Value.AvailableAmount).ToDictionary(x => x.Key, x => x.Value);
                foreach (var item in sortedProductsByAvailableAmount)
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");

                }
                Console.ReadKey();
                break;
            case 6:
                var productSoldTheMost = products.OrderByDescending(x => x.Value.SoldAmount).FirstOrDefault();
                if (productSoldTheMost.Value != default)
                {
                    DateOnly expiryDate = productSoldTheMost.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber);
                    Console.WriteLine($"Najprodavaniji proizvod {productSoldTheMost.Key} - Prodan: {productSoldTheMost.Value.SoldAmount} puta - {daysUntilExpiry} dana do isteka datuma trajanja");
                }
                Console.ReadKey();
                break;
            case 7:
                var productSoldTheLeast = products.OrderBy(x => x.Value.SoldAmount).FirstOrDefault();
                if (productSoldTheLeast.Value != default)
                {
                    DateOnly expiryDate = productSoldTheLeast.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber);
                    Console.WriteLine($"Najmanje prodavan proizvod: {productSoldTheLeast.Key} - Prodan: {productSoldTheLeast.Value.SoldAmount} puta - {daysUntilExpiry} dana do isteka datuma trajanja");
                }
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
        Console.Write("Izaberite jednu od opcija: ");

        var userInput = Console.ReadLine();
        int? parsedInput = null;

        try
        {
            parsedInput = int.Parse(userInput);
        }
        catch (Exception)
        {
            Console.WriteLine($"Neispravan unos: Ne mogu parsirati '{userInput}'.");
            Console.ReadKey();
            continue;
        }

        if (!menuItems.Any(item => item.Id == parsedInput))
        {
            Console.WriteLine($"Neispravan unos: Id {parsedInput} ne postoji.");
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
            WorkersMenu(employees);
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