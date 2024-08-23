using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XMLLoader
{
    class Program
    {

        static void Main()
        {
            string xmlFilePath = @"C:\temp\purchases.xml";

            LoadOrdersFromXml(xmlFilePath);

        }

        static void LoadOrdersFromXml(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            XDocument xdoc = XDocument.Load(xmlFilePath);
            List<Order> orders = xdoc.Descendants("order")
                .Select(o => new Order
                {
                    No = int.Parse(o.Element("no").Value),
                    RegDate = DateTime.Parse(o.Element("reg_date").Value),
                    Sum = double.Parse(o.Element("sum").Value),
                    Products = o.Descendants("product")
                        .Select(p => new Product
                        {
                            Name = p.Element("name").Value,
                            Price = double.Parse(p.Element("price").Value)
                        }).ToList(),
                    User = new User
                    {
                        Name = o.Element("user").Element("fio").Value,
                        Email = o.Element("user").Element("email").Value
                    }
                }).ToList();

            using (var context = new AppDbContext())
            {
                foreach (var order in orders)
                {
                    var user = context.Users.FirstOrDefault(u => u.Email == order.User.Email);
                    if (user == null)
                    {
                        context.Users.Add(order.User);
                        context.SaveChanges();
                        user = order.User;
                    }

                    var purchase = new Purchase
                    {
                        UserId = user.UserId,
                        PurchaseDate = order.RegDate,
                        Sum = order.Sum
                    };
                    context.Purchases.Add(purchase);
                    context.SaveChanges();

                    foreach (var product in order.Products)
                    {
                        var existingProduct = context.Products.FirstOrDefault(p => p.Name == product.Name);
                        if (existingProduct == null)
                        {
                            context.Products.Add(product);
                            context.SaveChanges();
                            existingProduct = product;
                        }

                        var purchaseDetail = new PurchaseDetails
                        {
                            PurchaseId = purchase.PurchaseId,
                            ProductId = existingProduct.ProductId,
                            Quantity = order.Products.First(p => p.Name == product.Name).Quantity
                        };
                        context.PurchaseDetails.Add(purchaseDetail);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}









