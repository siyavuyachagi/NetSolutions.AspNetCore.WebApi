using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Solution_FileMetadata_Image
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid SolutionId { get; set; }
    [ForeignKey(nameof(SolutionId))]
    public virtual Solution Solution { get; set; }

    public string FileMetadaId { get; set; }
    [ForeignKey(nameof(FileMetadaId))]
    public virtual FileMetadata FileMetadata { get; set; }
}
