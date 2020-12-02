using System;

namespace Dwapi.Exchange.Contracts
{
    public class Labs
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientID { get; set; }
        public int? VisitId { get; set; }
        public DateTime? OrderedbyDate { get; set; }
        public DateTime? ReportedByDate { get; set; }
        public string TestName { get; set; }
        public string TestResult { get; set; }
        public string Reason { get; set; }
        public long LiveRowId { get; set; }
    }
}