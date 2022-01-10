using FrontToBack.DAL;
using FrontToBack.HomeModels;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            SliderDesc slider = _context.SliderDescs.FirstOrDefault();
            List<Category> categories = _context.Categories.ToList();
            About_Video about_Videos = _context.About_Videos.FirstOrDefault();
            HomeVm homeVm = new HomeVm();
            homeVm.Sliders = sliders;
            homeVm.SliderDesc = slider;
            homeVm.Categories = categories;
            homeVm.About_Videos = about_Videos;
            return View(homeVm);
        }
    }
}
