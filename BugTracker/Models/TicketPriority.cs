using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }  //capital I change

        [Display(Name = "Ticket Priority")]
        public string Name { get; set; }
    }
}
