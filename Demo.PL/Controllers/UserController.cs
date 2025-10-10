using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    //[Authorize("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AuthUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Email = U.Email,
                    FName = U.FName,
                    Id = U.Id,
                    LName = U.LName,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);
                if (User is not null)
                {
                    var MappedUser = new UserViewModel()
                    {
                        Email = User.Email,
                        FName = User.FName,
                        Id = User.Id,
                        LName = User.LName,
                        PhoneNumber = User.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(User).Result
                    };
                    return View(new List<UserViewModel>() { MappedUser });
                }
            }
            return View();
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
                return NotFound();
            var MappedUser = _mapper.Map<AuthUser, UserViewModel>(User);
            return View(ViewName, MappedUser);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if(id != model.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    var User = await _userManager.FindByEmailAsync(model.Email); // Unchanged
                    User.FName = model.FName; // Modified
                    User.LName = model.LName; // Modified
                    User.PhoneNumber = model.PhoneNumber;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message); 
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
        {
            try
            {
                var User = await _userManager.FindByEmailAsync(id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
