using System;

namespace Dwapi.Exchange.Contracts
{
    public class Pharmacy
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientId { get; set; }
        public int? VisitID { get; set; }
        public string Drug { get; set; }
        public DateTime? DispenseDate { get; set; }
        public decimal? Duration { get; set; }
        public DateTime? ExpectedReturn { get; set; }
        public string TreatmentType { get; set; }
        public string PeriodTaken { get; set; }
        public string ProphylaxisType { get; set; }
        public string Provider { get; set; }
        public string RegimenLine { get; set; }
        public long LiveRowId { get; set; }
    }
}