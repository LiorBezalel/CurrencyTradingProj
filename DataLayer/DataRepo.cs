using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;



namespace DataLayer
{
    public class DataRepo
    {
        private readonly string _connectionString;

        public DataRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<CurrencyPairModel>> GetCurrencyPairs()
        {
            List<CurrencyPairModel> result = new List<CurrencyPairModel>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"SELECT cp.Id, 
                               fc.Abbreviation AS FromCurrency,
                               tc.Abbreviation AS ToCurrency,
                               cp.MinValue,
                               cp.MaxValue
                               FROM CurrencyPair cp
                               JOIN Currency fc ON cp.FromCurrencyId = fc.Id
                               JOIN Currency tc ON cp.ToCurrencyId = tc.Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CurrencyPairModel
                        {
                            Id = reader.GetInt32(0),
                            FromCurrency = reader.GetString(1),
                            ToCurrency = reader.GetString(2),
                            MinValue = reader.GetDecimal(3),
                            MaxValue = reader.GetDecimal(4),
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public async Task<bool> UpdateCurrencyPair(CurrencyPairModel pair)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                string query = @"UPDATE CurrencyPair 
                                 SET MinValue = @MinValue, 
                                     MaxValue = @MaxValue 
                                 WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", pair.Id);
                    cmd.Parameters.AddWithValue("@MinValue", pair.MinValue);
                    cmd.Parameters.AddWithValue("@MaxValue", pair.MaxValue);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateCurrencyCurrentVal(CurrencyModel currency, decimal newVal)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                string Query = @"UPDATE Currency
                                Set CurrentValue = @newVal
                                WHERE Id = @Id;";
                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", currency.Id);
                    cmd.Parameters.AddWithValue("@newVal", newVal);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<CurrencyModel>> GetCurrencies()
        {
            List<CurrencyModel> result = new List<CurrencyModel>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                String query = @"SELECT Id, Country, CurrencyName, Abbreviation, CurrentValue FROM Currency";
                using SqlCommand cmd = new SqlCommand(query, conn);
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    { 
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CurrencyModel
                        {
                            Id = reader.GetInt32(0),
                            Country = reader.GetString(1),
                            CurrencyName = reader.GetString(2),
                            Abbreviation = reader.GetString(3),
                            CurrentValue = reader.GetDecimal(4)
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<CurrencyModel> GetCurrencyByAbbreviation(String abbreviation)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                String query = @"SELECT Id, Country, CurrencyName, Abbrevation, CurrentValue
                             FROM Currency
                             WHERE Abbrevation = @abbrevation";
                using SqlCommand cmd = new SqlCommand(query, conn);
                {
                    cmd.Parameters.AddWithValue("@abbrevation", abbreviation);
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        CurrencyModel result = new CurrencyModel
                        {
                            Id = reader.GetInt32(0),
                            Country = reader.GetString(1),
                            CurrencyName = reader.GetString(2),
                            Abbreviation = reader.GetString(3),
                            CurrentValue = reader.GetDecimal(4)
                        };
                        return result;
                    }
                    else return null;
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("Failde to get currency " + abbreviation +" from DB");
                Console.WriteLine(e.Message);
                return null;
            }

        }
    }
}
