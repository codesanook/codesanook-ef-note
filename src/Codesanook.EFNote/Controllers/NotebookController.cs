using Codesanook.EFNote.Models;
using Codesanook.EFNote.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Codesanook.EFNote.Controllers
{
    public class NotebookController : Controller
    {
        private NoteDbContext dbContext;
        public NotebookController(NoteDbContext dbContext) => this.dbContext = dbContext;

        public async Task<IActionResult> IndexAsync()
        {
            var notebooks = await dbContext.Notebooks
                .Include(b => b.Notes)
                .OrderBy(b => b.Name)
                .ToListAsync();

            return View(notebooks);
        }

        public async Task<IActionResult> ChangedLog()
        {
            var notebooks = await dbContext.Notebooks
                .TemporalAll()
                .OrderBy(b => b.Name)
                .Select(b => new NotebookChangedLogViewModel()
                {
                    Notebook = b,
                    ValidFrom = EF.Property<DateTime>(b, "PeriodStart"),
                    ValidTo = EF.Property<DateTime>(b, "PeriodEnd")
                })
                .ToListAsync();
            return View(notebooks);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Notebook notebook)
        {
            notebook.Name = notebook.Name.Trim();
            await dbContext.Notebooks.AddAsync(notebook);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            var notebook = await dbContext.Notebooks.FindAsync(id);
            return View(notebook);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, Notebook notebook)
        {
            notebook.Name = notebook.Name.Trim();
            var existingNotebook = await dbContext.Notebooks.FindAsync(id);
            existingNotebook.Name = notebook.Name;
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var notebook = await dbContext.Notebooks
                .Include(b => b.Notes)
                .SingleAsync(b => b.Id == id);

            return View(notebook);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id, IFormCollection _)
        {
            var notebook = await dbContext.Notebooks
                .Include(b => b.Notes)
                .ThenInclude(n => n.Tags)
                .SingleAsync(b => b.Id == id);

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

            // Save all chanages
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
