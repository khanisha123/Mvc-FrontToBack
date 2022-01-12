using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class BasketController : Controller
    {
        public readonly Context _context;
        public BasketController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Remove() {
            
           
            Product product = new Product();
            int productId = product.Id;
            Product product1 = _context.Products.Find(productId);
            int empty;
            if (product1 !=null)
            {
                Remove(product1);
            }

            return View();
            
            
        }
        public async Task<IActionResult> AddBasket(int? id) {
            if (id == null) return RedirectToAction("Index", "Eror");
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return RedirectToAction("Index", "Eror");
            string basket = Request.Cookies["basket"];
            List<BasketProduct> basketProductsList;
            if (basket == null)
            {
                basketProductsList = new List<BasketProduct>();
            }
            else
            {
                basketProductsList = JsonConvert.DeserializeObject<List<BasketProduct>>(basket);
            }
            BasketProduct basketProduct = new BasketProduct {

                Id = product.Id,
                
                Count = 1
            };
            BasketProduct isExistProduct = basketProductsList.FirstOrDefault(p => p.Id == product.Id);
             
            if (isExistProduct==null)
            {
                BasketProduct basketProducts = new BasketProduct
                {

                    Id = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Count = 1
                };
                basketProductsList.Add(basketProducts);
            }
            else
            {
                isExistProduct.Count++;
            }
            
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProductsList), new CookieOptions { MaxAge = TimeSpan.FromMinutes(14) });               
            return RedirectToAction("Index","Home");
        }
        public IActionResult ShowBasket() {

            string basket = Request.Cookies["basket"];
            List<BasketProduct> products=new List<BasketProduct>();
            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketProduct>>(basket);
                foreach (var item in products)
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                    item.Price = product.Price;
                    item.ImageUrl = product.ImageUrl;
                    item.Name = product.Name;
                }
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(14) });
            }
            return View(products);
        }
    }
}
