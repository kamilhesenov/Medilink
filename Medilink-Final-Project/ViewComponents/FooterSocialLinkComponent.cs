using Medilink_Final_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.ViewComponents
{
    public class FooterSocialLinkComponent : ViewComponent
    {
        private readonly AplicationDbContext _context;
        public FooterSocialLinkComponent(AplicationDbContext context)
        {
            _context = context;
        }

        public ViewViewComponentResult Invoke()
        {
            var foterSocialLink = _context.SocialLinks.FirstOrDefault();

            return View(foterSocialLink);
        }
    }
}
