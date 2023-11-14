using System.Diagnostics;
using System.Globalization;

string password = "1950";

var products = new Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>()
{
    {"Riza 500g",(DateOnly.FromDateTime(DateTime.Now.AddDays(600)), 40, 2, 3.1m)},
    {"Brasno 1kg",(DateOnly.FromDateTime(DateTime.Now.AddDays(356)), 27, 1, 2.2m)},
    {"Coca Cola 500ml",(DateOnly.FromDateTime(DateTime.Now.AddDays(-100)), 28, 1, 1)},
};

var employees = new Dictionary<string, DateOnly>
{
    {"Ivan Livaja", DateOnly.FromDateTime(new DateTime(1999, 2, 21)) },
    {"Ivana Baturina", DateOnly.FromDateTime(new DateTime(1989, 4, 5)) },
    {"Petar Marin", DateOnly.FromDateTime(new DateTime(2001, 10, 1)) }
};

var bills = new Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)>()
{
    {
        1, (new DateTime(2023, 11, 5), 3.1m, new List<(string, int, decimal)> { ("Riza 500g", 1, 3.1m) })
    },
    {
        2, (new DateTime(2023, 11, 5), 6.3m, new List<(string, int, decimal)>
        {
            ("Riza 500g", 1, 3.1m),
            ("Brasno 1kg", 1, 2.2m),
            ("Coca Cola 500ml", 1, 1m)
        })
    }
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

        Console.WriteLine("Jeste li sigurni da želite dodati proizvod? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
        var answer = Console.ReadLine();

        if (answer.ToUpper() == "DA")
        {
            products.Add(name, (dateOfExpiry, availableAmount, 0, price));
        }
        else return;
        Console.Clear();
        Console.WriteLine($"{name} usješno je dodan u proizvode");
        Console.ReadKey();
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

                Console.WriteLine("Jeste li sigurni da želite ukloniti proizvod? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                var answer = Console.ReadLine();

                if (answer.ToUpper() == "DA")
                {
                    products.Remove(name);
                }
                else return;
                Console.Clear();
                Console.WriteLine($"{name} uspješno je uklonjen iz proizvoda");
                Console.ReadKey();
                break;
            case 2:
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

                Console.WriteLine("Jeste li sigurni da želite ukloniti proizvode? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                var answerCase2 = Console.ReadLine();

                if (answerCase2.ToUpper() == "DA")
                {

                    foreach (var item in products)
                    {
                        if (item.Value.DateOfExpiry <= currentDate) products.Remove(item.Key);

                    }
                    Console.Clear() ;
                    Console.WriteLine("Proizvodi s isteklim datumom trajanja uspješno su uklonjeni");
                    Console.ReadKey();
                }
                else return;

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
                                Console.WriteLine("Jeste li sigurni da želite promijeniti količinu? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                                var answer = Console.ReadLine();

                                if (answer.ToUpper() == "DA")
                                {
                                    products[name] = (products[name].DateOfExpiry, newAmount, products[name].SoldAmount, products[name].Price);
                                }
                                else return;
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
                        Console.Clear();
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

                                Console.WriteLine("Jeste li sigurni da želite promijeniti količinu prodanih? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                                var answer = Console.ReadLine();

                                if (answer.ToUpper() == "DA")
                                {
                                    products[name] = (products[name].DateOfExpiry, products[name].AvailableAmount, newSoldAmount, products[name].Price);
                                }
                                else return;


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

                                Console.WriteLine("Jeste li sigurni da želite promijeniti cijenu? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                                var answer = Console.ReadLine();

                                if (answer.ToUpper() == "DA")
                                {
                                    products[name] = (products[name].DateOfExpiry, products[name].AvailableAmount, products[name].SoldAmount, newPrice);

                                }
                                else return;

                            
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

                    Console.WriteLine("Jeste li sigurni da želite promijeniti cijene? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                    var answer = Console.ReadLine();

                    if (answer.ToUpper() == "DA")
                    {
                        foreach (var item in products)
                        {
                            decimal newPrice = item.Value.Price * (1 + ((decimal)priceChange / 100));


                            products[item.Key] = (products[item.Key].DateOfExpiry, products[item.Key].AvailableAmount, products[item.Key].SoldAmount, newPrice);
                        }
                        Console.Clear();
                        Console.WriteLine($"Uspješno promijenjena cijena svih proizvoda za {priceChange}%");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Neispravan unos. Molimo unesite cijeli broj između 1 i 100.");
                        Console.ReadKey();
                    }
                }
                else return;
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
                Console.Clear();
                foreach (var item in products)
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");
                }
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                foreach (var item in products.OrderBy(x => x.Key))
                {
                    DateOnly expiryDate = item.Value.DateOfExpiry;
                    int daysUntilExpiry = (expiryDate.DayNumber - currentDate.DayNumber);
                    Console.WriteLine($"{item.Key} ({item.Value.AvailableAmount}) - {item.Value.Price} - {daysUntilExpiry}");
                }
                Console.ReadKey();
                break;
            case 3:
                Console.Clear();
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
                Console.Clear();
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
                Console.Clear();
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
                Console.Clear();
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
                Console.Clear();
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

static void WorkersMenu(Dictionary<string, DateOnly>? employees)
{
    var employeeMenuItems = new List<(int Id, string Name)>
    {
        (1, "Unos radnika"),
        (2, "Brisanje radnika"),
        (3, "Uredivanje radnika"),
        (4, "Ispis radnika"),
        (5, "Glavni izbornik")
    };

    switch (DisplayMenuAndPick(employeeMenuItems))
    {
        case 1:
            AddEmployeeAction(employees);
            break;
        case 2:
            DeleteEmployeeAction(employees);
            break;
        case 3:
            UpdateEmployeeAction(employees);
            break;
        case 4:
            PrintEmployeeAction(employees);
            break;
        case 5:
            return;

    }

    static void AddEmployeeAction(Dictionary<string, DateOnly>? employees)
    {
        Console.WriteLine("Unesite ime radnika (bez znakova č, ć, đ, ž):");
        var name = Console.ReadLine();

        Console.WriteLine("Unesite datum rođenja (yyyy-MM-dd):");
        if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly dateOfBirth))
        {
            Console.WriteLine("Neispravan format datuma. Molim vas koristite yyyy-MM-dd.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Jeste li sigurni da želite dodati zaposlenika? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
        var answer = Console.ReadLine();

        if (answer.ToUpper() == "DA")
        {
            employees.Add(name, dateOfBirth);
        }
        else return;

        Console.WriteLine($"{name} usješno je dodan u radnike");
        Console.ReadLine();
    }

    static void DeleteEmployeeAction(Dictionary<string, DateOnly>? employees)
    {
        var deleteEmployeeMenuItems = new List<(int Id, string Name)>
        {
            (1, "Po imenu"),
            (2, "Svi stariji od 65 godina")
        };

        switch (DisplayMenuAndPick(deleteEmployeeMenuItems))
        {
            case 1:
                Console.WriteLine("Unesite ime radnika kojeg želite obrisati");
                var name = Console.ReadLine();

                if(employees.ContainsKey(name))
                {

                    Console.WriteLine("Jeste li sigurni da želite ukloniti zaposlenika? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                    var answerDeleteEmployee = Console.ReadLine();

                    if (answerDeleteEmployee.ToUpper() == "DA")
                    {
                        employees.Remove(name);

                    }
                    else return;
                    Console.Clear();
                    Console.WriteLine("Uspješno ste uklonili radnika");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Ne postoji radnik sa tim imenom");
                    Console.ReadKey();
                    return;
                }
                break;
            case 2:
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                Console.WriteLine("Jeste li sigurni da želite ukloniti zaposlenika? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                var answer = Console.ReadLine();

                if (answer.ToUpper() == "DA")
                {
                    foreach (var item in employees)
                    {
                        DateOnly dateOfBirth = item.Value;
                        var age = currentDate.DayNumber - dateOfBirth.DayNumber;
                        if (age > 65)
                        {
                            employees.Remove(item.Key);
                        }
                        Console.WriteLine("Zaposlenici stariji od 65 su izbrisani");
                        Console.ReadKey();

                    }  }
                else return;

                break;

        }
    }

    static void UpdateEmployeeAction(Dictionary<string, DateOnly>? employees)
    {
        Console.WriteLine("Odaberite radnika kojem želite promijeniti datum rođenja:");
        var name = Console.ReadLine();


        if (employees.ContainsKey(name))
        {
            Console.WriteLine("Unesite novi datum rođenja (yyyy-MM-dd):");
            var newDateInput = Console.ReadLine();

            if (DateOnly.TryParse(newDateInput, out DateOnly newDateOfBirth))
            {

                Console.WriteLine("Jeste li sigurni da želite promijeniti zaposleniku datum rođenja? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                var answer = Console.ReadLine();

                if (answer.ToUpper() == "DA")
                {
                    employees[name] = newDateOfBirth;

                }
                else return;

                Console.WriteLine($"Datum rođenja za radnika {name} uspješno promijenjen.");
            }
            else
            {
                Console.WriteLine("Neispravan format datuma. Molim vas koristite yyyy-MM-dd.");
            }
        }
        else
        {
            Console.WriteLine("Ne postoji radnik s tim imenom");
            Console.ReadKey();
            return;
        }
    }

    static void PrintEmployeeAction(Dictionary<string, DateOnly>? employees)
    {
        var printEmployeeMenuItems = new List<(int Id, string Name)>
        {
            (1, "Svi radnici"),
            (2, "Svi radnici kojima je rođendan u tekućem mjesecu")
        };

        var currentDate = DateOnly.FromDateTime(DateTime.Now);


        switch (DisplayMenuAndPick(printEmployeeMenuItems))
        {
            case 1:
                Console.Clear();
                foreach (var item in employees)
                {
                    int age = currentDate.Year - item.Value.Year;
                    Console.WriteLine($"{item.Key} - {age}");
                }
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                foreach (var item in employees)
                {
                    if (item.Value.Month == currentDate.Month)
                    {
                        int age = currentDate.Year - item.Value.Year;
                        Console.WriteLine($"{item.Key} - {age} godina (rođendan u tekućem mjesecu)");
                    }
                }
                Console.ReadKey();
                break;
        }

    }
    
};

static void BillsMenu(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)>? products,
    Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)>? bills)
{
    var billsMenuItems = new List<(int Id, string Name)>
    {
        (1, "Unos novih računa"),
        (2, "Ispis računa"),
        (3, "Povratak u prethodni meni")
    };

    switch (DisplayMenuAndPick(billsMenuItems))
    {
        case 1:
            CreateBillsAction(products, bills);
            break;
        case 2:
            PrintBillsAction(products, bills);
            break;
        case 3:
            return;
    }

    static void CreateBillsAction(
    Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products,
    Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)> bills)
    {
        int billId = GetNextBillId(bills);

        var newBill = new List<(string ProductName, int Quantity, decimal Price)>();
        decimal totalBillPrice = 0;

        foreach (var product in products)
        {
            Console.WriteLine($"{product.Key} - Dostupna količina: {product.Value.AvailableAmount}, Cijena: {product.Value.Price}");
        }

        bool isConfirmed = false;

        while (true)
        {
            Console.WriteLine("Unesite ime proizvoda (ili 'K' za kraj):");
            string productName = Console.ReadLine();

            if (productName.ToUpper() == "K")
            {
                break;
            }

            if (products.ContainsKey(productName))
            {
                Console.WriteLine($"Unesite količinu za {productName}:");

                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity <= products[productName].AvailableAmount)
                {

                    Console.WriteLine("Jeste li sigurni da želite dodati proizvod? (Upišite 'da' za potvrdu ili bilo što drugo za poništavanje)");
                    var answer = Console.ReadLine();
                    if (answer.ToUpper() == "DA")
                    {
                        newBill.Add((productName, quantity, products[productName].Price));
                        totalBillPrice += quantity * products[productName].Price;

                        products[productName] = (products[productName].DateOfExpiry, products[productName].AvailableAmount - quantity, products[productName].SoldAmount + quantity, products[productName].Price);
                    }
                    else return;
                }
                else
                {
                    Console.WriteLine("Neispravan unos. Pokušajte ponovno.");
                    continue;
                }
            }
            else
            {
                Console.WriteLine($"Proizvod '{productName}' ne postoji. Pokušajte ponovno.");
            }
        }


        var dateTimeNow = DateTime.Now;
        var newBillEntry = (dateTimeNow, totalBillPrice, newBill);

        bills.Add(billId, newBillEntry);

        Console.WriteLine($"\nRačun #{billId} uspješno kreiran.\n");
        Console.WriteLine("Proizvodi na računu:");
        foreach (var item in newBill)
        {
            Console.WriteLine($"{item.ProductName} - Količina: {item.Quantity} - Cijena po komadu: {item.Price}");
        }

        Console.WriteLine($"Ukupna cijena: {totalBillPrice}");

        Console.Write("Unesite 'P' za potvrdu ili 'O' za otkazivanje računa: ");
        var confirmationKey = Console.ReadKey().KeyChar;

        if (confirmationKey == 'P' || confirmationKey == 'p')
        {
            Console.WriteLine("\nRačun je potvrđen. Proizvodi su skinuti sa stanja.\n");
            isConfirmed = true;
        }
        else
        {
            Console.WriteLine("\nRačun je otkazan. Proizvodi nisu skinuti sa stanja.\n");
        }

        Console.ReadKey();
        if (!isConfirmed)
        {
            bills.Remove(billId);
            foreach (var item in newBill)
            {
                products[item.ProductName] = (products[item.ProductName].DateOfExpiry, products[item.ProductName].AvailableAmount + item.Quantity, products[item.ProductName].SoldAmount - item.Quantity, products[item.ProductName].Price);
            }
        }
    }

    static void PrintBillsAction (Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products,
    Dictionary< int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items) > bills)
    {
        Console.WriteLine("Svi računi:");

        foreach (var bill in bills)
        {
            Console.WriteLine($"#{bill.Key} - {bill.Value.DateAndTime} - {CalculateTotalPrice(bill.Value.Items)}");
        }


        Console.WriteLine("Odaberite račun za detalje (unestite id računa):");
        if (int.TryParse(Console.ReadLine(), out int selectedBillId) && bills.ContainsKey(selectedBillId))
        {
            var selectedBill = bills[selectedBillId];

            Console.WriteLine($"{selectedBillId} - {selectedBill.DateAndTime} - Ukupni iznos: {selectedBill.TotalPrice}");
            Console.WriteLine("Proizvodi:");

            foreach (var item in selectedBill.Items)
            {
                Console.WriteLine($"{item.ProductName} - Količina: {item.Quantity} - Cijena po komadu: {item.Price}");
            }
        }
        else
        {
            Console.WriteLine("Neispravan unos ili račun ne postoji.");
        }

        Console.ReadKey();



    }

    static decimal CalculateTotalPrice(List<(string ProductName, int Quantity, decimal Price)> items)
    {
        decimal total = 0;

        foreach (var item in items)
        {
            total += item.Quantity * item.Price;
        }

        return total;
    }


    static int GetNextBillId(Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)> bills)
    {
        return bills.Count > 0 ? bills.Keys.Max() + 1 : 1;
    }
}

static void StatisticsMenu(string password, Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products, Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)>? bills)
{
    Console.WriteLine("Unesite šifru za pristup:");
    string passwordInput = Console.ReadLine();

    if(CheckPassword(passwordInput, password))
    {
    var productMenuItems = new List<(int Id, string Name)>
    {
        (1, "Ukupan broj artikala u trgovini"),
        (2, "Vrijednost artikala koji nisu još prodani"),
        (3, "Vrijednost artikala koji su prodani"),
        (4, "Stanje po mjesecima"),
        (5, "Glavni izbornik")
    };

    switch(DisplayMenuAndPick(productMenuItems))
        {
            case 1:
                int totalProducts = CalculateTotalProducts(products);
                Console.WriteLine($"Ukupan broj artikala: {totalProducts}");
                Console.ReadKey();
                break;
            case 2:
                decimal valueOfAvailableProducts = CalculateAvailableProductsWorth(products);
                Console.WriteLine($"Vrijednost artikala koji nisu još prodani: {valueOfAvailableProducts}");
                Console.ReadKey();
                break;
            case 3:
                decimal valueOfSoldProducts = CalculateSoldProductsWorth(products);
                Console.WriteLine($"Vrijednost artikala koji su prodani: {valueOfSoldProducts}");
                Console.ReadKey();
                break;
            case 4:
                string case4PasswordInput = Console.ReadLine();
                CalculateProfitLoss(products, bills);
                break;
            case 5:
                return;


        }


        static int CalculateTotalProducts(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products)
        {
            int totalProducts = 0;
            foreach (var product in products)
            {
                totalProducts += product.Value.AvailableAmount;
            }
            return totalProducts;
        }

        static decimal CalculateAvailableProductsWorth(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products)
        {
            decimal value = 0;
            foreach (var product in products)
            {
                value += product.Value.AvailableAmount * product.Value.Price;
            }
            return value;
        }

        static decimal CalculateSoldProductsWorth(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products)
        {
            decimal value = 0;
            foreach (var product in products)
            {
                value += product.Value.SoldAmount * product.Value.Price;
            }
            return value;
        }

        static void CalculateProfitLoss(Dictionary<string, (DateOnly DateOfExpiry, int AvailableAmount, int SoldAmount, decimal Price)> products, Dictionary<int, (DateTime DateAndTime, decimal TotalPrice, List<(string ProductName, int Quantity, decimal Price)> Items)>? bills)
        {
            Console.WriteLine("Unesite mjesec (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
            {
                Console.WriteLine("Neispravan unos za mjesec.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Unesite godinu: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Neispravan unos za godinu.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Unesite ukupne plaće: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal totalWages))
            {
                Console.WriteLine("Neispravan unos za ukupne plaće.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Unesite iznos najma: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal rent))
            {
                Console.WriteLine("Neispravan unos za iznos najma.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Unesite ostale troškove: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal otherExpenses))
            {
                Console.WriteLine("Neispravan unos za ostale troškove.");
                Console.ReadKey();
                return;
            }

            decimal profitsMonth = 0;
            decimal expenses = rent + otherExpenses + totalWages;
            DateOnly targetDateTime = new DateOnly(year, month, 1);


            foreach (var bill in bills)
            {
                int billKey = bill.Key;
                DateTime dateAndTime = bill.Value.DateAndTime;

                DateOnly billDateOnly = new DateOnly(dateAndTime.Year, dateAndTime.Month, 1);

                if (billDateOnly == targetDateTime)
                {
                    profitsMonth += bill.Value.TotalPrice;
                }
            }


            Console.WriteLine($"date: {targetDateTime} - profit: {profitsMonth - expenses}");
            Console.ReadKey();
        }

        } else
        {
            Console.WriteLine("Neispravna lozinka.");
            Console.ReadKey();
        }



    static bool CheckPassword(string enteredPassword, string actualPassword)
    {
        return enteredPassword == actualPassword;
    }
    
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
            BillsMenu(products, bills);
            break;
        case 4:
            StatisticsMenu(password, products, bills);
            break;
        case 5:
            return;
        default:
            throw new InvalidOperationException("This should never happen.");
    }
}