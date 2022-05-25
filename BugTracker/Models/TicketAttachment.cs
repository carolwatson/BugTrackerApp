using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [Display(Name = "Ticket")]
        public int TicketId { get; set; }

        [Display(Name = "Team Member")]
        public string UserId { get; set; }

        [Display(Name = "File Date")]
        public DateTimeOffset Created { get; set; }



        //image properties
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile ImageFormFile { get; set; }

        [Display(Name = "File Name")]
        public string ImageFileName { get; set; }

        [Display(Name = "File Description")]
        public string Description { get; set; }

        public byte[] ImageFileData { get; set; }

        [Display(Name = "File Extension")]
        public string ImageContentType { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }

    }
}
