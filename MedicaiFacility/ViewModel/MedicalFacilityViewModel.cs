namespace MedicaiFacility.RazorPage.ViewModel
{
    public class MedicalFacilityViewModel
    {
        public int? FacilityId { get; set; }

        public string FacilityName { get; set; }

        public string Address { get; set; }
        public string Serivice { get; set; }
        public string FacilityType { get; set; }

        public bool? Verified { get; set; }

        public string ContactInfo { get; set; }

        public bool? IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public List<string>? DepartmentName { get; set; }  = new List<string>();
    }
}
