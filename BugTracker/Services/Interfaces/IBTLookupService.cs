using BugTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTLookupService
    {
        public Task<List<TicketPriority>> GetTicketPriorityAsync();

        public Task<List<TicketStatus>> GetTicketStatusAsync();

        public Task<List<TicketType>> GetTicketTypeAsync();

        public Task<List<ProjectPriority>> GetProjectPriorityAsync();
    }
}
