
namespace Domain.Entities
{
    public class Client : ApplicationUser
    {
        public virtual Subscription Subscription { get; set; }
        public virtual List<Project> Projects { get; set; }
    }

}

