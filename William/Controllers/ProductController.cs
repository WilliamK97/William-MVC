using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using William.ViewModels;

namespace William.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int id, string sort)
        {
            var model = new ProductIndexViewModel();
            
            using (var db = new WilliamDB())
            {
                model.CategoryName = string.Join("", db.Categories.Where(x => x.CategoryId == id).Select(x => x.Name));
                model.CategoryId = id;
                model.ProductList.AddRange(db.Products
                    .Select(x => new ProductIndexViewModel.ProductListViewModel
                    {
                        ProductId = x.ProductId,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        CategoryId = x.CategoryId
                    }).Where(x => x.CategoryId == id));

                model = Sort(model, sort);

                return View(model);
            }
        }

        
        [HttpGet]
        public ActionResult Search(string search, string sort)
        {
            var model = new ProductIndexViewModel();
            model.Search = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                using (var db = new WilliamDB())
                {
                    model.ProductList.AddRange(db.Products
                        .Select(x => new ProductIndexViewModel.ProductListViewModel
                        {
                            ProductId = x.ProductId,
                            Name = x.Name,
                            Description = x.Description,
                            Price = x.Price,
                            CategoryId = x.CategoryId
                        }));

                    model.ProductList = model.ProductList.Where(x => x.Name.ToUpper().Contains(search.ToUpper()) ||
                        x.Description.ToUpper().Contains(search.ToUpper())).ToList();

                    model = Sort(model, sort);

                    return View("Search", model);
                }
            }

            return View("Search", model);
        }
        

        
        
        [HttpGet]
        [Authorize(Roles = "Admin, ProductManager")]
        public ActionResult Edit(int? id)
        {
            using (var db = new WilliamDB())
            {
                var p = db.Products.Find(id);
                var model = new ProductEditViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Photo = p.Photo,
                    CategoryId = p.CategoryId
                };
                CategoriesDropDown(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProductManager")]
        public ActionResult Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { id = model.CategoryId });
            }

            using (var db = new WilliamDB())
            {
                var p = db.Products.FirstOrDefault(x => x.ProductId == model.ProductId);

                p.ProductId = model.ProductId;
                p.Name = model.Name;
                p.Description = model.Description;
                p.Price = model.Price;
                p.Photo = model.Photo;
                p.CategoryId = model.CategoryId;

                db.SaveChanges();
                return RedirectToAction("Index", new { id = p.CategoryId });
            }
        }
        

        
        
        [Authorize(Roles = "Admin, ProductManager")]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            var model = new ProductEditViewModel { CategoryId = (int)id };
            using (var db = new WilliamDB())
            {
                model.CategoryName = string.Join("" ,db.Categories.Where(x => x.CategoryId == id).Select(x => x.Name));
                model.CategoryId = (int)id;
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, ProductManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new WilliamDB())
            {
                var p = new Models.Product
                {
                    ProductId = model.ProductId,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Photo = model.Photo,
                    CategoryId = model.CategoryId
                };

                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index", "Product", new { id = model.CategoryId });
            }
        }
        

        
        [Authorize(Roles = "Admin, ProductManager")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new WilliamDB())
            {
                var p = db.Products.Find(id);
                var model = new ProductEditViewModel
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Photo = p.Photo,
                    CategoryId = p.CategoryId
                };
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, ProductManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var db = new WilliamDB())
            {
                var obj = db.Products.Find(id);
                if (obj != null)
                {
                    db.Products.Remove(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product", new { id = obj.CategoryId });
                }
                return RedirectToAction("Index", "Home");
            }
        }
        

        //sort
        public ProductIndexViewModel Sort(ProductIndexViewModel model, string sort)
        {
            if (sort == "nameAsc" || sort == "nameDesc")
            {
                model.SortOrderName = sort == "nameDesc" ? "nameAsc" : "nameDesc";
                switch (model.SortOrderName)
                {
                    case "nameDesc":
                        model.ProductList = model.ProductList.OrderBy(x => x.Name).ToList();
                        break;

                    default:
                        model.ProductList = model.ProductList.OrderByDescending(x => x.Name).ToList();
                        break;
                }
                return model;
            }
            else
            {
                model.SortOrderPrice = sort == "priceDesc" ? "priceAsc" : "priceDesc";
                switch (model.SortOrderPrice)
                {
                    case "priceDesc":
                        model.ProductList = model.ProductList.OrderBy(x => x.Price).ToList();
                        break;

                    default:
                        model.ProductList = model.ProductList.OrderByDescending(x => x.Price).ToList();
                        break;
                }
                return model;
            }
        }

        
        public void CategoriesDropDown(ProductEditViewModel model)
        {
            model.CategoryDropDownList = new List<SelectListItem>();
            using (var db = new WilliamDB())
            {
                foreach (var cat in db.Categories)
                {
                    model.CategoryDropDownList.Add(new SelectListItem { Value = cat.CategoryId.ToString(), Text = cat.Name });
                }
            }
        }
    }
}