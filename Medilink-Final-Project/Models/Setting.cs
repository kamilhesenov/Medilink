using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
