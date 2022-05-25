using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTNotificationService : IBTNotificationService
    {

        private readonly ApplicationDbContext _context;

        private readonly IEmailSender _emailSender;

        private readonly IBTRolesService _roleService;

        public BTNotificationService(ApplicationDbContext context, IEmailSender emailSender, IBTRolesService roleService)
        {
            _context = context;
            _roleService = roleService;
            _emailSender = emailSender;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetReceiveNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                    .Include(n => n.Recipient)
                     .Include(n => n.Sender)
                      .Include(n => n.Ticket)
                      .ThenInclude(t => t.Project)
                      .Where(n => n.RecipientId == userId)
                      .ToListAsync();

                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await _context.Notifications
                    .Include(n => n.Recipient)
                     .Include(n => n.Sender)
                      .Include(n => n.Ticket)
                      .ThenInclude(t => t.Project)
                      .Where(n => n.SenderId == userId)
                      .ToListAsync();

                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            BTUser btUser = await _context.Users.FirstOrDefaultAsync(n => n.Id == notification.Recipient.Id);

            if (btUser != null)
            {
                string btUserEmail = btUser.Email;
                string message = notification.Message;

                //send email
                try
                {
                    await _emailSender.SendEmailAsync(btUserEmail, emailSubject, message);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task  SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<BTUser> members = await _roleService.GetUsersInRolesAsync(role, companyId);

                foreach (BTUser bTUser in members)
                {
                    notification.RecipientId = bTUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members)
        {
            try
            {
                foreach (BTUser bTUser in members)
                {
                    notification.RecipientId = bTUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

       
    }
}
