using System.ComponentModel.DataAnnotations;



namespace CommonLib.RequestBody
{
    public class BranchRequest
    {
        [Required(ErrorMessage = "BranchName is required.")]
        public string BranchName { get; set; }


        [Required(ErrorMessage = "ContactPersonId is required.")]
        public long ContactPersonId { get; set; }


        [Required(ErrorMessage = "CityId is required.")]
        public long CityId { get; set; }


        [Required(ErrorMessage = "GovernateId is required.")]
        public long GovernateId { get; set; }

        public string? AlHai { get; set; }


        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Region is required.")]
        public int Region { get; set; }

        public string? Fax { get; set; }

        public string? Website { get; set; }


        [Required(ErrorMessage = "Mobile is required.")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; set; }

        public string? Gps { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "IsMainBranch is required.")]
        public bool IsMainBranch { get; set; }





    }
}
