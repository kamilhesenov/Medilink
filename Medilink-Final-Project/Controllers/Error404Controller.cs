using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class Error404Controller : Controller
    {
        public IActionResult Index()
        {
            ErrorViewModel model = new ErrorViewModel
            {
                BannerViewModel = new BannerViewModel
                {
                    Title = "Error 404"
                }
            };

            return View(model);
        }
    }
}
