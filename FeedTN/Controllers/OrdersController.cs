using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using FeedTN.Models.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedTN.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var order = await _context.Order
                .Where(o => o.UserId == user.Id)
                .Where(o => o.DateCompleted == null)
                .Include(o => o.UserMenuItems)
                    .ThenInclude(o => o.MenuItem)
                .FirstOrDefaultAsync();

            if (order == null || order.UserMenuItems.Count < 1)
            {
                return RedirectToAction(nameof(CartEmpty));
            } 

            else

            {
                var viewModel = new OrderDetailViewModel();

                viewModel.Order = order;

                var lineItems = order.UserMenuItems.Select(umi => new OrderLineItem()
                {
                    MenuItem = umi.MenuItem
                });

                viewModel.LineItems = lineItems;

                return View(viewModel);
            }
        }

        public ActionResult CartEmpty()
        {
            return View();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderDetailViewModel viewModel)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var dataModel = await _context.Order.FirstOrDefaultAsync(o => o.OrderId == viewModel.Order.OrderId);
                dataModel.DateCreated = viewModel.Order.DateCreated;
                dataModel.UserId = user.Id;
                dataModel.DateCompleted = DateTime.Now;

                _context.Order.Update(dataModel);
                await _context.SaveChangesAsync();

                TempData["orderConfirmed"] = "Your order has been processed.";

                return RedirectToAction("SendSms", "Sms");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var order = await _context.Order
                    .Where(o => o.UserId == user.Id).FirstOrDefaultAsync(o => o.DateCompleted == null);

                var userMenuItems = await _context.UserMenuItem.Where(mi => mi.OrderId == order.OrderId).ToListAsync();
                foreach (var mi in userMenuItems)
                {
                    _context.UserMenuItem.Remove(mi);
                }

                _context.Order.Remove(order);
                await _context.SaveChangesAsync();

                TempData["cancelledOrder"] = "Order cancelled";

                return RedirectToAction("Index", "Orders");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}