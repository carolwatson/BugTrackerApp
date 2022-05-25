using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }


        public string Title { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset Created { get; set; }

        public bool Viewed { get; set; }

        public int TicketId { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual BTUser Recipient { get; set; }

        public virtual BTUser Sender { get; set; }
    }
}
