using System;

namespace Dwapi.Exchange.Contracts
{
    public class ART
    {
        public int? FacilityCode {get;set;}
        public string County {get;set;}
        public Guid PatientID {get;set;}
        public DateTime? DOB {get;set;}
        public DateTime? StartARTDate {get;set;}
        public DateTime? PreviousARTStartDate {get;set;}
        public DateTime? StartARTAtThisFacility {get;set;}
        public string PreviousARTRegimen {get;set;}
        public string StartRegimen {get;set;}
        public string StartRegimenLine {get;set;}
        public DateTime? LastARTDate {get;set;}
        public string LastRegimen {get;set;}
        public string LastRegimenLine {get;set;}
        public decimal? Duration {get;set;}
        public DateTime? ExpectedReturn {get;set;}
        public DateTime? LastVisit {get;set;}
        public string ExitReason {get;set;}
        public DateTime? ExitDate {get;set;}
        public long LiveRowId {get;set;}
    }
}