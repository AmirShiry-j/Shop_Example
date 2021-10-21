using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Shop_Example.Entities.Models;

namespace Shop_Example.Web.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public AddressController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;

            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            var userActive = await _userManager.GetUserAsync(User);

            bool hasAddress = _unitOfWork.AddressRepository.GetAllAsync(p => p.UserId == userActive.Id).Result.Any();
            if (hasAddress)
            {
                return RedirectToAction("Edit");
            }

            var Uniteds = new List<SelectListItem>();
            Uniteds.Add(new SelectListItem { Text = "استان محل سکونت", Value = "0", Selected = true, Disabled = true });

            Uniteds.AddRange(_unitOfWork.UnitedRepository.GetAllAsync().Result.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString()
            }).ToList());

            var model = new CreateAddressDto
            {
                Uniteds = Uniteds,
                Cities = new List<SelectListItem> { new SelectListItem { Text = "شهرستان محل سکونت", Value = "0", Selected = true, Disabled = true } }
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressDto model)
        {
            var userActive = await _userManager.GetUserAsync(User);

            var city = _unitOfWork.CityRepository.GetAllAsync(p => p.Id == model.CityId, p => p.United).Result.SingleOrDefault();

            if (ModelState.IsValid)
            {
                if (city != null)
                {
                    Address newAddress = new Address
                    {
                        RecipientName = model.RecipientName,
                        FullAddress = model.FullAddress,
                        PostalCode = model.PostalCode,
                        UserId = userActive.Id,
                        CityId = model.CityId
                    };

                    var resAdd = await _unitOfWork.AddressRepository.AddAsync(newAddress);

                    if (resAdd)
                    {
                        return RedirectToAction("Address", "Profile");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                    return NotFound("همچین شهری موجود نیس");
            }
            else
            {
                var Uniteds = new List<SelectListItem>();
                Uniteds.Add(new SelectListItem { Text = "استان محل سکونت", Value = "0", Selected = true, Disabled = true });

                Uniteds.AddRange(_unitOfWork.UnitedRepository.GetAllAsync().Result.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString(),
                    Selected = f.Id != city?.United?.Id ? false : true
                }).ToList());

                var Cities = new List<SelectListItem>();

                Cities.AddRange(_unitOfWork.CityRepository.GetAllAsync(w => w.UnitedId == city.UnitedId)
                .Result.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                    Selected = p.Id != city?.Id ? false : true
                }));

                model.Uniteds = Uniteds;
                model.Cities = Cities;

                return View(model);
            }
        }

        public JsonResult GetCities(int UnitedId)
        {
            var Cities = new List<CitiesInDropDownDto>();

            Cities.AddRange(_unitOfWork.CityRepository.GetAllAsync(w => w.UnitedId == UnitedId)
                .Result.Select(p => new CitiesInDropDownDto
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }));

            var JsonFile = Json(Cities.ToArray());

            return JsonFile;
        }

        public async Task<IActionResult> Edit()
        {
            var userActive = await _userManager.GetUserAsync(User);

            var address = _unitOfWork.AddressRepository.GetAllAsync(p => p.UserId == userActive.Id, p => p.City, p => p.City.United).Result.SingleOrDefault();

            if (address == null || userActive == null)
            {
                return BadRequest();
            }

            var uniteds = new List<SelectListItem>();
            uniteds.Add(new SelectListItem { Text = "استان محل سکونت", Value = "0", Disabled = true });

            uniteds.AddRange(_unitOfWork.UnitedRepository.GetAllAsync().Result.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id != address.City.UnitedId ? false : true
            }));

            var cities = new List<SelectListItem>();
            cities.AddRange(_unitOfWork.CityRepository.GetAllAsync(p => p.UnitedId == address.City.UnitedId).Result.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id != address.CityId ? false : true
            }));

            var model = new EditAddressDto
            {
                AddressId = address.AddressId,
                CityId = address.CityId,
                UnitedId = address.City.UnitedId,
                FullAddress = address.FullAddress,
                PostalCode = address.PostalCode,
                RecipientName = address.RecipientName,
                Uniteds = uniteds,
                Cities = cities
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAddressDto model)
        {
            var userActive = await _userManager.GetUserAsync(User);

            if (userActive == null)
                return BadRequest();

            var city = _unitOfWork.CityRepository.GetAllAsync(p => p.Id == model.CityId, p => p.United).Result.SingleOrDefault();

            if (ModelState.IsValid)
            {
                if (city != null)
                {
                    var editAddress = await _unitOfWork.AddressRepository.GetByIdAsync(model.AddressId);

                    if (editAddress == null)
                        return BadRequest();

                    editAddress.CityId = city.Id;
                    editAddress.PostalCode = model.PostalCode;
                    editAddress.RecipientName = model.RecipientName;
                    editAddress.FullAddress = model.FullAddress;

                    var resEdit = await _unitOfWork.AddressRepository.UpdateAsync(editAddress);

                    if (resEdit)
                    {
                        return RedirectToAction("Address", "Profile");
                    }
                    else
                    {
                        return BadRequest();
                    }

                }

                return Content("OK");
            }

            var uniteds = new List<SelectListItem>();
            uniteds.Add(new SelectListItem { Text = "استان محل سکونت", Value = "0", Selected = true, Disabled = true });

            uniteds.AddRange(_unitOfWork.UnitedRepository.GetAllAsync().Result.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id != city.UnitedId ? false : true
            }));

            var cities = new List<SelectListItem>();
            cities.AddRange(_unitOfWork.CityRepository.GetAllAsync(p => p.UnitedId == city.UnitedId).Result.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id != city.Id ? false : true
            }));

            model.Uniteds = uniteds;
            model.Cities = cities;

            return View(model);
        }

        public async Task<IActionResult> Delete()
        {
            var userActive = await _userManager.GetUserAsync(User);

            var address = _unitOfWork.AddressRepository.GetAllAsync(p => p.UserId == userActive.Id).Result.SingleOrDefault();

            if (address != null)
            {
                var resDelete = await _unitOfWork.AddressRepository.RemoveAsync(address);

                if (resDelete)
                {
                    return RedirectToAction("Address", "Profile");
                }
                else
                {
                    return Content("حذف نشد");
                }
            }

            return Content("برای شما آدرسی ثبت نشده");
        }

    }
}
