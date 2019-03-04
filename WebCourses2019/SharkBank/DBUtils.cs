using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using ResultOf;

namespace SharkBank
{
    public class DbUtils
    {
        private string connString;
        public DbUtils(string dataSource, string database, string username, string password)
        {
            connString = $"Data Source={dataSource};Initial Catalog={database};" +
                             $"Persist Security Info=True;User ID={username};Password={password}";
        }
        
        public SqlConnection GetDbConnection() => new SqlConnection(connString);
    }

    public class Db
    {
        private DbUtils utils;

        public Db(DbUtils utils)
        {
            this.utils = utils;
        }

        public Result<int> InsertRequest(string paymentFrom, string bic, string accountNumber, string vatInfo, int cost, string phoneNumber, string email, DateTime requestDate)
        {
            Result<int> result;
            using (var connection = utils.GetDbConnection())
            {
                connection.Open();
                var sql = $"Insert into RequestedPayments(paymentFrom, bic, accountNumber, vatInfo, cost, phoneNumber, email, requestDate)" +
                          $"values(@paymentFrom, @bic, @accountNumber, @vatInfo, @cost, @phoneNumber, @email, @requestDate)";
                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@paymentFrom", SqlDbType.NVarChar).Value = paymentFrom;
                cmd.Parameters.Add("@bic", SqlDbType.NVarChar).Value = bic;
                cmd.Parameters.Add("@accountNumber", SqlDbType.NVarChar).Value = accountNumber;
                cmd.Parameters.Add("@vatInfo", SqlDbType.NVarChar).Value = vatInfo;
                cmd.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                cmd.Parameters.Add("@phoneNumber", SqlDbType.NVarChar).Value = phoneNumber;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                cmd.Parameters.Add("@requestDate", SqlDbType.Date).Value = requestDate;

                result = Result.Of(() => cmd.ExecuteNonQuery());
            }

            return result;
        }

        public Result<bool> CheckUser(string username, string password)
        {
            using (var connection = utils.GetDbConnection())
            {
                connection.Open();
                var sql = "Select * from Users where username=@username and password=@password";
                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    return Result.Of(() => reader.HasRows);
                }
            }
        }

        public string GetAllRequests()
        {
            var res = new List<string>();
            var sql = "Select * from RequestedPayments";
            using (var connection = utils.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            res.Add(JsonConvert.SerializeObject(new
                            {
                                id = reader.GetValue(0),
                                from = reader.GetValue(1),
                                bic = reader.GetValue(2),
                                accountNumber = reader.GetValue(3),
                                vatInf0 = reader.GetValue(4),
                                cost = reader.GetValue(5),
                                phoneNumber = reader.GetValue(6),
                                email = reader.GetValue(7),
                                requestDate = reader.GetValue(8)

                            }));
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(res);     
        }

        public Result<int> InsertPayments(string cardNumber, DateTime expirationDate, int cost, DateTime paymentDate)
        {
            Result<int> result;
            using (var connection = utils.GetDbConnection())
            {
                connection.Open();
                var sql = $"Insert into Payments(cardNumber, expirationDate, cost, paymentData)" +
                          $"values(@cardNum, @expDate, @cost, @paymentData)";
                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@cardNum", SqlDbType.NVarChar).Value = cardNumber;
                cmd.Parameters.Add("@expDate", SqlDbType.Date).Value = expirationDate;
                cmd.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                cmd.Parameters.Add("@paymentData", SqlDbType.Date).Value = paymentDate;

                result = Result.Of(() => cmd.ExecuteNonQuery());
            }

            return result;
        }
    }
}