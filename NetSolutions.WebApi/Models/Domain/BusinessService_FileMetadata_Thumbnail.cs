using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class BusinessService_FileMetadata_Thumbnail
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
