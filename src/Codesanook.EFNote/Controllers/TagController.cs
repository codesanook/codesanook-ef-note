using Codesanook.EFNote.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codesanook.EFNote.Controllers
{
    public class TagController : Controller
    {
        private readonly NoteDbContext dbContext;

        public TagController(NoteDbContext dbContext) => this.dbContext = dbContext;

        public async Task<IActionResult> IndexAsync()
        {
            var tags = await dbContext.Tags
                .Include(t => t.Notes)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(tags);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Tag tag)
        {
            tag.Name = GetFormatTagName(tag.Name);
            await dbContext.Tags.AddAsync(tag);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            var tags = await dbContext.Tags.FindAsync(id);
            return View(tags);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, Tag model)
        {
            var existingTag = await dbContext.Tags.FindAsync(id);
            existingTag.Name = GetFormatTagName(model.Name);

            var newTagName = GetFormatTagName(model.Name);
            await dbContext.Tags
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(t => t.Name, _ => newTagName)
                );

            await dbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tag = await dbContext.Tags
                .Include(t => t.Notes)
                .SingleAsync(t => t.Id == id);

            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id, IFormCollection formCollection)
        {
            var tag = await dbContext.Tags
                .Include(t => t.Notes)
                .SingleAsync(t => t.Id == id);

            foreach (var note in tag.Notes)
            {
                tag.Notes.Remove(note);// Remove relationship but note remove actual note from a database
            }

            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private static string GetFormatTagName(string tagName) =>
            Regex.Replace(tagName.Trim(), @"\s+", "-").ToLower();
    }
}
