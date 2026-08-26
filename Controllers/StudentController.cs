using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // =========================
        // STUDENT LIST
        // =========================
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();

            return View(students);
        }


        // =========================
        // CREATE - GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCountries();

            return View();
        }


        // =========================
        // GET STATES
        // =========================
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


        // =========================
        // GET CITIES
        // =========================
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


        // =========================
        // CREATE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Find selected Country
            var country = await _context.Countries
                .FirstOrDefaultAsync(c => c.Id == student.CountryId);

            // Find selected State
            var state = await _context.States
                .FirstOrDefaultAsync(s => s.Id == student.StateId);

            // Find selected City
            var city = await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == student.CityId);

            // Check that all three selections are valid
            if (country == null)
            {
                ModelState.AddModelError(
                    "CountryId",
                    "Please select a country."
                );
            }

            if (state == null)
            {
                ModelState.AddModelError(
                    "StateId",
                    "Please select a state."
                );
            }

            if (city == null)
            {
                ModelState.AddModelError(
                    "CityId",
                    "Please select a city."
                );
            }

            // Store the names
            if (country != null)
            {
                student.Country = country.Name;
            }

            if (state != null)
            {
                student.State = state.Name;
            }

            if (city != null)
            {
                student.City = city.Name;
            }

            // Save student
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload countries
            await LoadCountries(student.CountryId);

            return View(student);
        }


        // =========================
        // EDIT - GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
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

            // Load countries
            await LoadCountries(student.CountryId);

            return View(student);
        }


        // =========================
        // EDIT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            // Find selected Country
            var country = await _context.Countries
                .FirstOrDefaultAsync(c => c.Id == student.CountryId);

            // Find selected State
            var state = await _context.States
                .FirstOrDefaultAsync(s => s.Id == student.StateId);

            // Find selected City
            var city = await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == student.CityId);

            // Validate selections
            if (country == null)
            {
                ModelState.AddModelError(
                    "CountryId",
                    "Please select a country."
                );
            }

            if (state == null)
            {
                ModelState.AddModelError(
                    "StateId",
                    "Please select a state."
                );
            }

            if (city == null)
            {
                ModelState.AddModelError(
                    "CityId",
                    "Please select a city."
                );
            }

            // Convert IDs into names
            if (country != null)
            {
                student.Country = country.Name;
            }

            if (state != null)
            {
                student.State = state.Name;
            }

            if (city != null)
            {
                student.City = city.Name;
            }

            // Update student
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Reload countries if validation fails
            await LoadCountries(student.CountryId);

            return View(student);
        }


        // =========================
        // DELETE - GET
        // =========================
        [HttpGet]
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


        // =========================
        // DELETE - POST
        // =========================
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students
                .FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // LOAD COUNTRIES
        // =========================
        private async Task LoadCountries(int? selectedCountryId = null)
        {
            ViewBag.Countries = new SelectList(
                await _context.Countries
                    .ToListAsync(),
                "Id",
                "Name",
                selectedCountryId
            );
        }
    }
}