using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Exchange.Contracts;
using Dwapi.Exchange.SharedKernel.Common;
using Dwapi.Exchange.SharedKernel.Custom;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Dwapi.Exchange.SharedKernel.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Serilog;
using Serilog.Debugging;

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

        public async Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null)
        {
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;
            bool reCount = false;
            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript}";

            var sqlPaging = @"
                 OFFSET @Offset ROWS 
                 FETCH NEXT @PageSize ROWS ONLY
            ";

            if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
            {
                sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
            }

            if (null!=siteCode && siteCode.Any())
            {
                sql = $"{sql} WHERE FacilityCode IN @siteCode"; reCount = true;
                var count =await GetCountFrom(definition, sql,siteCode);  
                pageCount = Utils.PageCount(pageSize, count);
            }

            sql = $"{sql} ORDER BY LiveRowId {sqlPaging}";


            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();

                    IEnumerable<dynamic> results;
                    if (null!=siteCode && siteCode.Any())
                    {
                        results = await cn.QueryAsync(sql, new
                        {
                            Offset = (pageNumber - 1) * pageSize,
                            PageSize = pageSize,
                            siteCode=siteCode
                        });
                    }
                    else
                    {
                        results = await cn.QueryAsync(sql, new
                        {
                            Offset = (pageNumber - 1) * pageSize,
                            PageSize = pageSize
                        });
                    }

                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedExtract> Read(ExtractDefinition definition, int pageNumber, int pageSize, DateTime? evaluationDate,
            int[] siteCode = null, string cccNumber = "", string recencyId = "", string indicatorName = null, int[] period = null)
        {
            
           var whereList=new List<string>();
            dynamic whereVals = new ExpandoObject();
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript}";

            whereVals.Offset = (pageNumber - 1) * pageSize;
            whereVals.PageSize = pageSize;

            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();


                    if (null != siteCode && siteCode.Any())
                    {
                        whereList.Add($"FacilityCode IN @siteCode");
                        whereVals.siteCode = siteCode;
                    }

                    if (!string.IsNullOrWhiteSpace(cccNumber))
                    {
                        whereList.Add($"PatientCccNumber = @cccNumber");
                        whereVals.cccNumber = cccNumber;
                    }

                    if (evaluationDate.HasValue && evaluationDate>new DateTime(1901,1,1))
                    {
                        whereList.Add($"evaluationDate > @evaluationDate");
                        whereVals.evaluationDate = evaluationDate.Value.Date;
                    }

                    if (!string.IsNullOrWhiteSpace(recencyId))
                    {
                        whereList.Add($"recencyId = @recencyId");
                        whereVals.recencyId = recencyId;
                    }

                    if (null != indicatorName && indicatorName.Any())
                    {
                        whereList.Add($"indicator_name = @indicatorName");
                        whereVals.indicatorName = indicatorName;
                    }

                    if (null != period && period.Any())
                    {
                        whereList.Add($"period IN @period");
                        whereVals.period = period;
                    }

                    if (whereList.Any())
                        sql = $"{sql} WHERE {string.Join(" AND ",whereList)} ";

                    sql = $"{sql} ORDER BY LiveRowId ";

                    var sqlPaging = @"
                         OFFSET @Offset ROWS 
                         FETCH NEXT @PageSize ROWS ONLY
                    ";


                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
                    }

                    sql = $"{sql}{sqlPaging}";

                    IEnumerable<dynamic> results = new List<dynamic>();
                    results = await cn.QueryAsync(sql, (object) whereVals);

                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedExtract> ReadProc(ExtractDefinition definition, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlProc}";

            if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                return await Read(definition, pageNumber, pageSize);

            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();
                    var results = await cn.QueryAsync(
                        sql,
                        new {PageNumber = pageNumber, PageSize = pageSize},
                        commandType: CommandType.StoredProcedure);
                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedExtract> ReadProfile(ExtractDefinition definition, int pageNumber, int pageSize,int[] siteCode = null,
            string[] county = null)
        {
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript}";


            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();


                    if (null!=siteCode && siteCode.Any() && null!=county &&  county.Any())
                    {
                        sql = $"{sql} WHERE FacilityCode IN @siteCode OR County IN @county";
                    }
                    else
                    {

                        if (null!=siteCode && siteCode.Any())
                        {
                            sql = $"{sql} WHERE FacilityCode IN @siteCode";
                        }

                        if (null != county && county.Any())
                        {
                            sql = $"{sql} WHERE County IN @county";
                        }
                    }

                    sql = $"{sql} ORDER BY LiveRowId ";
                    var sqlPaging = @"
                 OFFSET @Offset ROWS 
                 FETCH NEXT @PageSize ROWS ONLY
            ";

                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
                    }

                    sql = $"{sql}{sqlPaging}";


                    IEnumerable<dynamic> results = new List<dynamic>();

                    if (null!=siteCode && siteCode.Any() && null!=county &&  county.Any())
                    {
                         results = await cn.QueryAsync(sql, new
                        {
                            Offset = (pageNumber - 1) * pageSize,
                            PageSize = pageSize,
                            siteCode=siteCode,
                            county=county

                        });
                    }
                    else
                    {

                        if (null!=siteCode && siteCode.Any())
                        {
                           results = await cn.QueryAsync(sql, new
                            {
                                Offset = (pageNumber - 1) * pageSize,
                                PageSize = pageSize,
                                siteCode=siteCode
                            });
                        }

                        if (null != county && county.Any())
                        {
                           results = await cn.QueryAsync(sql, new
                            {
                                Offset = (pageNumber - 1) * pageSize,
                                PageSize = pageSize,
                                county=county
                            });
                        }
                    }



                    foreach (var result in results)
                    {
                        result.Visits = new List<dynamic>();
                        result.AdverseEvent = new List<dynamic>();
                        result.PatientStatus = new List<dynamic>();
                        result.Baselines = new List<dynamic>();
                        result.Pharmacy = new List<dynamic>();
                        result.Labs = new List<dynamic>();
                        result.ART = new List<dynamic>();

                        #region Visits
                        var Visits = await cn.QueryAsync("SELECT * FROM Visits WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Visits.Any())
                            result.Visits = Visits;
                        #endregion

                        #region AdverseEvent
                        var AdverseEvent = await cn.QueryAsync("SELECT * FROM AdverseEvent WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (AdverseEvent.Any())
                            result.AdverseEvent = AdverseEvent;
                        #endregion

                        #region PatientStatus
                        var PatientStatus = await cn.QueryAsync("SELECT * FROM PatientStatus WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (PatientStatus.Any())
                            result.PatientStatus = PatientStatus;
                        #endregion

                        #region Baselines
                        var Baselines = await cn.QueryAsync("SELECT * FROM Baselines WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Baselines.Any())
                            result.Baselines = Baselines;
                        #endregion

                        #region Pharmacy

                        var Pharmacy = await cn.QueryAsync("SELECT * FROM Pharmacy WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Pharmacy.Any())
                            result.Pharmacy = Pharmacy;

                        #endregion

                        #region Labs
                        var Labs = await cn.QueryAsync("SELECT * FROM Labs WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Labs.Any())
                            result.Labs = Labs;
                        #endregion

                        #region ART
                        var ART = await cn.QueryAsync("SELECT * FROM ART WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (ART.Any())
                            result.ART = ART;
                        #endregion
                    }
                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedExtract> ReadProfileFilter(ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null,
            string[] county = null, string gender = "", int age = -1)
        {
            var whereList=new List<string>();
            dynamic whereVals = new ExpandoObject();
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript}";

            whereVals.Offset = (pageNumber - 1) * pageSize;
            whereVals.PageSize = pageSize;

            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();


                    if (null != siteCode && siteCode.Any())
                    {
                        whereList.Add($"FacilityCode IN @siteCode");
                        whereVals.siteCode = siteCode;
                    }

                    if (null != county && county.Any())
                    {
                        whereList.Add($"County IN @county");
                        whereVals.county = county;
                    }

                    if (!string.IsNullOrWhiteSpace(gender))
                    {
                        if (gender.Trim().ToLower() == "f")
                            gender = "Female";
                        if (gender.Trim().ToLower() == "m")
                            gender = "Male";
                        whereList.Add($"Gender Like @gender");
                        whereVals.gender = gender;
                    }

                    if (age > 0)
                    {
                        whereList.Add($"Age=@age");
                        whereVals.age = age;
                    }

                    if(whereList.Any())
                        sql = $"{sql} WHERE {string.Join(" AND ",whereList)} ";

                    sql = $"{sql} ORDER BY LiveRowId ";

                    var sqlPaging = @"
                         OFFSET @Offset ROWS 
                         FETCH NEXT @PageSize ROWS ONLY
                    ";


                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
                    }

                    sql = $"{sql}{sqlPaging}";


                    IEnumerable<dynamic> results = new List<dynamic>();
                    results = await cn.QueryAsync(sql, (object) whereVals);

                    foreach (var result in results)
                    {
                        result.Visits = new List<dynamic>();
                        result.AdverseEvent = new List<dynamic>();
                        result.PatientStatus = new List<dynamic>();
                        result.Baselines = new List<dynamic>();
                        result.Pharmacy = new List<dynamic>();
                        result.Labs = new List<dynamic>();
                        result.ART = new List<dynamic>();

                        #region Visits
                        var Visits = await cn.QueryAsync("SELECT * FROM Visits WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Visits.Any())
                            result.Visits = Visits;
                        #endregion


                        #region AdverseEvent
                        var AdverseEvent = await cn.QueryAsync("SELECT * FROM AdverseEvent WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (AdverseEvent.Any())
                            result.AdverseEvent = AdverseEvent;
                        #endregion

                        #region PatientStatus
                        var PatientStatus = await cn.QueryAsync("SELECT * FROM PatientStatus WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (PatientStatus.Any())
                            result.PatientStatus = PatientStatus;
                        #endregion

                        #region Baselines
                        var Baselines = await cn.QueryAsync("SELECT * FROM Baselines WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Baselines.Any())
                            result.Baselines = Baselines;
                        #endregion

                        #region Pharmacy

                        var Pharmacy = await cn.QueryAsync("SELECT * FROM Pharmacy WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Pharmacy.Any())
                            result.Pharmacy = Pharmacy;

                        #endregion

                        #region Labs
                        var Labs = await cn.QueryAsync("SELECT * FROM Labs WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (Labs.Any())
                            result.Labs = Labs;
                        #endregion

                        #region ART
                        var ART = await cn.QueryAsync("SELECT * FROM ART WHERE PatientID=@PatientID", new
                        {

                            PatientID = result.PatientID
                        });

                        if (ART.Any())
                            result.ART = ART;
                        #endregion

                    }
                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());
                }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedExtract> ReadDataFilter( ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null, string indicatorName = null, int[] period = null)
        {
            var whereList = new List<string>();
            dynamic whereVals = new ExpandoObject();
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{definition.SqlScript}";
            var extractSql = $"{definition.SqlScript}";

            whereVals.Offset = (pageNumber - 1) * pageSize;
            whereVals.PageSize = pageSize;

            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();


                    if (null != siteCode && siteCode.Any())
                    {
                        whereList.Add($"FacilityCode IN @SiteCode");
                        whereVals.siteCode = siteCode;
                    }

                    if (null != indicatorName && indicatorName.Any())
                    {
                        whereList.Add($"indicator_name = @indicatorName");
                        whereVals.indicatorName = indicatorName;
                    }

                    if (null != period && period.Any())
                    {
                        whereList.Add($"period IN @period");
                        whereVals.period = period;
                    }

                    if (whereList.Any())
                        sql = $"{sql} WHERE {string.Join(" AND ", whereList)} ";

                    sql = $"{sql} ORDER BY FacilityCode ";

                    var sqlPaging = @"
                         OFFSET @Offset ROWS 
                         FETCH NEXT @PageSize ROWS ONLY
                    ";


                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
                    }

                    sql = $"{sql}{sqlPaging}";

                    IEnumerable<dynamic> results = new List<dynamic>();
                    results = await cn.QueryAsync(sql, (object)whereVals);

                    return new PagedExtract(pageNumber, pageSize, pageCount, results.ToList());

                   }
            }
            catch (Exception e)
            {
                Log.Error("Error reading extract", e);
                throw;
            }
        }

        public async Task<PagedProfileExtract> ReadProfileFilterExpress(ExtractDefinition mainDefinition,ExtractDefinition definition, int pageNumber, int pageSize, int[] siteCode = null,
            string[] county = null, string gender = "", int age = -1)
        {
             var whereList=new List<string>();
            dynamic whereVals = new ExpandoObject();
            pageNumber = pageNumber < 0 ? 1 : pageNumber;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var pageCount = Utils.PageCount(pageSize, definition.RecordCount);

            var sql = $"{mainDefinition.SqlScript.Replace("*", nameof(Patients.PatientID))}";
            var extractSql = $"{definition.SqlScript}";

            whereVals.Offset = (pageNumber - 1) * pageSize;
            whereVals.PageSize = pageSize;

            try
            {
                using (var cn = GetConnection())
                {
                    cn.Open();


                    if (null != siteCode && siteCode.Any())
                    {
                        whereList.Add($"FacilityCode IN @siteCode");
                        whereVals.siteCode = siteCode;
                    }

                    if (null != county && county.Any())
                    {
                        whereList.Add($"County IN @county");
                        whereVals.county = county;
                    }

                    if (!string.IsNullOrWhiteSpace(gender))
                    {
                        if (gender.Trim().ToLower() == "f")
                            gender = "Female";
                        if (gender.Trim().ToLower() == "m")
                            gender = "Male";
                        whereList.Add($"Gender Like @gender");
                        whereVals.gender = gender;
                    }

                    if (age > 0)
                    {
                        whereList.Add($"Age=@age");
                        whereVals.age = age;
                    }

                    if(whereList.Any())
                        sql = $"{sql} WHERE {string.Join(" AND ",whereList)} ";

                    sql = $"{sql} ORDER BY PatientID ";

                    var sqlPaging = @"
                         OFFSET @Offset ROWS 
                         FETCH NEXT @PageSize ROWS ONLY
                    ";


                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
                    }

                    sql = $"{sql}{sqlPaging}";

                    IEnumerable<dynamic> results = new List<dynamic>();

                    if (_extractDataSource.DatabaseType == DatabaseType.SqLite)
                    {
                        var mainIdResult = await cn.QueryAsync<string>(sql, (object) whereVals);
                        var patientIds = mainIdResult.ToList();
                        results = await cn.QueryAsync(extractSql, new {patientIds});
                    }
                    else
                    {
                        var mainIdResult = await cn.QueryAsync<Guid>(sql, (object) whereVals);
                        var patientIds = mainIdResult.ToList();
                        results = await cn.QueryAsync(extractSql, new {patientIds});
                    }

                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Patients), new List<string> { nameof(Patients.PatientID) });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(AdverseEvent), new List<string> { nameof(AdverseEvent.LiveRowId) });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(ART), new List<string> { nameof(ART.LiveRowId) });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Baselines), new List<string> { nameof(Baselines.LiveRowId)});
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Labs), new List<string> { nameof(Labs.LiveRowId) });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(PatientStatus), new List<string> { nameof(PatientStatus.LiveRowId) });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Pharmacy), new List<string> { nameof(Pharmacy.LiveRowId)  });
                    Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Visits), new List<string> { nameof(Visits.LiveRowId)  });

                    var patients = (Slapper.AutoMapper.MapDynamic<Patients>(results) as IEnumerable<Patients>).ToList();


                    return new PagedProfileExtract(pageNumber, pageSize, pageCount, patients);
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

        public async Task<long> GetCountFrom(ExtractDefinition definition,string fromSource,int[] siteCode)
        {
            try
            {
                var sql = definition.GenerateCountScript(fromSource);
                using (var cn = GetConnection())
                {
                    cn.Open();
                    var results = await cn.QueryAsync<long>(sql,new {siteCode});

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
