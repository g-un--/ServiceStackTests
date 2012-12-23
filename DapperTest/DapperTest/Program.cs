using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryDatabase().ContinueWith(task =>
                {
                    foreach (var item in task.Result)
                    {
                        Console.WriteLine(item.Value);
                    }
                });

            Console.ReadLine();
        }

        static async Task<IEnumerable<dynamic>> QueryDatabase()
        {
            using (var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=password;"))
            {
                await connection.OpenAsync();
                var data = await connection.QueryAsync<dynamic>("select * from dbo.DataSet");
                return data;
            }
        }
    }
}
