using System;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [Display(Name = "Member Comment")]
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset Created { get; set; }

        public int TicketId { get; set; }

        [Display(Name = "Team Member")]
        public string UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }
    }
}
