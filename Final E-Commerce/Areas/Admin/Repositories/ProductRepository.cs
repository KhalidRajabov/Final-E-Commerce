using Final_E_Commerce.Entities;
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

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            

            var data = GetProductDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                Product product = new Product
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
            var query = "SELECT Id, Name, Price FROM Product";
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
                catch (Exception ex)
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