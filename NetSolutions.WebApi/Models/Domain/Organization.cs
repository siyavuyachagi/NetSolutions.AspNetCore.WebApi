using NetSolutions.WebApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Organization
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual List<ApplicationUser> Users { get; set; }
}
