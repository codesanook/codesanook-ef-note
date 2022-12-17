using Microsoft.AspNetCore.Mvc;
using Codesanook.EFNote.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Codesanook.EFNote.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Codesanook.EFNote.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteDbContext dbContext;
        public const string ErrorMessageKey = "errorMessage";

        public NoteController(NoteDbContext dbContext) => this.dbContext = dbContext;

        public async Task<IActionResult> IndexAsync(int? selectedNotebookId, int? selectedNoteId)
        {
            var allNotebooks = await dbContext.Notebooks.ToListAsync();
            var selectedNotebook = allNotebooks.SingleOrDefault(b => b.Id == selectedNotebookId);

            var noteList = await dbContext.Notes
                .Where(n => n.Notebook.Id == selectedNotebookId)
                .Include(n => n.Tags) // Eger loading
                .ToListAsync();

            //.AsEnumerable() vs to list
            var allNotesForSelectedNotebook = noteList.Select(
                n => new NoteViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    NotebookId = n.NotebookId,
                    Tags = string.Join(", ", n.Tags.Select(t => t.Name)),
                }
            ).ToList();

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

        public async Task<IActionResult> CreateAsync(int? selectedNotebookId)
        {
            if (selectedNotebookId == null)
            {
                TempData[nameof(ErrorMessageKey)] = "Notebook not selected. If not exist, please create new and select";
                return RedirectToAction("Index");
            }

            var allNotebooks = await dbContext.Notebooks.ToListAsync();
            var selectedNotebook = allNotebooks.SingleOrDefault(b => b.Id == selectedNotebookId);

            var noteList = await dbContext.Notes
                .Where(n => n.Notebook.Id == selectedNotebookId)
                .Include(n => n.Tags)
                .ToListAsync();

            var allNotesForSelectedNotebook = noteList.Select(
                n => new NoteViewModel()
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
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([Bind(Prefix = "SelectedNote")] NoteViewModel viewModel)
        {
            if (viewModel.NotebookId == 0)
            {
                return View("Index");
            }

            var allTags = await GetAllTagsAsync(viewModel.Tags);
            var note = new Note()
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                NotebookId = viewModel.NotebookId,
                CreatedUtc = DateTime.UtcNow,
                Tags = allTags
            };

            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();
            return RedirectToIndex(note);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, [Bind(Prefix = "SelectedNote")] NoteViewModel viewModel)
        {
            var existingNote = await dbContext.Notes
                .Include(n => n.Tags)
                .SingleAsync(n => n.Id == id);

            var existingTags = existingNote.Tags.ToList();

            var allTagsFromInput = await GetAllTagsAsync(viewModel.Tags);
            var newTagsToAdd = allTagsFromInput.Except(existingTags, new TagComparer()).ToList();
            var existingTagsToRemove = existingTags.Except(allTagsFromInput, new TagComparer()).ToList();

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
            existingNote.NotebookId = viewModel.NotebookId; // Update the notebook of the current note

            await dbContext.SaveChangesAsync();
            return RedirectToIndex(existingNote);
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var note = await dbContext.Notes.FindAsync(id);
            return View(note);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id, IFormCollection _)
        {
            // ExecuteDeleteAsync
            // var note = await dbContext.Notes.SingleAsync(n => n.Id == id);
            // dbContext.Notes.Remove(note);

            await dbContext.Notes.Where(n => n.Id == id).ExecuteDeleteAsync();

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private IActionResult RedirectToIndex(Note note)
        {
            return RedirectToAction(
                "Index",
                new
                {
                    selectedNotebookId = note.NotebookId,
                    selectedNoteId = note.Id
                });
        }

        private async Task<List<Tag>> GetAllTagsAsync(string tagNamesValue)
        {
            var tagNames = string.IsNullOrWhiteSpace(tagNamesValue)
                ? Array.Empty<string>()
                : tagNamesValue.ToLower()?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());

            var existingTags = await dbContext.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync();

            var newTags = tagNames.Select(t => new Tag() { Name = t }).Except(existingTags, new TagComparer());
            var allTags = existingTags.Concat(newTags).ToList();

            return allTags;
        }

    }
}
