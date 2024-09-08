using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public interface IUserRepo
    {
        public Task<List<User>> GetPendingReturnsUserDetails();
    }
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;
        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<User>> GetPendingReturnsUserDetails()
        {
            var users = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string tomorrowDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    // Updated query to check if GatepassType is 'Returnable' and Expected_DateofReturn is tomorrow
                    string query = @"SELECT ReceiverEmail, ReceiverName, Expected_DateofReturn,Gatepass_Number
                                    FROM GatePasses 
                                    WHERE Expected_DateofReturn = @tomorrowDate 
                                    AND GatepassType = 'Returnable'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tomorrowDate", tomorrowDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User()
                                {
                                    Email = reader["ReceiverEmail"]?.ToString(),
                                    UserName = reader["ReceiverName"]?.ToString(),
                                    GatepassNumber = reader["Gatepass_Number"]?.ToString()
                                });

                            }
                        }
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.Message);
            }

            return users;
        }

    }
}
