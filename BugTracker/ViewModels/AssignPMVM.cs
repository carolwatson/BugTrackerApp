using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class AssignPMVM
    {
        
        public Project Project { get; set; }
        public SelectList PMList { get; set; }

        public string PMId { get; set; }
    }
}
