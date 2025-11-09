using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Details(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            var useBase = await _context.Users.FindAsync(user.Id);
            if (useBase == null)
                return NotFound();

            useBase.FirstName = user.FirstName;
            useBase.LastName = user.LastName;

            _context.Users.Update(useBase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userBase = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userBase == null)
                return NotFound();

            _context.Users.Remove(userBase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}