using System;

namespace Dwapi.Exchange.Contracts
{
    public class PatientStatus
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientId { get; set; }
        public string ExitReason { get; set; }
        public string ExitDescription { get; set; }
        public DateTime? ExitDate { get; set; }
        public long LiveRowId { get; set; }
    }
}