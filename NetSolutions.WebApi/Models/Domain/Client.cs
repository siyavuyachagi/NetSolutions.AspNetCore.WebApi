using NetSolutions.WebApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSolutions.WebApi.Models.Domain;

public class Client : ApplicationUser
{
    public virtual Subscription Subscription { get; set; }
    public virtual List<Project> Projects { get; set; }
    public virtual List<Client_SolutionBookmark> Client_SolutionBookmarks { get; set; }
}
