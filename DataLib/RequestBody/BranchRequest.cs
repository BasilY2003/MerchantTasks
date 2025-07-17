namespace CommonLib.RequestBody
{
    public class BranchRequest
    {
        public string BranchName { get; set; } = string.Empty;

        public long ContactPersonId { get; set; }
        public long CityId { get; set; }
        public long GovernateId { get; set; }
        public string? AlHai { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Region { get; set; }
        public string? Fax { get; set; }
        public string? Website { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Gps { get; set; }
        public bool Status { get; set; }
        public bool IsMainBranch { get; set; }
    }
}
