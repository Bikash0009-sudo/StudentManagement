using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();

            return View(students);
        }

        // GET: Student/Create
        public async Task<IActionResult> Create()
{
    ViewBag.Countries = new SelectList(
        await _context.Countries.ToListAsync(),
        "Id",
        "Name"
    );

    return View();
}

[HttpGet]
public async Task<JsonResult> GetStates(int countryId)
{
    var states = await _context.States
        .Where(s => s.CountryId == countryId)
        .Select(s => new
        {
            id = s.Id,
            name = s.Name
        })
        .ToListAsync();

    return Json(states);
}
[HttpGet]
public async Task<JsonResult> GetCities(int stateId)
{
    var cities = await _context.Cities
        .Where(c => c.StateId == stateId)
        .Select(c => new
        {
            id = c.Id,
            name = c.Name
        })
        .ToListAsync();

    return Json(cities);
}
        // POST: Student/Create
       [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Student student)
{
    if (ModelState.IsValid)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Reload countries if validation fails
    ViewBag.Countries = new SelectList(
        await _context.Countries.ToListAsync(),
        "Id",
        "Name",
        student.CountryId
    );

    return View(student);
}

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }


        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}