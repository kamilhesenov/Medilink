using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class SocialLink
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Facebook { get; set; }

        [Required, MaxLength(100)]
        public string Twitter { get; set; }

        [Required, MaxLength(100)]
        public string Linkedin { get; set; }

        [Required, MaxLength(100)]
        public string Yotube { get; set; }
    }
}
