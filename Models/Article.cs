using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lista10.Models
{
    public class Article
    {
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        public string Name { set; get; }
        [Required]
        public double Price { set; get; }
        [MaxLength(100)]
        public string Image { get; set; }
        [Display(Name = "Category")]
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
