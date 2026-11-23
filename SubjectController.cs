using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InstructorInfoSystem.Data;
using InstructorInfoSystem.Models;

namespace InstructorInfoSystem.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        // READ: lists all subjects
        public IActionResult Index()
        {
            return View(SubjectRepository.GetAll());
        }

        // CREATE: shows the empty form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE: saves the new subject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subject subject)
        {
            if (!ModelState.IsValid) return View(subject);
            SubjectRepository.Add(subject);
            return RedirectToAction("Index");
        }

        // UPDATE: shows the form filled with the existing subject
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subject = SubjectRepository.GetById(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        // UPDATE: saves the edited subject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subject subject)
        {
            if (!ModelState.IsValid) return View(subject);
            SubjectRepository.Update(subject);
            return RedirectToAction("Index");
        }

        // DELETE: asks for confirmation before removing
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var subject = SubjectRepository.GetById(id);
            if (subject == null) return NotFound();
            return View(subject);
        }

        // DELETE: removes the subject
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            SubjectRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}