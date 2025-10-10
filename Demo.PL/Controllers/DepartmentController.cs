using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    //[Authorize("Admin")]
    //[AllowAnonymous]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        // Ask CLR For Creating object of class that implement IDepartmentRepository
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = new DepartmentRepository();
        }

        // BaseURL/Department/Index

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department) // Bind department form Form!
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                if (await _unitOfWork.CompleteAsync() > 0)
                {
                    TempData["Message"] = "Department is Created!";
                    // 3. Temp Data -> KeyValuePair [Dicionary Object]
                    // Transfer Data From Action To Action
                    return RedirectToAction(nameof(Index));

                }
                
            }
            return View(department);
        }
        /*URLBase/Department/Details/Id*/
        public async Task<IActionResult> Details(int? Id, string ViewName = "Details")
        {
            if (Id is null)
            {
                return BadRequest(); // Status Code 400
            }
            var Department = await _unitOfWork.DepartmentRepository.GetByIdAsync(Id.Value);
            if (Department is null)
            {
                return NotFound();
            }
            return View(ViewName, Department);
        }
        public async Task<IActionResult> Edit(int? Id)
        {
            return await Details(Id, nameof(Edit));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department, [FromRoute] int Id)
        {
            if (Id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1. Log Exception
                    // 2. Form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            return await Details(Id, nameof(Delete));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int Id)
        {
            if(Id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Delete(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
    }
}
