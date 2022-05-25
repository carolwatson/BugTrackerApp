//using BugTracker.Data;
//using BugTracker.Models;
//using BugTracker.Services.Interfaces;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BugTracker.Services
//{
//    public class BTInviteService : IBTInviteService
//    {
//        private readonly ApplicationDbContext _context;
     

//        public BTInviteService(ApplicationDbContext context)
//        {
//            _context = context;
           
//        }

//        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
//        {
//            InviteModel invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

//            if (invite == null)
//            {
//                return false;
//            }

//            try
//            {
//                invite.IsValid = false;
//                invite.InviteeId = userId;
//                await _context.SaveChangesAsync();

//                return true;
//            }
//            catch
//            {
//                throw;
//            }
           
//        }

//        public async Task AddNewInviteAsync(InviteModel invite)
//        {
//            try
//            {
//                await _context.AddAsync(invite);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task<bool> AnyInviteAsync(Guid token, string email, int companyId)
//        {
//            try
//            {
//                bool result = await _context.Invites.Where(i => i.Id == companyId)
//                    .AnyAsync(i => i.CompanyToken == token && i.InviteeEmail == email);
//                return result;

//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task<InviteModel> GetInviteAsync(Guid token, string email, int companyId)
//        {
//            try
//            {
//                InviteModel invite = await _context.Invites.Where(i => i.CompanyId == companyId)
//                    .Include(i => i.Company)
//                    .Include(i => i.Project)
//                    .Include(i => i.Invitor)
//                    .FirstOrDefaultAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

//                return invite;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task<InviteModel> GetInviteAsync(int inviteId, int companyId)
//        {
//            try
//            {
//                InviteModel invite = await _context.Invites.Where(i => i.CompanyId == companyId)
//                    .Include(i => i.Company)
//                    .Include(i => i.Project)
//                    .Include(i => i.Invitor)
//                    .FirstOrDefaultAsync(i => i.Id == inviteId);

//                return invite;
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//        }

//        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
//        {
//            try
//            {
//                if (token == null)
//                {
//                    return false;
//                }

//                bool result = false;

//                InviteModel invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

//                if (invite != null)
//                {
//                    // get date, can convert to datime useing offset
//                    DateTime inviteDate = invite.InviteDate.DateTime;

//                    //create custom validation of invite based on the date it was issued
//                    // we allow invite to be valid for 7 days (send reminder day b4)

//                    bool validDate = (DateTime.Now - inviteDate).TotalDays <= 7;

//                    if (validDate)
//                    {
//                        result = invite.IsValid;
//                    }
//                }
//                return result;

//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//    }
//}
