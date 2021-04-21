using Codesanook.EFNote.Models;
using Codesanook.EFNote.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Codesanook.EFNote.Controllers
{
    public class NotebookController : Controller
    {
        private readonly IRepository<Notebook> notebookRepository;
        private readonly IRepository<Note> noteRepository;

        public NotebookController(
            IRepository<Notebook> notebookRepository,
            IRepository<Note> noteRepository
            )
        {
            this.notebookRepository = notebookRepository;
            this.noteRepository = noteRepository;
        }

        public IActionResult Index()
        {
            var notebooks = notebookRepository.List().OrderBy(b=>b.Name).ToList();
            return View(notebooks);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Notebook model)
        {
            model.Name = model.Name.Trim();
            notebookRepository.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var notebook = notebookRepository.GetById(id);
            return View(notebook);
        }

        [HttpPost]
        public IActionResult Edit(int id, Notebook model)
        {
            model.Name = model.Name.Trim();
            var existingNotebook = notebookRepository.GetById(id);
            existingNotebook.Name = model.Name;
            notebookRepository.Update(existingNotebook);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var notebook = notebookRepository.GetById(id);
            return View(notebook);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection _)
        {
            var notebook = notebookRepository.GetById(id);
            var notes = notebook.Notes;

            var noteAndItsTag = 
                from note in notes
                from tag in note.Tags
                select (note, tag);

            foreach (var (note, tag) in noteAndItsTag.ToList())
            {
                note.Tags.Remove(tag);
            }

            foreach(var note in notebook.Notes.ToList())
            {
                noteRepository.Remove(note);
            }

            notebookRepository.Remove(notebook);
            return RedirectToAction(nameof(Index));
        }
    }
}
