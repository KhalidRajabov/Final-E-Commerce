using Final_E_Commerce.Entities;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Final_E_Commerce.Areas.Admin.Repositories
{
    public class MessageRepository
    {
        string? connectionString;
        public MessageRepository(string? connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Messages> GetMessages()
        {
            List<Messages> messages = new List<Messages>();
            var data = GetMessageFromDb();

            foreach (DataRow row in data.Rows)
            {
                Messages message = new Messages
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Subject = row["Subject"].ToString(),
                    Date = Convert.ToDateTime(row["Date"]),
                };
                messages.Add(message);
            }
            return messages;
        }


        public DataTable GetMessageFromDb()
        {
            var query = "SELECT Id, Subject, Date FROM Messages WHERE IsAnswered = 'False'";
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