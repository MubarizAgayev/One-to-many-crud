using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderInfoController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<SliderInfo> sliderInfos = await _context.SliderInfos.ToListAsync();
            return View(sliderInfos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderInfo sliderInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!sliderInfo.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!sliderInfo.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "_" + sliderInfo.Photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);



                await FileHelper.SaveFileAsync(path, sliderInfo.Photo);

                sliderInfo.SignatureImage = fileName;

                await _context.SliderInfos.AddAsync(sliderInfo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo is null) return NotFound();

            string path = FileHelper.GetFilePath(_env.WebRootPath, "img", sliderInfo.SignatureImage);

            FileHelper.DeleteFile(path);

            _context.SliderInfos.Remove(sliderInfo);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo is null) return NotFound();

            return View(sliderInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderInfo sliderInfo)
        {
            if (id is null) return BadRequest();


            SliderInfo dbSliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(m => m.Id == id);

            if (dbSliderInfo is null) return NotFound();

            if (!sliderInfo.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type must be image");
                return View(dbSliderInfo);
            }

            if (!sliderInfo.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size must be max 200kb");
                return View(dbSliderInfo);
            }

            string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbSliderInfo.SignatureImage);

            FileHelper.DeleteFile(oldPath);

            string fileName = Guid.NewGuid().ToString() + "_" + sliderInfo.Photo.FileName;

            string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);


            await FileHelper.SaveFileAsync(path, sliderInfo.Photo);

            dbSliderInfo.SignatureImage = fileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfos.FindAsync(id);
            if (sliderInfo is null) return NotFound();



            return View(sliderInfo);
        }
    }
}
