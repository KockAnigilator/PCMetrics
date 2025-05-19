using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PcBack.Data.DbContext
{
    public static class Database
    {
        private static string connectionString =
        "Host=localhost;Username=postgres;Password=ваш_пароль;Database=PcMonitoringDb";

        public static NpgsqlConnection GetConnetction()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
