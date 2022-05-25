using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [Display(Name = "Ticket Type")]
        public string Name { get; set; }
    }
}
