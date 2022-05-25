//using BugTracker.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BugTracker.Services.Interfaces
//{
//   public  interface IBTInviteService
//    {
//        Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId);

//        Task AddNewInviteAsync(InviteModel invite);

//        Task <bool> AnyInviteAsync(Guid Token, string email, int companyId);

//        Task<InviteModel> GetInviteAsync(Guid token, string email, int companyId);

//        Task<InviteModel> GetInviteAsync(int inviteId, int companyId);

//        Task<bool> ValidateInviteCodeAsync(Guid? token);

//    }
//}
