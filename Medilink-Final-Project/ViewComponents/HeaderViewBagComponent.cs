using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.ViewComponents
{
    public class HeaderViewBagComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HeaderViewBagComponent(AplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public async Task<ViewViewComponentResult> Invoke()
        //{
        //    ViewBag.UserName = "";
        //    ViewBag.BasketCount = 0;

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        ViewBag.UserName = user.UserName;

        //        if (Request.Cookies["basket"] != null)
        //        {
        //            List<ShopViewModel> products = JsonConvert.DeserializeObject<List<ShopViewModel>>(Request.Cookies["fbasket"]);

        //            List<ShopViewModel> userProducts = new List<ShopViewModel>();
        //            foreach (var item in products)
        //            {
        //                if(item.UserName == User.Identity.Name)
        //                {
        //                    userProducts.Add(item);
        //                }
        //            }

        //            ViewBag.BasketCount = products.Count;
        //        }
        //    }





        //    return View();
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.BasketCount = 0;

            if (User.Identity.IsAuthenticated)
            {

                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                if (Request.Cookies["basket"] != null)
                {
                    List<BasketViewModel> products = JsonConvert.DeserializeObject<List<BasketViewModel>>(Request.Cookies["basket"]);

                    ViewBag.BasketCount = products.Where(x => x.UserName == User.Identity.Name).Count();
                }

            }
            return View(await Task.FromResult(""));
        }
    }
}
