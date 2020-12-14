using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class BasketController : Controller
    {
        
        private readonly AplicationDbContext _context;
        public BasketController(AplicationDbContext context)
        {
            _context = context;
            
        }


        //public async Task<IActionResult> AddBasket(int? id)
        //{
        //    if (id == null) return NotFound();
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        Shop dbproduct = await _context.Shops.FindAsync(id);
        //        if (dbproduct == null) return NotFound();
        //    }

        //    Shop product = await _context.Shops.FindAsync(id);
        //    if (product == null) return NotFound();
        //    List<ShopViewModel> products;
        //    if (Request.Cookies["basket"] == null)
        //    {
        //        products = new List<ShopViewModel>();
        //    }
        //    else
        //    {
        //        products = JsonConvert.DeserializeObject<List<ShopViewModel>>(Request.Cookies["basket"]);
        //    }
        //    ShopViewModel existProduct = products.FirstOrDefault(p => p.Id == id && p.UserName == User.Identity.Name);
        //    if (existProduct == null)
        //    {
        //        ShopViewModel newProduct = new ShopViewModel
        //        {
        //            Id = product.Id,
        //            Count = 1,
        //            UserName = User.Identity.Name
        //        };
        //        products.Add(newProduct);
        //    }
        //    else
        //    {
        //        if(existProduct.Count < product.Count)
        //        {
        //            existProduct.Count++;
        //        }
        //    }

        //    string basket2 = JsonConvert.SerializeObject(products);
        //    Response.Cookies.Append("basket", basket2, new CookieOptions { MaxAge = TimeSpan.FromDays(30) });

        //    if (Request.Headers["X-Request-With"] == "XMLHttpRequest")
        //    {
        //        return RedirectToAction("Basket");
        //    }
        //    return Json(products.Where(p => p.UserName == User.Identity.Name).Count());
        //}


        //public async Task<IActionResult> AddBasket(int? id)
        //{
        //    if (id == null) return NotFound();
        //    Shop dbproduct = await _context.Shops.FindAsync(id);

        //    if (dbproduct == null) return NotFound();
        //    List<ShopViewModel> products;
        //    if (Request.Cookies["fbasket"] == null)
        //    {
        //        products = new List<ShopViewModel>();
        //    }
        //    else
        //    {
        //        products = JsonConvert.DeserializeObject<List<ShopViewModel>>(Request.Cookies["fbasket"]);
        //    }

        //    ShopViewModel existProduct = products.FirstOrDefault(p => p.Id == id);

        //    if (existProduct == null)
        //    {
        //        ShopViewModel newproduct = new ShopViewModel
        //        {
        //            Id = dbproduct.Id,
        //            Count = 1
        //        };
        //        products.Add(newproduct);
        //    }
        //    else
        //    {
        //        existProduct.Count++;
        //    }


        //    string fbasket = JsonConvert.SerializeObject(products);
        //    Response.Cookies.Append("fbasket", fbasket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });

        //    return Json(products.Count());
        //}

        [Route("basket")]
        public IActionResult Index()
        {


            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            double number = 0;
            ViewBag.BasketTotalPrice = "";
            string fbasket = Request.Cookies["basket"];
            List<BasketViewModel> basketProducts = new List<BasketViewModel>();
            List<BasketViewModel> userProducts = new List<BasketViewModel>();

            if (fbasket != null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(fbasket);

                foreach (BasketViewModel basketProduct in basketProducts)
                {
                    if (basketProduct.UserName == User.Identity.Name)
                    {
                        Shop dbProduct = _context.Shops.FirstOrDefault(x => x.Id == basketProduct.Id);
                        if (dbProduct != null)
                        {
                            basketProduct.Price = dbProduct.Price;
                            basketProduct.Photo = dbProduct.Photo;
                            basketProduct.Name = dbProduct.Name;
                            basketProduct.DbCount = dbProduct.Count;
                            userProducts.Add(basketProduct);
                        }
                        basketProduct.ProductTotalPrice = basketProduct.BasketCount * basketProduct.Price;
                        number += basketProduct.ProductTotalPrice;
                    }
                }
                ViewBag.BasketTotalPrice = number;
            }
            return View(userProducts);

        }

        public IActionResult AddBasket(int? id)
        {
            double basketTotalPrice = 0;

            if (id == null) return RedirectToAction("Index", "Error404");

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Shop dbproduct = _context.Shops.FirstOrDefault(x => x.Id == id);

            if (dbproduct == null) return RedirectToAction("Index", "Error404");
            List<BasketViewModel> basketProducts;
            if (Request.Cookies["basket"] == null)
            {
                basketProducts = new List<BasketViewModel>();
            }
            else
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);
            }

            BasketViewModel existProduct = basketProducts.FirstOrDefault(p => p.Id == id && p.UserName == User.Identity.Name);

            if (existProduct == null)
            {
                BasketViewModel newproduct = new BasketViewModel
                {
                    Id = dbproduct.Id,
                    BasketCount = 1,
                    UserName = User.Identity.Name
                };
                basketProducts.Add(newproduct);
            }
            else
            {
                existProduct.BasketCount++;
            }

            foreach (var basketProduct in basketProducts.Where(x => x.UserName == User.Identity.Name))
            {
                Shop dbProduct = _context.Shops.FirstOrDefault(x => x.Id == basketProduct.Id);
                if (dbProduct != null)
                {
                    basketProduct.Price = dbProduct.Price;
                    basketProduct.Photo = dbProduct.Photo;
                    basketProduct.Name = dbProduct.Name;
                    basketProduct.DbCount = dbProduct.Count;
                }
                basketProduct.ProductTotalPrice = basketProduct.BasketCount * basketProduct.Price;
                basketTotalPrice += basketProduct.ProductTotalPrice;
            }

            string fbasket = JsonConvert.SerializeObject(basketProducts);
            Response.Cookies.Append("basket", fbasket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            var anonymObject = new
            {
                BasketTotalPrice = basketTotalPrice,
                BasketProductCount = basketProducts.Where(p => p.UserName == User.Identity.Name).Count()
            };

            return Ok(anonymObject);
            
        }


        public IActionResult ProductCountPlus([FromForm]  int id)
        {
            int basketProductDbCount = _context.Shops.FirstOrDefault(x=>x.Id == id).Count;
            double basketTotalPrice = 0;
            double productTotalPrice = 0;
            string basket = Request.Cookies["basket"];
            List<BasketViewModel> basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            BasketViewModel product = basketProducts.FirstOrDefault(p => p.Id == id && p.UserName == User.Identity.Name);

            product.BasketCount++;
            int basketCount = product.BasketCount;
            foreach (var basketProduct in basketProducts.Where(x => x.UserName == User.Identity.Name))
            {
                Shop dbProduct = _context.Shops.FirstOrDefault(x => x.Id == basketProduct.Id);
                if (dbProduct != null)
                {
                    basketProduct.Price = dbProduct.Price;
                    basketProduct.Photo = dbProduct.Photo;
                    basketProduct.Name = dbProduct.Name;
                    basketProduct.DbCount = dbProduct.Count;
                }
                basketProduct.ProductTotalPrice = basketProduct.BasketCount * basketProduct.Price;
                if (basketProduct.Id == id)
                {
                    productTotalPrice = basketProduct.ProductTotalPrice;
                }
                basketTotalPrice += basketProduct.ProductTotalPrice;
            }

            string fbasket = JsonConvert.SerializeObject(basketProducts);
            Response.Cookies.Append("basket", fbasket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            var anonymObject = new
            {
                BasketProducts = basketProducts,
                ProductBasketCount = basketCount,
                BasketTotalPrice = basketTotalPrice,
                ProductTotalPrice = productTotalPrice,
                BasketProductDbCount = basketProductDbCount,
                ProductId = product.Id
            };
            return Ok(anonymObject);
        }

        public IActionResult ProductCountMinus(int? id)
        {
            double basketTotalPrice = 0;
            double productTotalPrice = 0;
            string basket = Request.Cookies["basket"];
            List<BasketViewModel> basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            BasketViewModel product = basketProducts.FirstOrDefault(p => p.Id == id);

            if (product.BasketCount > 1)
            {
                product.BasketCount--;
            }
            else
            {
                basketProducts.Remove(product);
            }

            int basketCount = product.BasketCount;
            foreach (var basketProduct in basketProducts.Where(x => x.UserName == User.Identity.Name))
            {
                Shop dbProduct = _context.Shops.FirstOrDefault(x => x.Id == basketProduct.Id);
                if (dbProduct != null)
                {
                    basketProduct.Price = dbProduct.Price;
                    basketProduct.Photo = dbProduct.Photo;
                    basketProduct.Name = dbProduct.Name;
                    basketProduct.DbCount = dbProduct.Count;
                }
                basketProduct.ProductTotalPrice = basketProduct.BasketCount * basketProduct.Price;
                if (basketProduct.Id == id)
                {
                    productTotalPrice = basketProduct.ProductTotalPrice;
                }
                basketTotalPrice += basketProduct.ProductTotalPrice;
            }

            string fbasket = JsonConvert.SerializeObject(basketProducts);
            Response.Cookies.Append("basket", fbasket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            var anonymObject = new
            {
                BasketProducts = basketProducts,
                ProductBasketCount = basketCount,
                BasketTotalPrice = basketTotalPrice,
                ProductTotalPrice = productTotalPrice
            };
            return Ok(anonymObject);
        }

        public IActionResult RemoveItem(int? id)
        {
            double basketTotalPrice = 0;
            string basket = Request.Cookies["basket"];
            List<BasketViewModel> basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            BasketViewModel product = basketProducts.FirstOrDefault(p => p.Id == id);

            basketProducts.Remove(product);

            foreach (var basketProduct in basketProducts.Where(x => x.UserName == User.Identity.Name))
            {
                Shop dbProduct = _context.Shops.FirstOrDefault(x => x.Id == basketProduct.Id);
                if (dbProduct != null)
                {
                    basketProduct.Price = dbProduct.Price;
                    basketProduct.Photo = dbProduct.Photo;
                    basketProduct.Name = dbProduct.Name;
                    basketProduct.DbCount = dbProduct.Count;
                }
                basketProduct.ProductTotalPrice = basketProduct.BasketCount * basketProduct.Price;
                basketTotalPrice += basketProduct.ProductTotalPrice;
            }

            string fbasket = JsonConvert.SerializeObject(basketProducts);
            Response.Cookies.Append("basket", fbasket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            var anonymObject = new
            {
                BasketTotalPrice = basketTotalPrice,
                BasketProductCount = basketProducts.Where(x => x.UserName == User.Identity.Name).Count()
            };
            return Ok(anonymObject);
        }
    }
}
