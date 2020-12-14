using Medilink_Final_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.ViewComponents
{
    public class HeaderSettingComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;
        public HeaderSettingComponent(AplicationDbContext context)
        {
            _context = context;
        }

        public ViewViewComponentResult Invoke()
        {
            var headerSetting = _context.Settings.FirstOrDefault();

            return View(headerSetting);
        }
    }
}
