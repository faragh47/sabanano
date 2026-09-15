using System;
namespace CleanArchitecture.Application.Common.Mappings
{
    public class BaseSearchDto
    {
        public long? Id { get; set; }
        public int? PageNumber { get; set; }
        public int? RecordsPerPage { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatorId { get; set; }
        public long? ModifierId { get; set; }
        public string FromModificationDate { get; set; }
        public string ToModificationDate { get; set; }
        public string FromCreationDate { get; set; }
        public string ToCreationDate { get; set; }


        public BaseSearchDto()
        {
            if (RecordsPerPage is null)
                RecordsPerPage = 10;

            if (PageNumber is null)
                PageNumber = 1;

        }

    }


    public record BaseRecordSearchDto
    {
        public long? Id { get; set; }
        public int? PageNumber { get; set; }
        public int? RecordsPerPage { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatorId { get; set; }
        public long? ModifierId { get; set; }
        public string FromModificationDate { get; set; }
        public string ToModificationDate { get; set; }
        public string FromCreationDate { get; set; }
        public string ToCreationDate { get; set; }

        public BaseRecordSearchDto()
        {
            if (RecordsPerPage is  null)
                RecordsPerPage = 10;

            if (PageNumber is null)
                PageNumber = 1;
            
        }


    }
}

