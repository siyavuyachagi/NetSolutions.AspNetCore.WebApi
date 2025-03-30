using NetSolutions.WebApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class User_Metadata_Profile
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public string FileMetadataId { get; set; }
    [ForeignKey(nameof(FileMetadataId))]
    public virtual FileMetadata FileMetadata { get; set; }
}
