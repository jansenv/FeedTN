using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedTN.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Favorites
        public ActionResult Index()
        {
            return View();
        }

        // GET: Favorites/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Favorites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Favorites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Favorite favorite)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                var likedMenuItem = await _context.MenuItem
                    .Where(mi => mi.MenuItemId == favorite.MenuItem.MenuItemId)
                    .FirstOrDefaultAsync();

                likedMenuItem.FavoriteCount++;

                _context.MenuItem.Update(likedMenuItem);
                await _context.SaveChangesAsync();

                var favoriteInstance = new Favorite
                {
                    MenuItemId = favorite.MenuItem.MenuItemId
                };

                favoriteInstance.UserId = user.Id;

                _context.Favorite.Add(favoriteInstance);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "MenuItems", new { id = favorite.MenuItem.MenuItemId });
            }
            catch
            {
                return View();
            }
        }

        // GET: Favorites/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Favorites/Edit/5
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

        // GET: Favorites/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Favorites/Delete/5
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

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}