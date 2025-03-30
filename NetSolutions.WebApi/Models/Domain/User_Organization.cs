using NetSolutions.WebApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

class User_Organization
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }

    public string OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
}
