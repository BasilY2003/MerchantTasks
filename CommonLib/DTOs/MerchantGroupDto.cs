using CommonLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.DTOs
{
    public class MerchantGroupDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MerchantDTO> Merchants { get; set; } = new();

        public class Builder
        {
            private readonly MerchantGroupDto _dto = new();

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

            public Builder WithDeletedAt(DateTime? deletedAt)
            {
                _dto.DeletedAt = deletedAt;
                return this;
            }

            public Builder WithMerchants(List<MerchantDTO> merchants)
            {
                _dto.Merchants = merchants;
                return this;
            }

            public MerchantGroupDto Build()
            {
                return _dto;
            }
        }


    }

}
