using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503WebApp.Data;
using PB503WebApp.Models;
using PB503WebApp.ViewModels.Slider;

namespace PB503WebApp.Controllers
{

    public class SliderController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public SliderController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var slider=await _context.Sliders.ToListAsync();
            return View(slider);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.Image.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("Image", "Yalnız şəkil faylı yükləyə bilərsiniz.");
                return View(model);
            }
            if (model.Image.Length>50*1024)
            {
                ModelState.AddModelError("Image", " şəkil faylı 50KB kicik olmalidir.");
                return View(model);
            }


            string fileName = Guid.NewGuid().ToString() + model.Image.FileName; //+ Path.GetExtension(model.Image.FileName);
            string folderPath = Path.Combine(_env.WebRootPath,"uploads","sliders");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }


            Slider slider = new Slider
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = fileName
            };

            
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<bool> Delete(int? id)
        {
            var slider= await _context.Sliders.FirstOrDefaultAsync(m=>m.Id==id);
            if (slider == null)
            {
                return false;
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return true; 
        }
    }


}
