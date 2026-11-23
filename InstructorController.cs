using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstructorInfoSystem.Data;
using InstructorInfoSystem.Models;

namespace InstructorInfoSystem.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BackupDatabase()
        {
            try
            {
                string connectionString =
                    _context.Database.GetConnectionString()!;

                string backupFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "DatabaseBackups"
                );

                Directory.CreateDirectory(backupFolder);

                string backupFileName =
                    $"InstructorInfoSystem_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.db";

                string backupPath =
                    Path.Combine(backupFolder, backupFileName);

                using var source = new SqliteConnection(connectionString);
                using var destination = new SqliteConnection(
                    $"Data Source={backupPath}"
                );

                source.Open();
                destination.Open();

                source.BackupDatabase(destination);

                TempData["SuccessMessage"] =
                    "Database backup created successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "Unable to create database backup.";
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: /Instructor
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 5;

            var instructors = _context.Instructors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                instructors = instructors.Where(i =>
                    i.InstructorId.Contains(searchString) ||
                    i.FullName.Contains(searchString) ||
                    i.Designation.Contains(searchString) ||
                    i.Majorship.Contains(searchString) ||
                    i.ContactNumber.Contains(searchString) ||
                    i.Email.Contains(searchString));
            }

            // Count matching records
            int totalRecords = await instructors.CountAsync();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Keep page within valid limits
            if (totalPages > 0)
            {
                page = Math.Clamp(page, 1, totalPages);
            }
            else
            {
                page = 1;
            }

            // Get records for the current page
            var pagedInstructors = await instructors
                .OrderBy(i => i.InstructorId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Dashboard information
            ViewBag.TotalInstructors = await _context.Instructors.CountAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;

            ViewData["CurrentFilter"] = searchString;

            return View(pagedInstructors);
        }
        // GET: /Instructor/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return View(instructor);
            }

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Instructor/Details
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: /Instructor/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: /Instructor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return View(instructor);
            }

            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Instructor/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: /Instructor/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
