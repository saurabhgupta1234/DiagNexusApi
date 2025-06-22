namespace DiagNexusApi.Model
{
    public class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string PatientName { get; set; }
    }
}
