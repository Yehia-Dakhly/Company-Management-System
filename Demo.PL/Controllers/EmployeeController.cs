using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        // Ask CLr for Creating object implement IEmployeeRepository Interface
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string name)
        {
            #region View Bag & Data
            //// 1. ViewData[] => KeyValuePair[Dicionary Object] .Net Framework 3.5
            //// Dicionary Object That Transfer Data From [Controller] Action To it's View
            //ViewData["Message"] = "Hello From View Date!";

            //// 2. ViewBag[] => Dynamic Property Based on Dynamic Keyword .Net Framework 4.0
            //// Dicionary Object That Transfer Data From [Controller] Action To it's View
            //ViewBag.Message = "Hello From View Bag!"; 
            #endregion
            IEnumerable<Employee> employees;
            if (name is not null)
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(name);
            else
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);
        }
        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                #region Manual Mapper
                //var MappedEmployee = new Employee()
                //{
                //    Name = employeeVM.Name,
                //    Address = employeeVM.Address,
                //    Age = employeeVM.Age,
                //    DepartmentId = employeeVM.DepartmentId,
                //    Phone = employeeVM.Phone,
                //    Salary = employeeVM.Salary,
                //};
                //Employee employee = (Employee) employee; 
                #endregion

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                // 1. Update
                // 2. Delete
                // 3. Update
                // DbContext.Set<T>().Add();
                if(await _unitOfWork.CompleteAsync() > 0)
                {
                    TempData["Message"] = $"Employee {MappedEmployee.Name} Has Been Created";
                }
                return RedirectToAction("Index");
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? Id, string ViewName = "Details")
        {
            if (Id == null)
                return BadRequest();
            var Employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(Id.Value);
            if (Employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            return View(ViewName, MappedEmployee);
        }
        public async Task<IActionResult> Edit(int? Id)
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(Id, nameof(Edit));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? Id, EmployeeViewModel employeeVM)
        {
            if (employeeVM.Id != Id.Value)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    MappedEmployee.ImageName = Helpers.DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
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
            return View(employeeVM);
        }
        public async Task<IActionResult> Delete(int? Id) 
        {
            return await Details(Id, nameof(Delete));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, EmployeeViewModel employeeVM)
        {
            if(employeeVM.Id != Id.Value)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                    if (await _unitOfWork.CompleteAsync() > 0 && employeeVM.ImageName is not null)
                    {
                        Helpers.DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    }
                    //_unitOfWork.Dispose();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
    }
}
