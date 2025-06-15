using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContextName _context;

        public EmployeesController(EmployeeDbContextName context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, string operation)
        {
            Employee employee = await _context.Employees.FindAsync(id);
            EmployeeViewModel empViewModel = new EmployeeViewModel
            {
                Employees = await _context.Employees.ToListAsync(),
                Employee = employee ?? new Employee(),
                Operation = operation ?? "Insert"
            };
            return View(empViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost(EmployeeViewModel empViewModel)
        {
            if (ModelState.IsValid)
            {
                if (empViewModel.Operation == "Insert")
                {
                    _context.Add(empViewModel.Employee);
                    await _context.SaveChangesAsync();
                    empViewModel.Employees = await _context.Employees.ToListAsync();
                    return RedirectToAction("Index", empViewModel);
                }
                else if (empViewModel.Operation == "Update")
                {
                    _context.Update(empViewModel.Employee);
                    await _context.SaveChangesAsync();
                    empViewModel.Employees = await _context.Employees.ToListAsync();
                    empViewModel.Operation = "Insert";
                    return RedirectToAction("Index");
                }
                else if (empViewModel.Operation == "Delete")
                {
                    var employee = await _context.Employees.FindAsync(empViewModel.Employee.Id);
                    if (employee != null)
                    {
                        _context.Employees.Remove(employee);
                    }
                    await _context.SaveChangesAsync();
                    empViewModel.Employees = await _context.Employees.ToListAsync();
                    empViewModel.Operation = "Insert";
                    return RedirectToAction("Index");
                }
                else if (empViewModel.Operation == "Details")
                {
                    empViewModel.Employee = new Employee();
                    empViewModel.Employees = await _context.Employees.ToListAsync();
                    empViewModel.Operation = "Insert";
                    return RedirectToAction("Index");
                }
            }
            return View(empViewModel.Employee);
        }
    }
}
