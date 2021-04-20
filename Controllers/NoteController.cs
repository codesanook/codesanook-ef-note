using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Codesanook.EFNote.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Codesanook.EFNote.Repositories;
using Codesanook.EFNote.Models;
using Microsoft.EntityFrameworkCore;

namespace Codesanook.EFNote.Controllers
{
    public class NoteController : Controller
    {
        private readonly IRepository<Note> noteRepository;
        private readonly IRepository<Notebook> notebookRepository;
        private readonly IRepository<Tag> tagRepository;

        public NoteController(
            IRepository<Note> noteRepository,
            IRepository<Notebook> notebookRepository,
            IRepository<Tag> tagRepository
        )
        {
            this.noteRepository = noteRepository;
            this.notebookRepository = notebookRepository;
            this.tagRepository = tagRepository;
        }

        public const string ErrorMessageKey = "errorMessage";
        public IActionResult Index(int? selectedNotebookId, int? selectedNoteId)
        {
            var allNotebooks = this.notebookRepository.List().ToList();
            var selectedNotebook = allNotebooks.SingleOrDefault(b => b.Id == selectedNotebookId);

            var allNotesForSelectedNotebook = noteRepository.List()
                .Where(n => n.Notebook.Id == selectedNotebookId)
                .Include(n => n.Tags)
                .AsEnumerable()
                .Select(n => new NoteViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    NotebookId = n.NotebookId,
                    Tags = string.Join(", ", n.Tags.Select(t => t.Name)),
                })
                .ToList();

            var selectedNote = allNotesForSelectedNotebook.SingleOrDefault(n => n.Id == selectedNoteId);
            var model = new NoteIndexViewModel()
            {
                AllNotebooks = allNotebooks,
                SelectedNotebook = selectedNotebook,
                AllNotesForSelectedNotebook = allNotesForSelectedNotebook,
                SelectedNote = selectedNote
            };
            return View(model);
        }

        public IActionResult Create(int? selectedNotebookId)
        {
            if (selectedNotebookId == null)
            {
                TempData[nameof(ErrorMessageKey)] = "Notebook not selected. If not exist, please create new and select";
                return RedirectToAction(nameof(Index));
            }

            var allNotebooks = notebookRepository.List().ToList();
            var selectedNotebook = allNotebooks.SingleOrDefault(b => b.Id == selectedNotebookId);

            var allNotesForSelectedNotebook = noteRepository.List()
                .Where(n => n.Notebook.Id == selectedNotebookId)
                .Include(n => n.Tags)
                .AsEnumerable()
                .Select(n => new NoteViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    NotebookId = n.NotebookId,
                    Tags = string.Join(", ", n.Tags.Select(t => t.Name)),
                })
                .ToList();

            var model = new NoteIndexViewModel()
            {
                AllNotebooks = allNotebooks,
                SelectedNotebook = selectedNotebook,
                AllNotesForSelectedNotebook = allNotesForSelectedNotebook,
            };
            return View(nameof(Index), model);
        }

        [HttpPost]
        public IActionResult Create([Bind(Prefix = "SelectedNote")] NoteViewModel viewModel)
        {
            var allTags = GetAllTags(viewModel.Tags);
            var note = new Note()
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                NotebookId = viewModel.NotebookId,
                Tags = allTags
            };

            noteRepository.Add(note);
            return RedirectToIndex(note);
        }

        public IActionResult Edit(int id, [Bind(Prefix = "SelectedNote")] NoteViewModel viewModel)
        {
            var allTags = GetAllTags(viewModel.Tags);
            var existingNote = noteRepository.GetById(id);
            var newTagsToAdd = allTags.Except(existingNote.Tags, new TagComparer()).ToList();
            var existingTagsToRemove = existingNote.Tags.Except(allTags, new TagComparer()).ToList();
            foreach (var tag in newTagsToAdd)
            {
                existingNote.Tags.Add(tag);
            }

            foreach (var tag in existingTagsToRemove)
            {
                existingNote.Tags.Remove(tag);
            }

            existingNote.Title = viewModel.Title;
            existingNote.Content = viewModel.Content;
            existingNote.NotebookId = viewModel.NotebookId;//update notebook
            noteRepository.Update(existingNote);
            return RedirectToIndex(existingNote);
        }

        private IList<Tag> GetAllTags(string tagNamesValue)
        {
            var tagNames = string.IsNullOrWhiteSpace(tagNamesValue)
                ? Array.Empty<string>()
                : tagNamesValue.ToLower()?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());

            var existingTags = tagRepository.List().Where(t => tagNames.Contains(t.Name)).ToList();
            var newTags = tagNames.Select(t => new Tag() { Name = t }).Except(existingTags, new TagComparer());
            var allTags = existingTags.Concat(newTags).ToList();
            return allTags;
        }

        private IActionResult RedirectToIndex(Note note)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    selectedNotebookId = note.NotebookId,
                    selectedNoteId = note.Id
                });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public IActionResult Delete(int id)
        {
            var note = noteRepository.GetById(id);
            return View(note);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection _)
        {
            var note = noteRepository.GetById(id);
            foreach (var tag in note.Tags.ToList())
            {
                note.Tags.Remove(tag);
            }

            noteRepository.Remove(note);
            return RedirectToAction(nameof(Index));
        }
    }
}
