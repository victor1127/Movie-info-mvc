using Microsoft.Extensions.Configuration;
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
            var externalHost= Environment.GetEnvironmentVariable("")
        }
    }
}
