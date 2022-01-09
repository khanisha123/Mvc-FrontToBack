﻿using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly Context _context;
        public ProductController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Count();
            List<Product> products = _context.Products.Include(p=>p.Category).Take(8).ToList();
            return View(products);
        }
        //public IActionResult LoadMore() {

        //    return Json(_context.Products.Select(p=> new ProductReturn { 
        //    Id=p.Id,
        //    Name=p.Name,
        //    Price=p.Price,
        //    ImageUrl=p.ImageUrl,
        //    Category=p.Category.Name
        //    }).Include(c => c.Category).Take(8).ToList());
        //}

        public IActionResult LoadMore(int skip) {
            ViewBag.ProductCount=_context.Products.Count();
            return Json(_context.Products.Skip(8).Take(8).ToList());

        }
    }
}
