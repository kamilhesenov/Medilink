using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class ContactSendViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(600)]
        public string Message { get; set; }
    }
}
