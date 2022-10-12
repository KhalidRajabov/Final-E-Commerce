﻿using Final_E_Commerce.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Final_E_Commerce.Areas.Admin.Repositories
{
    public class ProductRepository
    {
        string? connectionString;

        public ProductRepository(string? connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Products> GetProducts()
        {
            List<Products> products = new List<Products>();
            

            var data = GetProductDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                Products product = new Products
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDouble(row["Price"])
                };
                products.Add(product);
            }
            return products;
        }

        public DataTable GetProductDetailsFromDb()
        {
            var query = "SELECT Id, Name, Price FROM Products";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    return dataTable;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}