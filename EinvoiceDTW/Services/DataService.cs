using Dapper;
using EinvoiceDTW.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EinvoiceDTW.Services
{
    public interface IDataService
    {
        Task<IEnumerable<QueryResult>> PushJsonToTable(string json, string type, string myfilename);
        Task<IEnumerable<LHDN_Invoice>> GetInvoicesAsync();
    }

    public class DataService : IDataService
    {
        private readonly IDatabaseContext _db;

        public DataService(IDatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<QueryResult>> PushJsonToTable(string json, string type, string myfilename)
        {
            using (var conn = _db.Connection)
            {
                conn.Open();
                var result = await conn.QueryAsync<QueryResult>($"exec PushJsonToTable N'{json}', '{type}', '{myfilename}'");
                return result;
            }
        }
        public async Task<IEnumerable<LHDN_Invoice>> GetInvoicesAsync()
        {
            using (var conn = _db.Connection)
            {
                conn.Open();
                var result = await conn.QueryAsync<LHDN_Invoice>($"exec GetInvoiceList");
                return result;
            }
        }
    }
}
