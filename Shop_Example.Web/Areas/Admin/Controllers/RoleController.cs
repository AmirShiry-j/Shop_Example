using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.Role;
using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public RoleController(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            List<RoleListDto> Roles = _roleManager.Roles.ToList().Select(r => new RoleListDto()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                CountUsers = _userManager.GetUsersInRoleAsync(r.Name).Result.Count
            }).ToList();

            return View(Roles);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(AddNewRoleDto newRoleDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(newRoleDto);
            }

            Role newRole = new Role
            {
                Name = newRoleDto.Name,
                Description = newRoleDto.Description
            };

            var resultAddNewRole = await _roleManager.CreateAsync(newRole);

            if (resultAddNewRole.Succeeded)
            {
                return Redirect("/Admin/Role/Index");
            }
            else
            {
                string Message = "";

                foreach (var error in resultAddNewRole.Errors)
                {
                    Message += error.Description;
                }

                ModelState.AddModelError("", Message);

                return View(newRoleDto);
            }
        }
        public async Task<IActionResult> Delete(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null || role?.Name == "Admin")
            {
                return BadRequest();
            }

            var resultDeleteRole = await _roleManager.DeleteAsync(role);

            if (resultDeleteRole.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
