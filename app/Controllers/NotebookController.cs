using Codesanook.EFNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Codesanook.EFNote.Controllers
{
    public class NotebookController : Controller
    {
        private NoteDbContext dbContext;

        public NotebookController(NoteDbContext dbContext) => this.dbContext = dbContext;

        public IActionResult Index()
        {
            var notebooks = dbContext.Notebooks
                .Include(b => b.Notes)
                .OrderBy(b => b.Name)
                .ToList();
            return View(notebooks);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Notebook notebook)
        {
            notebook.Name = notebook.Name.Trim();
            dbContext.Notebooks.Add(notebook);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var notebook = dbContext.Notebooks.Find(id);
            return View(notebook);
        }

        [HttpPost]
        public IActionResult Edit(int id, Notebook notebook)
        {
            notebook.Name = notebook.Name.Trim();
            var existingNotebook = dbContext.Notebooks.Find(id);
            existingNotebook.Name = notebook.Name;
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var notebook = dbContext.Notebooks.Include(b => b.Notes).Single(b => b.Id == id);
            return View(notebook);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection _)
        {
            var notebook = dbContext.Notebooks
                .Include(b => b.Notes)
                .ThenInclude(n => n.Tags)
                .Single(b => b.Id == id);
            var notes = notebook.Notes;

            var noteAndItsTag =
                from note in notes
                from tag in note.Tags
                select (note, tag);

            // Remove tag from note 
            foreach (var (note, tag) in noteAndItsTag)
            {
                note.Tags.Remove(tag);
            }

            // Remove note
            foreach (var note in notebook.Notes)
            {
                dbContext.Notes.Remove(note);
            }

            // Remote notebook
            dbContext.Notebooks.Remove(notebook);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
