using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeedTN.Controllers
{
    [Authorize]
    public class UserMenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserMenuItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: UserMenuItems
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserMenuItems/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserMenuItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int id)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var userOpenOrder = _context.Order.Where(o => o.UserId == user.Id).FirstOrDefault(o => o.DateCompleted == null);
                if (userOpenOrder != null)
                {
                    var newUserMenuItem = new UserMenuItem
                    {
                        UserId = userOpenOrder.UserId,
                        MenuItemId = id
                    };

                    userOpenOrder.UserMenuItems = new List<UserMenuItem>();

                    userOpenOrder.UserMenuItems.Add(newUserMenuItem);

                    _context.UserMenuItem.Add(newUserMenuItem);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "MenuItems");

                } else
                {
                    var newOrder = new Order
                    {
                        UserId = user.Id,
                        DateCreated = DateTime.Now,
                        UserMenuItems = new List<UserMenuItem>()
                    };

                    var newUserMenuItem = new UserMenuItem
                    {
                        UserId = user.Id,
                        MenuItemId = id
                    };

                    newOrder.UserMenuItems.Add(newUserMenuItem);

                    _context.Order.Add(newOrder);
                    _context.UserMenuItem.Add(newUserMenuItem);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "MenuItems");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: UserMenuItems/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserMenuItems/Edit/5
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

        // GET: UserMenuItems/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserMenuItems/Delete/5
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