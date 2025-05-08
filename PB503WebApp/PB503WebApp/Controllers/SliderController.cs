using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503WebApp.Data;
using PB503WebApp.Models;
using PB503WebApp.ViewModels.Slider;
using SmartMapValidator;
using System.Collections;

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

        public async Task<IActionResult> Index(int page = 1, int count = 3)
        {

         var query =  _context.Sliders.AsQueryable();


//ViewBag.TotalItems=query.Count();

//ViewBag.TotalPage =Math.Ceiling((double)query.Count()/count);

// var slider=await query.ToListAsync();



int skip = (page - 1) * count;
var data = await _context.Sliders.ToListAsync();
var sliders = data.Skip(skip).Take(count).ToList();

var datas = sliders.Select(m => new SliderVM
{
    ImageUrl = m.ImageUrl,
    Description = m.Description,
    Title = m.Title,
}).ToList();




return View(datas);
        }

        public async Task<IActionResult>GetPaginateDatas(int page=1,int count=3)
        {



            return Redirect($"/slider/Index?page={page}&{count}");
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
            if (model.Image.Length >50 * 1024)
            {
                ModelState.AddModelError("Image", " şəkil faylı 50KB kicik olmalidir.");
                return View(model);
            }


            string fileName = Guid.NewGuid().ToString() + model.Image.FileName; //+ Path.GetExtension(model.Image.FileName);
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "sliders");

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
        public async Task<IActionResult> Delete(int? id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            
            if (slider == null)
            {
                return Json(new { success = false });
            }
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "uploads", "sliders", slider.ImageUrl);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slider =await _context.Sliders.FirstOrDefaultAsync(m=>m.Id==id);

            var datas = SmartMap.Map<Slider, SliderEditVM>(slider);
            return View(datas);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SliderEditVM request)
        {
           
            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
           
            request.ImageUrl=slider.ImageUrl;
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            if (request.NewImage is null)
            {
                slider.Title = request.Title;
                slider.Description = request.Description;

                _context.Update(slider);
                await _context.SaveChangesAsync();

               return Redirect("/slider/index");
            }
            else
            {
                string oldImagePath = Path.Combine(_env.WebRootPath, "uploads", "sliders", slider.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                if (!request.NewImage.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("NewImage", "Yalnız şəkil faylı yükləyə bilərsiniz.");
                    return View(request);
                }
                if (request.NewImage.Length > 50 * 1024)
                {
                    ModelState.AddModelError("NewImage", " şəkil faylı 50KB kicik olmalidir.");
                    return View(request);
                }
                string fileName = Guid.NewGuid().ToString() + request.NewImage.FileName;
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "sliders");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }

                slider.Title = request.Title;
                slider.Description = request.Description;

                slider.ImageUrl = fileName;
                _context.Update(slider);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
           
           

           
          
            
        }
    }


}
