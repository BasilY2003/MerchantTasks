namespace DataLib.Requests
{
    public class SearchRequest
    {
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public int? BusinessType { get; set; }
        public string? ManagerName { get; set; }
    }
}
