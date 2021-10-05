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
    public class todoController : Controller
    {
        private readonly todoListContext _context;

        public todoController(todoListContext context)
        {
            _context = context;
        }

        // GET: todo
        public async Task<IActionResult> Index(int? isAdmin)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");

            ViewData["isAdmin"] = GlobalVariables.isAdmin;
            return View(await _context.todo.ToListAsync());
        }


        public async Task<IActionResult> DoneConfirmed(int id)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");

            var tODO = await _context.todo.FindAsync(id);
            tODO.Done = true;
            tODO.DoneDate = DateTime.Now;
            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "todo", new { @isAdmin = GlobalVariables.isAdmin });
        }

        // GET: todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: todo/Create
        public IActionResult Create()
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");
            return View();
        }

        // POST: todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DueDate,Done")] todo todo)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");

            if (ModelState.IsValid)
            {
                if (todo.DueDate < DateTime.Now.Date)
                {
                    return View(todo);
                }


                _context.Add(todo);
                await _context.SaveChangesAsync();
                //  return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "todo", new { @isAdmin = GlobalVariables.isAdmin });
            }
            return View(todo);
        }

        // GET: todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DueDate,Done")] todo todo)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!todoExists(todo.Id))
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
            return View(todo);
        }

        // GET: todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!GlobalVariables.LoggedIn)
                return RedirectToAction("Login", "Users");
            var todo = await _context.todo.FindAsync(id);
            _context.todo.Remove(todo);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "todo", new { @isAdmin = GlobalVariables.isAdmin });
        }

        private bool todoExists(int id)
        {
            return _context.todo.Any(e => e.Id == id);
        }
    }
}
