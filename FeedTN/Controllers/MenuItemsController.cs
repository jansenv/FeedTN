using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using FeedTN.Models.MenuItemViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> Index(string filter)
        {
            var menuItems = await _context.MenuItem
                .ToListAsync();
            
            switch (filter)
            {
                case "Gluten Free":
                    menuItems = await _context.MenuItem
                        .Where(mi => mi.GlutenFree == true)
                        .ToListAsync();
                    break;

                case "Vegetarian":
                    menuItems = await _context.MenuItem
                        .Where(mi => mi.Vegetarian == true)
                        .ToListAsync();
                    break;

                case "Vegan":
                    menuItems = await _context.MenuItem
                        .Where(mi => mi.Vegan == true)
                        .ToListAsync();
                    break;

                default:
                    menuItems = await _context.MenuItem
                        .ToListAsync();
                    break;
            }

            return View(menuItems);
        }

        // GET: MenuItems/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var menuItem = await _context.MenuItem
                .FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            var viewModel = new MenuItemViewModel()
            {
                MenuItem = new MenuItem()
            };

            viewModel.MenuItem.MenuItemId = menuItem.MenuItemId;
            viewModel.MenuItem.Title = menuItem.Title;
            viewModel.MenuItem.Description = menuItem.Description;
            viewModel.MenuItem.ImagePath = menuItem.ImagePath;
            viewModel.MenuItem.Active = menuItem.Active;
            viewModel.MenuItem.GlutenFree = menuItem.GlutenFree;
            viewModel.MenuItem.Vegetarian = menuItem.Vegetarian;
            viewModel.MenuItem.Vegan = menuItem.Vegan;

            if (_signInManager.IsSignedIn(User))
            {
                var user = await GetCurrentUserAsync();

                var userPreference = await _context.Favorite
                    .Where(p => p.MenuItemId == id)
                    .Where(p => p.UserId == user.Id)
                    .FirstOrDefaultAsync();

                if (userPreference == null)
                {
                    viewModel.UserHasFavorited = false;
                } else
                {
                    viewModel.UserHasFavorited = true;
                }

                return View(viewModel);
            }

            return View(viewModel);
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
        public async Task<ActionResult> Edit(int id)
        {
            var menuItem = await _context.MenuItem.FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, MenuItem menuItem)
        {
            try
            {
                var menuItemItem = new MenuItem()
                {
                    MenuItemId = menuItem.MenuItemId,
                    Title = menuItem.Title,
                    Description = menuItem.Description,
                    ImagePath = menuItem.ImagePath,
                    FavoriteCount = menuItem.FavoriteCount,
                    Active = menuItem.Active,
                    GlutenFree = menuItem.GlutenFree,
                    Vegetarian = menuItem.Vegetarian,
                    Vegan = menuItem.Vegan
                };

                _context.MenuItem.Update(menuItemItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}