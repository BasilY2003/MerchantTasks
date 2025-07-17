using System.ComponentModel.DataAnnotations;
using DataLib.Resources;
namespace CommonLib.RequestBody
{
    public class MerchantRequest
    {
        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "Required")]
        [StringLength(50,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "StringLength")]
        public string Name { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "Required")]
        public int BusinessType { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "Required")]
        public bool Status { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "Required")]
        [StringLength(50,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "StringLength")]
        public string ManagerName { get; set; }
    }
}
