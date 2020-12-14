using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class ChekoutController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChekoutController(AplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            CheckoutViewModel model = new CheckoutViewModel
            {
                BannerViewModel = new BannerViewModel
                {
                    Title = "Checkout"
                },
                Checkout = _context.Checkouts.FirstOrDefault()
            };
            return View(model);
        }

        
        //public async Task<IActionResult> Send(CheckoutViewModel model)
        //{
        //    if (!ModelState.IsValid) return BadRequest();

        //    Checkout checkout = new Checkout
        //    {
        //        FullName = model.FullName,
        //        Address = model.Address,
        //        Email = model.Email,
        //        Phone = model.Phone
        //    };

        //    await _context.AddAsync(checkout);
        //    await _context.SaveChangesAsync();

        //    Response.Cookies.Delete("basket");

        //    string basketProduct = Request.Cookies["basket"];
        //    List<BasketViewModel> products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketProduct);
        //    foreach (var item in products)
        //    {
        //        Shop shop = _context.Shops.Where(x => x.Id == item.Id).FirstOrDefault();
        //        shop.Count--;
        //        _context.Update(shop);

        //    }

        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", "Home");

        //}

        [HttpPost]
        public async Task<IActionResult> Send(CheckoutViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            string basketProduct = Request.Cookies["basket"];
            //List<BasketViewModel> products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketProduct);
            //foreach (var item in products)
            //{
            //    Shop shop = _context.Shops.Where(x => x.Id == item.Id).FirstOrDefault();
            //    shop.Count--;
            //    _context.Update(shop);

            //}

            //await _context.SaveChangesAsync();



            Checkout sale = new Checkout
                {
                    Date = DateTime.Now,
                    FullName = model.FullName,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone

            };

                List<BasketViewModel> basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);

                List<Shop> dbProducts = new List<Shop>();
                foreach (BasketViewModel item in basketProducts)
                {
                    Shop dbProduct = await _context.Shops.FindAsync(item.Id);
                    if (dbProduct.Count < item.BasketCount)
                    {
                        TempData["error"] = $"{dbProduct.Name} mehsulundan {dbProduct.Count} eded qalib,zehmet olmasa duzelish edin!!!";
                        return RedirectToAction("Basket");
                    }
                    dbProducts.Add(dbProduct);
                }

                List<ShopCkeckout> saleProducts = new List<ShopCkeckout>();

                double total = 0;
                foreach (BasketViewModel pro in basketProducts)
                {
                    Shop dbProduct = dbProducts.Find(p => p.Id == pro.Id);

                    await DecreaseProductCount(dbProduct, pro);

                    ShopCkeckout saleProduct = new ShopCkeckout
                    {
                        Price = dbProduct.Price,
                        Count = pro.BasketCount,
                        ShopId = pro.Id,
                        CheckoutId = sale.Id
                    };
                    total += pro.BasketCount * dbProduct.Price;
                    saleProducts.Add(saleProduct);
                }
                sale.Total = total;
                sale.ShopCkeckouts = saleProducts;

                await _context.Checkouts.AddAsync(sale);
                await _context.SaveChangesAsync();
                TempData["success"] = "Alish-verishiniz ugurla yerine yetirildi";

            Response.Cookies.Delete("basket");

            return RedirectToAction("Index", "Home");




        }

        private async Task DecreaseProductCount(Shop dbProduct, BasketViewModel basketPro)
        {
            dbProduct.Count = dbProduct.Count - basketPro.BasketCount;
            await _context.SaveChangesAsync();
        }
    }
}
