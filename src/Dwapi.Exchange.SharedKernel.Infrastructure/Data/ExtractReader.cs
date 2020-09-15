using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Custom;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Dwapi.Exchange.SharedKernel.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Data
{
    public class ExtractReader:IExtractReader
    {
        private readonly ExtractDataSource _extractDataSource;

        public ExtractReader(ExtractDataSource extractDataSource)
        {
            _extractDataSource = extractDataSource ?? throw new Exception("Datasource not initialized !");
        }

        public async Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript} ORDER BY LiveRowId ";

            var sqlPaging = @"
                 OFFSET @Offset ROWS 
                 FETCH NEXT @PageSize ROWS ONLY
            ";

            if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
            {
                sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
            }

            sql = $"{sql}{sqlPaging}";


            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();
                    var results = await cn.QueryAsync(sql, new
                    {
                        Offset = (pageNumber - 1) * pageSize,
                        PageSize = pageSize
                    });
                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<long> GetCount(ExtractDefinition definition)
        {
            try
            {
                var sql = definition.GenerateCountScript();
                using (var cn = GetConnection())
                {
                    cn.Open();
                    var results = await cn.QueryAsync<long>(sql);

                    var counts = results.ToList();
                    if (counts.Any())
                        return counts.First();
                    return 0;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract count", e);
                throw;
            }
        }

        public Task Initialize(ExtractDefinition definition)
        {
            throw new NotImplementedException();
        }


        private IDbConnection GetConnection()
        {
            if (_extractDataSource.DatabaseType==DatabaseType.MsSql)
                return new SqlConnection(_extractDataSource.Connection);

            return new SqliteConnection(_extractDataSource.Connection);
        }
    }
}
