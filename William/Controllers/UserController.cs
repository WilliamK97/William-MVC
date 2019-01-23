﻿using William.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using System.Web;
using William.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace William.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        
        ApplicationDbContext context;
        
        public UserController()
        {
            context = new ApplicationDbContext();
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            
            var model = new UserIndexViewModel();

            using (var db = new ApplicationDbContext())
            {
                model.UserList.AddRange(db.Users.Select(x => new UserIndexViewModel.UserListViewModel
                {
                    UserId = x.Id,
                    Email = x.Email,
                    UserName = x.UserName
                }));
                foreach (var item in model.UserList)
                {
                    item.UserRoles = UserManager.GetRoles(item.UserId).SingleOrDefault();
                }
                return View(model);
            }
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var model = new UserEditViewModel();
            var user = UserManager.FindById(id);
            using (var db = new ApplicationDbContext())
            {
                
                model.UserId = user.Id;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.UserRoles = UserManager.GetRoles(user.Id).SingleOrDefault();
                model.UserDropDownList = new List<SelectListItem>();

                foreach (var item in db.Roles)
                {
                    model.UserDropDownList.Add(new SelectListItem { Value = item.Name, Text = item.Name });
                }

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new ApplicationDbContext())
            {
                var user = UserManager.FindById(model.UserId);
                user.Id = model.UserId;
                user.Email = model.Email;
                user.UserName = model.UserName;

                UserManager.RemoveFromRole(user.Id, model.UserRoles);
                UserManager.AddToRole(user.Id, model.UserDropDownHolder);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            var model = new UserEditViewModel();
            using (var db = new ApplicationDbContext())
            {
                var user = UserManager.FindById(id);
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.UserRoles = UserManager.GetRoles(user.Id).SingleOrDefault();

                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = UserManager.FindById(id);
            var rolesForUser = UserManager.GetRoles(id);

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    var result = UserManager.RemoveFromRoleAsync(user.Id, item);
                }
            }

            UserManager.Delete(user);
            

            return RedirectToAction("Index");
        }
        
    }
}