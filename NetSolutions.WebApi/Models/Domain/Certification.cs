using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Certification
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid TechnicalSkillId { get; set; }
    [ForeignKey(nameof(TechnicalSkillId))]
    public virtual TechnicalSkill TechnicalSkill { get; set; }

    [Required]
    [MaxLength(200)]
    public string CertificationName { get; set; }

    [MaxLength(100)]
    public string IssuingOrganization { get; set; }

    // When certification was obtained
    public DateTime DateObtained { get; set; }

    // When certification expires (if applicable)
    public DateTime? ExpirationDate { get; set; }

    // Certificate ID or credential number
    [MaxLength(100)]
    public string CredentialId { get; set; }

    // URL to verify certification (if available)
    [MaxLength(500)]
    public string VerificationUrl { get; set; }
}
