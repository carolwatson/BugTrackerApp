﻿//using System;
//using System.ComponentModel.DataAnnotations;

//namespace BugTracker.Models
//{
//    public class Invite
//    {
//        public int Id { get; set; }

//        [DataType(DataType.Date)]
//        [Display(Name = "Date Sent")]
//        public DateTimeOffset InviteDate { get; set; }

//        [DataType(DataType.Date)]
//        [Display(Name = "Date Joined")]
//        public DateTimeOffset JoinDate { get; set; }

//        [Display(Name = "Code")]
//        public Guid CompanyToken { get; set; }

//        [Display(Name = "Invitee")]
//        public string InviteeId { get; set; }

//        [Display(Name = "Invitor")]
//        public string InvitorId { get; set; }

//        [Display(Name = "Project")]
//        public int ProjectId { get; set; }

//        [Display(Name = "Company")]
//        public int CompanyId { get; set; }

//        [Display(Name = "Invitee First Name")]
//        public string InviteeFirstName { get; set; }

//        [Display(Name = "Invitee Last Name")]
//        public string InviteeLastName { get; set; }

//        [Display(Name = "Invitee Email")]
//        public string InviteeEmail { get; set; }

        
//        public bool IsValid { get; set; }

//        public virtual BTUser Invitee { get; set; }

//        public virtual BTUser Invitor { get; set; }

//        public virtual Company Company { get; set; }

//        public virtual Project Project { get; set; }
//    }
//}
