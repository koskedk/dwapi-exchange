using System;

namespace Dwapi.Exchange.Contracts
{
    public class Visits
    {
        public int? FacilityCode { get; set; }
        public string County { get; set; }
        public Guid PatientID { get; set; }
        public int? VisitID { get; set; }
        public DateTime? VisitDate { get; set; }
        public string Service { get; set; }
        public string visitType { get; set; }
        public int? WhoStage { get; set; }
        public string WabStage { get; set; }
        public string Pregnant { get; set; }
        public DateTime? LMP { get; set; }
        public DateTime? EDD { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string BP { get; set; }
        public string OI { get; set; }
        public DateTime? OIDate { get; set; }
        public DateTime? SubstitutionFirstLineRegimenDate { get; set; }
        public string SubstitutionFirstLineRegimenReason { get; set; }
        public DateTime? SubstitutionSecondLineRegimenDate { get; set; }
        public string SubstitutionSecondLineRegimenReason { get; set; }
        public DateTime? SecondLineRegimenChangeDate { get; set; }
        public string SecondLineRegimenChangeReason { get; set; }
        public string Adherence { get; set; }
        public string AdherenceCategory { get; set; }
        public string FamilyPlanningMethod { get; set; }
        public string PWP { get; set; }
        public decimal? GestationAge { get; set; }
        public string StabilityAssessment { get; set; }
        public string DifferentiatedCare { get; set; }
        public string PopulationType { get; set; }
        public string KeyPopulationType { get; set; }
        public DateTime? NextAppointmentDate { get; set; }
        public long LiveRowId { get; set; }
    }
}