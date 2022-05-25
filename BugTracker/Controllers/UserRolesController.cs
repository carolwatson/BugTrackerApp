using BugTracker.Extensions;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using BugTracker.Services;
using BugTracker.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {

        private readonly IBTRolesService _roleService;
        private readonly IBTCompanyInfoService _companyInfoService;

        
        public UserRolesController(IBTRolesService roleService, IBTCompanyInfoService companyInfoService)
        {
            _roleService = roleService;
            _companyInfoService = companyInfoService;
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {

            List<ManageUserRolesVM> model = new List<ManageUserRolesVM>();


            int companyId = User.Identity.GetCompanyId().Value;
            List<BTUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            foreach (BTUser user in users)
            {
                ManageUserRolesVM viewModel = new ManageUserRolesVM();
                viewModel.BTUser = user;
                IEnumerable<string> selected = await _roleService.GetUserRolesAsync(user);
                viewModel.Roles = new MultiSelectList(await _roleService.GetRolesAsync(), "Name", "Name", selected);
                model.Add(viewModel);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesVM member)
        {

            int companyId = User.Identity.GetCompanyId().Value;
            BTUser btUser = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.BTUser.Id);


            IEnumerable<string> roles = await _roleService.GetUserRolesAsync(btUser);

            string userRole = member.SelectedRoles.FirstOrDefault();
            if (!string.IsNullOrEmpty(userRole))
            {
                if (await _roleService.RemoveUserFromRolesAsync(btUser, roles))
                {
                    await _roleService.AddUserToRoleAsync(btUser, userRole);
                }
            }

            return RedirectToAction(nameof(ManageUserRoles));
            }

    }

}



