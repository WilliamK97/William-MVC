
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using William.ViewModels;

namespace William.Controllers
{
    public class HomeController : Controller
    {
        

        // GET: Home
        public ActionResult Index()
        {
            var model = new CategoryIndexViewModel();
            using (var db = new WilliamDB())
            {
                model.CategoriesList.AddRange(db.Categories.Select(x => new CategoryIndexViewModel.CategoryListViewModel
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name
                }));

                return View(model);
            }
        }

        
        
        [HttpGet]
        [Authorize(Roles = "Admin, ProductManager")]
        public ActionResult Edit(int? id)
        {
            using (var db = new WilliamDB())
            {
                var cat = db.Categories.Find(id);
                var model = new CategoryEditViewModel
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
                };
                return View(model);
            }
        }

        [Authorize(Roles = "Admin, ProductManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new WilliamDB())
            {
                var cat = db.Categories.FirstOrDefault(x => x.CategoryId == model.CategoryId);

                cat.CategoryId = model.CategoryId;
                cat.Name = model.Name;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        


        
        [Authorize(Roles = "Admin, ProductManager")]
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CategoryEditViewModel());
        }

        [Authorize(Roles = "Admin, ProductManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new WilliamDB())
            {
                var cat = new Models.Category
                {
                    Name = model.Name
                };

                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        


        
        [Authorize(Roles = "Admin, ProductManager")]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var db = new WilliamDB())
            {
                var cat = db.Categories.Find(id);
                var model = new CategoryEditViewModel
                {
                    CategoryId = cat.CategoryId,
                    Name = cat.Name
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
                return RedirectToAction("Index");
            }
            using (var db = new WilliamDB())
            {
                var obj = db.Categories.Find(id);
                if (obj != null)
                {
                    db.Categories.Remove(obj);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
        
    }
}