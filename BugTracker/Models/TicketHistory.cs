using System;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }

        [Display(Name = "Updated Item")]
        public string Property { get; set; }

        [Display(Name = "Previous")]
        public string OldValue { get; set; }
        
        [Display(Name = "Current")]
        public string NewValue { get; set; }

        [Display(Name = "Date Modified")]
        public DateTimeOffset Created { get; set; }

        [Display(Name = "Description Of Change")]
        public string Description { get; set; }


        [Display(Name = "Team Member")]
        public string UserId { get; set; }

        public virtual BTUser User { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}
