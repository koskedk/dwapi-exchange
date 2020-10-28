using System;
using System.Collections;
using System.Collections.Generic;
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
