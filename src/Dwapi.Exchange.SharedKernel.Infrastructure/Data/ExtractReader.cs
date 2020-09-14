using System;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Infrastructure
{
    public class ExtractReader:IExtractReader
    {
        private readonly ExtractDataSource _extractDataSource;

        public ExtractReader(ExtractDataSource extractDataSource)
        {
            _extractDataSource = extractDataSource;
        }

        public PagedExtract Read(IExtractDefinition definition, int pageNumber, int pageSize)
        {
            var pageCount=(int)Math.Ceiling((double)12/(double)5);
            try
            {
                using (var cn=GetConnection())
                {
                    cn.Open();
                    var results = cn.Query(definition.Sql).ToList();
                    return new PagedExtract(pageNumber, pageSize, pageCount, results);
                }
            }
            catch (Exception e)
            {
               Log.Error("Error reading",e);
                throw;
            }
        }
        private IDbConnection GetConnection()
        {
            if (_extractDataSource.DatabaseType==DatabaseType.MsSql)
                return new SqlConnection(_extractDataSource.Connection);

            return new SqliteConnection(_extractDataSource.Connection);
        }
    }
}
