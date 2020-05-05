using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using FeedTN.Models.MenuItemViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeedTN.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuItemsController(ApplicationDbContext ctx, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = ctx;
        }

        // GET: MenuItems
        public ActionResult Index()
        {
            return View();
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuItemViewModel menuItemViewModel)
        {
            try
            {
                var menuItem = new MenuItem
                {
                    Title = menuItemViewModel.MenuItem.Title,
                    Description = menuItemViewModel.MenuItem.Description,
                    FavoriteCount = menuItemViewModel.MenuItem.FavoriteCount,
                    Active = menuItemViewModel.MenuItem.Active,
                    GlutenFree = menuItemViewModel.MenuItem.GlutenFree,
                    Vegetarian = menuItemViewModel.MenuItem.Vegetarian,
                    Vegan = menuItemViewModel.MenuItem.Vegan
                };

                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                if (menuItemViewModel.ImageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + menuItemViewModel.ImageFile.FileName;
                    menuItem.ImagePath = fileName;
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await menuItemViewModel.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.MenuItem.Add(menuItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuItems/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MenuItems/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MenuItems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}