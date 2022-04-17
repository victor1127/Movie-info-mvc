using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var localHost = configuration.GetConnectionString("DefaultConnection");
            var externalHost = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(externalHost) ? localHost : BuildConnectionString(externalHost);
        }

        private static string BuildConnectionString(string connection)
        {
            var connectionUri = new Uri(connection);
            var userInfo = connectionUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = connectionUri.Host,
                Port = connectionUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = connectionUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
