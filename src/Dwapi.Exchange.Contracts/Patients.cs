using System;
using System.Collections.Generic;

namespace Dwapi.Exchange.Contracts
{

    public class Patients
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientID { get; set; }
        public string PatientCCCNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? DateConfirmedHIVPositive { get; set; }
        public DateTime? RegistrationAtCCC { get; set; }
        public DateTime? RegistrationATPMTCT { get; set; }
        public DateTime? RegistrationAtTBClinic { get; set; }
        public DateTime? TransferInDate { get; set; }
        public string PatientSource { get; set; }
        public string MaritalStatus { get; set; }
        public string EducationLevel { get; set; }
        public string Orphan { get; set; }
        public string Inschool { get; set; }
        public string PatientType { get; set; }
        public string PopulationType { get; set; }
        public string KeyPopulationType { get; set; }
        public string PreviousARTExposure { get; set; }
        public DateTime? PreviousARTStartDate { get; set; }
        public string ContactRelation { get; set; }
        public DateTime? LastVisit { get; set; }
        public string StatusATCCC { get; set; }
        public string statusAtPMTCT { get; set; }
        public string statusAtTBClinic { get; set; }
        public string PatientResidentCounty { get; set; }
        public string PatientResidentSubCounty { get; set; }
        public string PatientResidentLocation { get; set; }
        public string PatientResidentSubLocation { get; set; }
        public string PatientResidentWard { get; set; }
        public long LiveRowId { get; set; }
        public int?  Age { get; set; }
        public List<AdverseEvent> AdverseEvent { get; set; }=new List<AdverseEvent>();
        public List<ART> ART { get; set; }=new List<ART>();
        public List<Baselines> Baselines { get; set; }=new List<Baselines>();
        public List<Labs> Labs { get; set; }=new List<Labs>();
        public List<PatientStatus> PatientStatus { get; set; }=new List<PatientStatus>();
        public List<Pharmacy> Pharmacy { get; set; }=new List<Pharmacy>();
        public List<Visits> Visits { get; set; }=new List<Visits>();
    }
}
