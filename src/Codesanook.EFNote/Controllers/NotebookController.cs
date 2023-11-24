using Codesanook.EFNote.Models;
using Codesanook.EFNote.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codesanook.EFNote.Controllers;

public class NotebookController : Controller
{
    private NoteDbContext dbContext;
    public NotebookController(NoteDbContext dbContext) => this.dbContext = dbContext;

    public async Task<IActionResult> IndexAsync()
    {
        var notebook = await dbContext.Notebooks
            .Where(n => n.Settings.ColorTheme == "#F00")
            .ToListAsync();

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
        // ExecuteUpdate fails when target entity has an owned entity #28727
        // https://github.com/dotnet/efcore/issues/28727
        var existingNotebook = await dbContext.Notebooks.FindAsync(id);
        existingNotebook.Name = notebook.Name.Trim();

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

    [Route("/add-data")]
    public async Task<IActionResult> AddData()
    {
        var settings = new Settings()
        {
            ColorTheme = "Blue",
            SupportedFileFormats = new List<SupportedFileFormat>{
                new SupportedFileFormat(){
                    FileFormat = "Plaintext",
                    Editor = "TextEditor"
                }
            }
        };

        var notebook = new Notebook()
        {
            Name = "EF Core",
            Settings = settings,
        };
        await dbContext.Notebooks.AddAsync(notebook);

        var tag = new Tag()
        {
            Name = "ef"
        };
        await dbContext.Tags.AddAsync(tag);

        var note = new Note()
        {
            Title = "What's new in EF Core 8",
            Content = "Here are the content",
            Notebook = notebook,
            Tags = new List<Tag>() { tag },
            UtcUpdates = new List<DateTime>(){
                DateTime.UtcNow.AddDays(-5),
                DateTime.UtcNow,
            }
        };
        await dbContext.Notes.AddAsync(note);

        await dbContext.SaveChangesAsync();

        return Json("done");
    }


    [Route("/access-data")]
    public async Task<IActionResult> AccessData()
    {
        var notebook = await dbContext.Notebooks
            .FirstOrDefaultAsync();

        return Json(notebook.Settings.ColorTheme);
    }

    [Route("/simple-filter")]
    public async Task<IActionResult> SimpleFilter()
    {
        var notebook = await dbContext.Notebooks
            .Where(n => n.Settings.ColorTheme == "Blue")
            .ToListAsync();

        return Json(notebook);
    }

    [Route("/complex-filter")]
    public async Task<IActionResult> ComplexFilter()
    {
        var notebook = await dbContext.Notebooks
            .Where(
                n => n.Settings.SupportedFileFormats.Any(f => f.FileFormat == "Plaintext")
            )
            .ToListAsync();

        return Json(notebook);
    }

    [Route("/projection")]
    public async Task<IActionResult> Projection()
    {
        var notebookWithSupportFileFormat = await dbContext.Notebooks
            .AsNoTracking()
            .Where(
                n => n.Settings.SupportedFileFormats.Any(f => f.FileFormat == "Plaintext")
            )
            .Select(n => new
            {
                n.Name,
                SupportedFileFormat = n.Settings.SupportedFileFormats.First(f => f.FileFormat == "Plaintext")
            })
            .ToListAsync();

        return Json(notebookWithSupportFileFormat);
    }

    [Route("/primitive-collections")]
    public async Task<IActionResult> PrimitiveCollections()
    {
        var notes = await dbContext.Notes
            .Where(n => n.UtcUpdates.Count() >= 2)
            .ToListAsync();

        return Json(notes);
    }

    [Route("/primitive-function")]
    public async Task<IActionResult> PrimitiveFunction()
    {
        var from = 2022;
        var to = 2023;
        var notes = await dbContext.Notes
            .Where(n => n.UtcUpdates.Any(u => u.Year >= from && u.Year <= to))
            .ToListAsync();

        return Json(notes);
    }

    [Route("/in-parameter")]
    public async Task<IActionResult> InParameter()
    {
        var tags = new[] { "ef", "c#", "java", "sql", "ts" };
        var notes = await dbContext.Tags
            .Where(t => tags.Contains(t.Name))
            .ToListAsync();

        return Json(notes);
    }

}
