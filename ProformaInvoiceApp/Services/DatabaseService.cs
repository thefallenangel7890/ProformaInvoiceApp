using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ProformaInvoiceApp.Models;

namespace ProformaInvoiceApp.Services
{
    public class DatabaseService
    {
        private string dbPath;
        private string connectionString;

        public DatabaseService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "ProformaInvoiceApp");
            Directory.CreateDirectory(appFolder);
            dbPath = Path.Combine(appFolder, "invoicedata.db");
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                
                // Products table
                var productsTable = @"CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductCode TEXT NOT NULL,
                    VariationCode TEXT NOT NULL UNIQUE,
                    ProductName TEXT NOT NULL,
                    PackSize TEXT NOT NULL,
                    UnitPrice DECIMAL(10,2) NOT NULL,
                    Description TEXT,
                    IsBaseProduct BOOLEAN DEFAULT 1,
                    BaseProductId INTEGER DEFAULT 0,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                )";
                
                // Invoices table
                var invoicesTable = @"CREATE TABLE IF NOT EXISTS Invoices (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceNumber TEXT NOT NULL UNIQUE,
                    InvoiceDate DATETIME NOT NULL,
                    Buyer TEXT NOT NULL,
                    ProductCode TEXT NOT NULL,
                    ProductName TEXT NOT NULL,
                    PackSize TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Rate DECIMAL(10,2) NOT NULL,
                    TotalAmount DECIMAL(10,2) NOT NULL,
                    BaseProductCode TEXT NOT NULL,
                    OriginalPrice DECIMAL(10,2) NOT NULL,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                )";
                
                using (var cmd = new SQLiteCommand(productsTable, conn))
                    cmd.ExecuteNonQuery();
                
                using (var cmd = new SQLiteCommand(invoicesTable, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        public void AddProduct(Product product)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "INSERT INTO Products (ProductCode, VariationCode, ProductName, PackSize, UnitPrice, Description, IsBaseProduct, BaseProductId) VALUES (@code, @varcode, @name, @pack, @price, @desc, @isbase, @baseid)", 
                    conn);
                cmd.Parameters.AddWithValue("@code", product.ProductCode);
                cmd.Parameters.AddWithValue("@varcode", product.VariationCode);
                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@pack", product.PackSize);
                cmd.Parameters.AddWithValue("@price", product.UnitPrice);
                cmd.Parameters.AddWithValue("@desc", product.Description);
                cmd.Parameters.AddWithValue("@isbase", product.IsBaseProduct);
                cmd.Parameters.AddWithValue("@baseid", product.BaseProductId);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Product> SearchProducts(string keyword)
        {
            var products = new List<Product>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "SELECT * FROM Products WHERE ProductName LIKE @keyword AND IsBaseProduct = 1", 
                    conn);
                cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32("Id"),
                            ProductCode = reader.GetString("ProductCode"),
                            VariationCode = reader.GetString("VariationCode"),
                            ProductName = reader.GetString("ProductName"),
                            PackSize = reader.GetString("PackSize"),
                            UnitPrice = reader.GetDecimal("UnitPrice")
                        });
                    }
                }
            }
            return products;
        }

        public void AddInvoice(Invoice invoice)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "INSERT INTO Invoices (InvoiceNumber, InvoiceDate, Buyer, ProductCode, ProductName, PackSize, Quantity, Rate, TotalAmount, BaseProductCode, OriginalPrice) VALUES (@num, @date, @buyer, @pcode, @pname, @pack, @qty, @rate, @total, @basecode, @origprice)", 
                    conn);
                cmd.Parameters.AddWithValue("@num", invoice.InvoiceNumber);
                cmd.Parameters.AddWithValue("@date", invoice.InvoiceDate);
                cmd.Parameters.AddWithValue("@buyer", invoice.Buyer);
                cmd.Parameters.AddWithValue("@pcode", invoice.ProductCode);
                cmd.Parameters.AddWithValue("@pname", invoice.ProductName);
                cmd.Parameters.AddWithValue("@pack", invoice.PackSize);
                cmd.Parameters.AddWithValue("@qty", invoice.Quantity);
                cmd.Parameters.AddWithValue("@rate", invoice.Rate);
                cmd.Parameters.AddWithValue("@total", invoice.TotalAmount);
                cmd.Parameters.AddWithValue("@basecode", invoice.BaseProductCode);
                cmd.Parameters.AddWithValue("@origprice", invoice.OriginalPrice);
                cmd.ExecuteNonQuery();
            }
        }

        public List<string> GetAllBuyers()
        {
            var buyers = new List<string>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT DISTINCT Buyer FROM Invoices", conn);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buyers.Add(reader.GetString("Buyer"));
                    }
                }
            }
            return buyers;
        }
    }
}