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
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult> Index()
        {
            var menuItems = await _context.MenuItem
                .ToListAsync();

            return View(menuItems);
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
        public async Task<ActionResult> SetInactive(int id)
        {
            var menuItem = await _context.MenuItem.FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetInactive(int id, MenuItem menuItem)
        {
            try
            {
                var foundMenuItem = await _context.MenuItem.FirstOrDefaultAsync(mi => mi.MenuItemId == id);

                var menuItemItem = new MenuItem()
                {
                    MenuItemId = id,
                    Title = foundMenuItem.Title,
                    Description = foundMenuItem.Description,
                    ImagePath = foundMenuItem.ImagePath,
                    FavoriteCount = foundMenuItem.FavoriteCount,
                    Active = false,
                    GlutenFree = foundMenuItem.GlutenFree,
                    Vegetarian = foundMenuItem.Vegetarian,
                    Vegan = foundMenuItem.Vegan
                };

                _context.MenuItem.Update(menuItemItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}