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
        "Host=localhost;Username=postgres;Password=1545;Database=PcMonitoringDb";

        public static NpgsqlConnection GetConnetction()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
