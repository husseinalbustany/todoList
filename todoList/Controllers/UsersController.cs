using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using todoList.Data;
using todoList.Models;

namespace todoList.Controllers
{
    public class UsersController : Controller
    {
        private readonly todoListContext _context;

        public UsersController(todoListContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");//, new { @isAdmin = isAdmin.Count });

            return RedirectToAction("Login", "Users");//View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");//, new { @isAdmin = isAdmin.Count });

            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return RedirectToAction("Login", "Users");//View(users);
        }




        // GET: Users/Login
        public IActionResult Login()
        {
            GlobalVariables.LoggedIn = false;
            GlobalVariables.isAdmin = 0;
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password)
        {


            //HttpContext.Session.Set
            //  _context.Add(users);
            var log = _context.Users.ToList();
            // await _context.SaveChangesAsync();

            var log2 = (from c in log
                        where c.Name == userName && c.Password == password
                        orderby c.Name
                        select c).ToList();




            if (log2.Count > 0)
            //return RedirectToAction(nameof(Index));
            {
                var isAdmin = (from c in log2
                               where c.isAdmin == true
                               select c).ToList();

                // if (isAdmin.Count > 0)
                //    User.IsInRole("Admin");
                // HttpContext.Session.Set("LoggedInRole", "Admin");
                // else
                //    HttpContext.Session.Set("LoggedInRole", "Admin");
                GlobalVariables.isAdmin = isAdmin.Count;
                GlobalVariables.LoggedIn = true;
                return RedirectToAction("Index", "todo", new { @isAdmin = GlobalVariables.isAdmin });
            }
            else
                return RedirectToAction("Login", "Users");//, new { @isAdmin = isAdmin.Count });

        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,isAdmin")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
