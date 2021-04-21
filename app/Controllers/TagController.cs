using Codesanook.EFNote.Models;
using Codesanook.EFNote.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

namespace Codesanook.EFNote.Controllers
{
    public class TagController : Controller
    {
        private readonly IRepository<Tag> tagRepository;

        public TagController(IRepository<Tag> tagRepository) => this.tagRepository = tagRepository;

        public IActionResult Index()
        {
            var tags = tagRepository.List().OrderBy(t => t.Name).ToList();
            return View(tags);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Tag model)
        {
            model.Name = FormatTagName(model.Name);
            tagRepository.Add(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var tags = tagRepository.GetById(id);
            return View(tags);
        }

        [HttpPost]
        public IActionResult Edit(int id, Tag model)
        {
            var existingTag = tagRepository.GetById(id);
            existingTag.Name = FormatTagName(model.Name);
            tagRepository.Update(existingTag);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var tag = tagRepository.GetById(id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(int id, IFormCollection formCollection)
        {
            var tag = tagRepository.GetById(id);
            foreach (var note in tag.Notes.ToList())
            {
                tag.Notes.Remove(note);// Remove relationship but note remove actual note from a database
            }

            tagRepository.Remove(tag);
            return RedirectToAction(nameof(Index));
        }

        private static string FormatTagName(string tagName) =>
            Regex.Replace(tagName.Trim(), @"\s+", "-").ToLower();
    }
}
