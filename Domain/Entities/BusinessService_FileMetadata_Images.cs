using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BusinessService_FileMetadata_Images
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid BusinessServiceId { get; set; }
        [ForeignKey(nameof(BusinessServiceId))]
        public virtual BusinessService BusinessService { get; set; }

        public string FileMetadataId { get; set; }
        [ForeignKey(nameof(FileMetadataId))]
        public virtual FileMetadata FileMetadata { get; set; }
    }

}

