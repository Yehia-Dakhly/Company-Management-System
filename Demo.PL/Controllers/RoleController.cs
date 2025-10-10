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

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _role;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> role, IMapper mapper)
        {
            _role = role;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Roles = await _role.Roles.ToListAsync();
                var MappedRole = _mapper.Map<IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRole);
            }
            else
            {
                var Role = await _role.FindByNameAsync(SearchValue);
                var MappedRole = _mapper.Map<RoleViewModel>(Role);
                return View(new List<RoleViewModel>() { MappedRole });
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _role.CreateAsync(MappedRole);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Role = await _role.FindByIdAsync(id);
            if (Role is null)
                return NotFound();
            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName, MappedRole);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id) 
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _role.FindByIdAsync(model.Id); // Unchanged
                    Role.Name = model.RoleName; // Modified
                    await _role.UpdateAsync(Role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
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
                var Role = await _role.FindByIdAsync(id);
                await _role.DeleteAsync(Role);
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
