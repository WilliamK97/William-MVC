using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace William.ViewModels
{
    
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Price field is required.")]
        [DataType(DataType.Currency, ErrorMessage = "You have to enter a number.")]
        [Range(1, 100000, ErrorMessage = "The Price field must be between 1 and 100.000")]
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public List<SelectListItem> CategoryDropDownList { get; set; }
    }
}