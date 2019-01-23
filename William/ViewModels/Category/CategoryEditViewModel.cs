using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace William.ViewModels
{
    
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
    }
}