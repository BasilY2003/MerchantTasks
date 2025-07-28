using DataLib.Resources;
using System.ComponentModel.DataAnnotations;

namespace CommonLib.RequestBody
{
    public class MerchantGroupRequest
    {

        [Required(
           ErrorMessageResourceType = typeof(SharedResource),
           ErrorMessageResourceName = "Required")]
        [StringLength(30,
            ErrorMessageResourceType = typeof(SharedResource),
            ErrorMessageResourceName = "StringLength")]
        public string Name { get; set; } = string.Empty;
  
    }
}
