using System;
using System.Xml.Linq;
using System.Data.Entity;


namespace XMLLoader
{
    class Program
    {

        static void Main()
        {
            string xmlFilePath = @"C:\temp\purchases.xml"; // Путь к вашему XML файлу

            try
            {
                // Загрузка XML файла
                XDocument xdoc = XDocument.Load(xmlFilePath);

                // Парсинг XML в объекты классов
                List<Order> orders = xdoc.Descendants("order")
                    .Select(o => new Order
                    {
                        No = int.Parse(o.Element("no").Value),
                        RegDate = DateTime.Parse(o.Element("reg_date").Value),
                        Sum = double.Parse(o.Element("sum").Value),
                        Products = o.Descendants("product")
                            .Select(p => new Product
                            {
                                Quantity = int.Parse(p.Element("quantity").Value),
                                Name = p.Element("name").Value,
                                Price = double.Parse(p.Element("price").Value)
                            }).ToList(),
                        User = new User
                        {
                            Name = o.Element("user").Element("fio").Value,
                            Email = o.Element("user").Element("email").Value
                        }
                    }).ToList();

                // Вывод информации в консоль
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order No: {order.No}");
                    Console.WriteLine($"Registration Date: {order.RegDate}");
                    Console.WriteLine($"Sum: {order.Sum}");
                    Console.WriteLine("Products:");
                    foreach (var product in order.Products)
                    {
                        Console.WriteLine($"  Name: {product.Name}, Quantity: {product.Quantity}, Price: {product.Price}");
                    }
                    Console.WriteLine($"User: {order.User.Name}, Email: {order.User.Email}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении XML файла: {ex.Message}");
            }
        }
    }

}







