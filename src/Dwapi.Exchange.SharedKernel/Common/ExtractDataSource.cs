using System;

namespace Dwapi.Exchange.SharedKernel.Common
{
    public class ExtractDataSource
    {
        public DatabaseType DatabaseType { get; set; }
        public string Connection { get; set; }

        
        public ExtractDataSource(DatabaseType databaseType, string connection)
        {
            DatabaseType = databaseType;
            Connection = connection;
        }

        public static ExtractDataSource Load(string databaseType, string connection)
        {
            if (int.TryParse(databaseType, out var dbType))
                return new ExtractDataSource((DatabaseType) dbType, connection);

            throw new Exception("invalid connection!");
        }
    }
}
