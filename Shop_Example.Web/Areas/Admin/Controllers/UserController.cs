using Microsoft.AspNetCore.Mvc;
using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shop_Example.Dtoes.Admin.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Tools.TimeAndDate;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly Time _time;
        public UserController(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _time = new Time();
        }

        public async Task<IActionResult> Index(int Page = 1, string Search = "")
        {
            IEnumerable<User> users = _userManager.Users.Where(user => user.FullName.ToLower().Contains(Search.ToLower())).ToList();

            int countInPage = 10;

            var model = new IndexPageUsersVM
            {
                Page = Page,
                CounInPage = countInPage,
                CountAllItems = users.Count(),
            };

            model.Users = users.Skip((Page - 1) * model.CounInPage).Take(model.CounInPage)
                .Select(user => new ListInfoUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ConfirmedEmail = user.EmailConfirmed,
                    ConfirmedPhoneNumber = user.PhoneNumberConfirmed,
                    Roles = string.Join(',', _userManager.GetRolesAsync(user).Result)
                }).ToList();

            return View(model);
        }

        public async Task<IActionResult> BlockUsers()
        {
            var users = _unitOfWork.UserRepository.GetAllAsync(p => p.IsBlocked).Result.ToList();

            List<ListInfoUserDto> modelUsers = users.Select(user => new ListInfoUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ConfirmedEmail = user.EmailConfirmed,
                ConfirmedPhoneNumber = user.PhoneNumberConfirmed,
                Roles = string.Join(',', _userManager.GetRolesAsync(user).Result)
            }).ToList();

            return View(modelUsers);
        }

        public async Task<IActionResult> Detail(string UserId)
        {
            var user = _unitOfWork.UserRepository.GetAllAsync(p => p.Id == UserId
                                                    , include => include.Address).Result.FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            var roles = _userManager.GetRolesAsync(user).Result;

            var userModel = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImageProfileName = user.ImageProfileName,
                IsBlocked = user.IsBlocked,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Roles = roles?.ToList() ?? null
            };

            if (user.Address != null)
            {
                City city = _unitOfWork.CityRepository.GetAllAsync(p => p.Id == user.Address.CityId
                                                                , includ => includ.United).Result.FirstOrDefault();
                userModel.Address = new AddressDto
                {
                    FullAddress = user.Address.FullAddress,
                    PostalCode = user.Address.PostalCode,
                    RecipientName = user.Address.RecipientName,
                    UnitedAndCity = city.United.Name + " , " + city.Name
                };
            }

            var cart = _unitOfWork.CartRepository.GetAllAsync(p => p.UserId == UserId && p.Finished == false,
                                                             include => include.CartItems).Result.FirstOrDefault();

            if (cart != null)
            {
                userModel.Cart = new CartDto
                {
                    BrowserId = cart.BrowserId,
                    Finished = cart.Finished,
                    Id = cart.Id,
                    TimeCreate = cart.TimeCreate,
                    CartItems = cart.CartItems.Select(ci => new CartItemDto
                    {
                        Count = ci.Count,
                        Id = ci.Id,
                        Price = ci.Price,
                        TimeCreate = _time.ToShamsi(ci.TimeCreate),
                        ProductId = ci.ProductId,
                        ProductName = _unitOfWork.ProductRepository.GetByIdAsync(ci.ProductId).Result.Name
                    }).ToList()
                };
            }

            return View(userModel);
        }
        public async Task<IActionResult> AddRoleToUser(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return BadRequest();
            }

            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            var userRoles = new AddRoleToUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = ListItems
            };

            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserDto addRoleToUser)
        {
            if (ModelState.IsValid == false)
            {
                return View(addRoleToUser);
            }

            var user = _userManager.FindByIdAsync(addRoleToUser.Id).Result;
            var role = _roleManager.FindByIdAsync(addRoleToUser.Role).Result;

            if (user == null || role == null)
            {
                return BadRequest();
            }

            var resultAddRoleUser = _userManager.AddToRoleAsync(user, role.Name).Result;

            if (resultAddRoleUser.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string Message = "";

                foreach (var error in resultAddRoleUser.Errors)
                {
                    Message += error.Description;
                }

                ModelState.AddModelError("", Message);

                var ListItems = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                }).ToList();

                addRoleToUser.Roles = ListItems;

                return View(addRoleToUser);
            }
        }

        public async Task<IActionResult> RolesOfUser(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return BadRequest();
            }

            var roles = _userManager.GetRolesAsync(user).Result;

            ViewBag.UserId = user.Id;
            ViewBag.Email = user.Email;

            return View(roles);
        }

        public async Task<IActionResult> RemoveRoleToUser(string RoleName, string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return BadRequest();
            }

            if (RoleName == "Admin" && _userManager.GetUsersInRoleAsync("Admin").Result.Count == 1)
            {
                return BadRequest();
            }

            var resultRemoveRole = _userManager.RemoveFromRoleAsync(user, RoleName).Result;

            if (resultRemoveRole.Succeeded)
            {
                return RedirectToAction("RolesOfUser", new { UserId });
            }
            else
                return BadRequest();
        }

        public async Task<IActionResult> UsersOfRole(string RoleId)
        {
            var role = _roleManager.FindByIdAsync(RoleId).Result;
            if (role == null)
            {
                return BadRequest();
            }

            var users = _userManager.GetUsersInRoleAsync(role.Name).Result;

            IEnumerable<ListUsersInRoleDto> infoUsersInRole = users.Select(user => new ListUsersInRoleDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = string.Join(',', _userManager.GetRolesAsync(user).Result)
            }).ToList();

            var tuple = new Tuple<string, IEnumerable<ListUsersInRoleDto>>(role.Name, infoUsersInRole);

            return View(tuple);
        }


        public async Task<IActionResult> Create()
        {
            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            ListItems.Add(new SelectListItem { Text = "بدون نقش", Value = "بدون نقش", Selected = true });

            var addNew = new AddNewUserDto
            {
                Roles = ListItems
            };

            return View(addNew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddNewUserDto newUser)
        {
            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();
            ListItems.Add(new SelectListItem { Text = "بدون نقش", Value = "بدون نقش", Selected = true });

            newUser.Roles = ListItems;//برای اوکی کردن مقادیر هنگام برگشت

            if (ModelState.IsValid == false)
            {
                return View(newUser);
            }

            var user = new User
            {
                UserName = newUser.Email,
                Email = newUser.Email,
                FullName = newUser.FullName,

                //برای اضافه کردن کاربر فیک
                EmailConfirmed = true//تایید موقت ایمیل                
            };

            var result = _userManager.CreateAsync(user, newUser.Password).Result;

            if (result.Succeeded)
            {
                if (newUser.Role == "بدون نقش")
                {
                    return RedirectToAction("Index");
                }

                var role = _roleManager.FindByIdAsync(newUser.Role).Result;
                if (role == null)
                {
                    return BadRequest();
                }

                var result2 = _userManager.AddToRoleAsync(user, role.Name).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string MesRole = "";
                    foreach (var Mes in result2.Errors)
                    {
                        MesRole += Mes;
                    }

                    ModelState.AddModelError("", MesRole);
                    return View(newUser);
                }
            }
            else
            {
                string MesUser = "";
                foreach (var Mes in result.Errors)
                {
                    MesUser += Mes.Description;
                }

                ModelState.AddModelError("", MesUser);
                return View(newUser);
            }
        }

    }
}
