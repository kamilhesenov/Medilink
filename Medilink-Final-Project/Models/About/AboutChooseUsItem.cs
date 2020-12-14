using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.About
{
    public class AboutChooseUsItem
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required, MaxLength(550)]
        public string Content { get; set; }
    }
}
