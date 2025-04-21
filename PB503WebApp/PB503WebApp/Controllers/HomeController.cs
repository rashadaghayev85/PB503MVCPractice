using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503WebApp.Data;
using PB503WebApp.Models;
using PB503WebApp.ViewModels;
using PB503WebApp.ViewModels.Slider;
using System.Diagnostics;

namespace PB503WebApp.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
           _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.ToListAsync();

            var mappedSlider = sliders.Select(m => new SliderVM
            {
                Title = m.Title,
                Description = m.Description,
                ImageUrl = m.ImageUrl,
            });

            var homeVM = new HomeVM
            {
                Sliders = mappedSlider.ToList(),
            };
            return View(homeVM);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
