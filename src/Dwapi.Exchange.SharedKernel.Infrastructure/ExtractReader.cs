using System;
using System.Collections.Generic;
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
        private readonly DataSource _dataSource;

        public ExtractReader(DataSource dataSource)
        {
            _dataSource = dataSource;
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
            if (_dataSource.ConnectionType==ConnectionType.MsSql)
                return new SqlConnection(_dataSource.ConnectionString);

            return new SqliteConnection(_dataSource.ConnectionString);
        }
    }
}
