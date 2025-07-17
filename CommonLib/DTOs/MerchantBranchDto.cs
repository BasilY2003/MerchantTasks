
using CommonLib.DTOs;

namespace CommonLib.DTOs
{
    public class MerchantBranchDto
    {




        public long Id { get; set; }
        public string Name { get; set; }
        public int BusinessType { get; set; }
        public bool Status { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ManagerName { get; set; }

        // Optional nested DTOs
        public MerchantGroupDto? MerchantsGroup { get; set; }

        public class Builder
        {
            private readonly MerchantDTO _dto;

            public Builder()
            {
                _dto = new MerchantDTO();
            }

            public Builder WithId(long id)
            {
                _dto.Id = id;
                return this;
            }

            public Builder WithName(string name)
            {
                _dto.Name = name;
                return this;
            }

            public Builder WithBusinessType(int businessType)
            {
                _dto.BusinessType = businessType;
                return this;
            }

            public Builder WithStatus(bool status)
            {
                _dto.Status = status;
                return this;
            }

            public Builder WithDeletedAt(DateTime? deletedAt)
            {
                _dto.DeletedAt = deletedAt;
                return this;
            }

            public Builder WithCreatedAt(DateTime createdAt)
            {
                _dto.CreatedAt = createdAt;
                return this;
            }

            public Builder WithUpdatedAt(DateTime updatedAt)
            {
                _dto.UpdatedAt = updatedAt;
                return this;
            }

            public Builder WithManagerName(string managerName)
            {
                _dto.ManagerName = managerName;
                return this;
            }

            // Optional: include group info
            public Builder WithMerchantsGroup(MerchantGroupDto groupDto)
            {
                _dto.MerchantsGroup = groupDto;
                return this;
            }

            // Optional: include branches
         

            public MerchantDTO Build()
            {
                return _dto;
            }
        }










    }
}
