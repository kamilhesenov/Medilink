using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class ContactViewModel
    {
        public BannerViewModel BannerViewModel { get; set; }
        public Contact.Contact Contact { get; set; }
        public Setting Setting { get; set; }
        public ContactSendViewModel ContactSendViewModel { get; set; }
    }
}
