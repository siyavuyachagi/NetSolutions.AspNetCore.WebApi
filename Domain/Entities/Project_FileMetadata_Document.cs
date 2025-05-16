using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Project_FileMetadata_Document
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; }

        public string FileMetadataId { get; set; }
        [ForeignKey(nameof(FileMetadataId))]
        public virtual FileMetadata FileMetadata { get; set; }
    }

}

