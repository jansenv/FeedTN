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
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            var reports = await _context.Report
                .Include(r => r.ApplicationUser)
                .ToListAsync();

            return View(reports);
        }

        public async Task<ActionResult> IndexOfCurrentUserIssues()
        {
            var user = await GetCurrentUserAsync();

            var reports = await _context.Report
                .Include(r => r.ApplicationUser)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            return View(reports);
        }

        // GET: Reports/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var menuItem = await _context.Report
                .FirstOrDefaultAsync(r => r.ReportId == id);

            return View(menuItem);
        }

        // GET: Reports/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Report report)
        {
            try
            {
                var user = await GetCurrentUserAsync();

                var reportItem = new Report
                {
                    Description = report.Description,
                    Active = true,
                    UserId = user.Id,
                    Comments = report.Comments,
                    ApplicationUser = user
                };

                _context.Report.Add(reportItem);
                await _context.SaveChangesAsync();

                TempData["reportConfirmation"] = "Report sent";

                return RedirectToAction("Index", "Orders");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var report = await _context.Report.FirstOrDefaultAsync(r => r.ReportId == id);

            return View(report);
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int id, Report report)
        {
            try
            {
                var reportItem = new Report()
                {
                    ReportId = report.ReportId,
                    Description = report.Description,
                    Active = report.Active,
                    Comments = report.Comments,
                    UserId = report.UserId,
                };

                _context.Report.Update(reportItem);
                await _context.SaveChangesAsync();

                TempData["commentConfirmed"] = "Updated";

                return RedirectToAction("Edit", "Reports", new { id = report.ReportId });
            }
            catch
            {
                return View();
            }
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Resolve(int id, Report report)
        {
            try
            {
                var reportItem = new Report()
                {
                    ReportId = report.ReportId,
                    Description = report.Description,
                    Active = false,
                    Comments = report.Comments,
                    UserId = report.UserId,
                };

                _context.Report.Update(reportItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
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
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}