using Codesanook.EFNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

namespace Codesanook.EFNote.Controllers
{
    public class TagController : Controller
    {
        private NoteDbContext dbContext;

        public TagController(NoteDbContext dbContext) => this.dbContext = dbContext;

        public IActionResult Index()
        {
            var tags = dbContext.Tags
                .Include(t => t.Notes)
                .OrderBy(t => t.Name)
                .ToList();

            return View(tags);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            tag.Name = FormatTagName(tag.Name);
            dbContext.Tags.Add(tag);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var tags = dbContext.Tags.Find(id);
            return View(tags);
        }

        [HttpPost]
        public IActionResult Edit(int id, Tag model)
        {
            var existingTag = dbContext.Tags.Find(id);
            existingTag.Name = FormatTagName(model.Name);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var tag = dbContext.Tags
                .Include(t => t.Notes)
                .Single(t => t.Id == id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection formCollection)
        {
            var tag = dbContext.Tags
                .Include(t => t.Notes)
                .Single(t => t.Id == id);
            foreach (var note in tag.Notes)
            {
                tag.Notes.Remove(note);// Remove relationship but note remove actual note from a database
            }

            dbContext.Tags.Remove(tag);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private static string FormatTagName(string tagName) => Regex.Replace(tagName.Trim(), @"\s+", "-").ToLower();
    }
}
