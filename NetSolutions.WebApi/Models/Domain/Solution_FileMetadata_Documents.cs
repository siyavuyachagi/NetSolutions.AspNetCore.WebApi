using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Solution_FileMetadata_Document
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid SolutionId { get; set; }
    [ForeignKey(nameof(SolutionId))]
    public virtual Solution Solution { get; set; }

    public string FileMetadataId { get; set; }
    [ForeignKey(nameof(FileMetadataId))]
    public virtual FileMetadata FileMetadata { get; set; }
}
