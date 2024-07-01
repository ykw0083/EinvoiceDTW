using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EinvoiceDTW
{
    public interface IDatabaseContext
    {
        IDbConnection Connection { get; }
    }

    public class DatabaseContext : IDatabaseContext
    {
        private readonly IConfiguration _config;

        public DatabaseContext(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
