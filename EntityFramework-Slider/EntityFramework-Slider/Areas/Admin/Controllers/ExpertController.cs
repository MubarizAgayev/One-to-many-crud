using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExpertController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ExpertController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Expert> experts = await _context.Experts.ToListAsync();
            return View(experts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expert expert)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!expert.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type must be image");
                return View();
            }

            if (!expert.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size must be max 200kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + expert.Photo.FileName;

            string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new(path, FileMode.Create))
            {
                await expert.Photo.CopyToAsync(stream);
            }
            expert.Image = fileName;

            await _context.Experts.AddAsync(expert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Expert expert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);

            if (expert is null) return NotFound();

            string path = FileHelper.GetFilePath(_env.WebRootPath, "img", expert.Image);

            FileHelper.DeleteFile(path);

            _context.Experts.Remove(expert);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            Expert expert = await _context.Experts.FirstOrDefaultAsync(m => m.Id == id);

            if (expert is null) return NotFound();

            return View(expert);
        }
    }
}
