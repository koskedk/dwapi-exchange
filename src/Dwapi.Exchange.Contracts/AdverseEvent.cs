using System;

namespace Dwapi.Exchange.Contracts
{
    public class AdverseEvent
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientId { get; set; }
        public DateTime? VisitDate { get; set; }
        public string adverseEvent { get; set; }
        public DateTime? AdverseEventStartDate { get; set; }
        public DateTime? AdverseEventEndDate { get; set; }
        public string Severity { get; set; }
        public string AdverseEventClinicalOutcome { get; set; }
        public string AdverseEventActionTaken { get; set; }
        public bool AdverseEventIsPregnant { get; set; }
        public string AdverseEventRegimen { get; set; }
        public string AdverseEventCause { get; set; }
        public long LiveRowId { get; set; }
    }
}