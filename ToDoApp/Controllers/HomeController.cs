using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoListContext _context;

        public HomeController(TodoListContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.TodoLists;
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoList todoList)
        {
            _context.TodoLists.Add(todoList);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.TodoLists == null)
            {
                return NotFound();
            }
            var item = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
            if(item == null)
            {
                return NoContent();
            }

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || _context.TodoLists == null)
            {
                return NoContent();
            }

            var item = await _context.TodoLists.FindAsync(id);
            if(item == null)
            {
                return NoContent();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return NoContent();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckList(todoList.Id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(todoList);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TodoLists == null)
            {
                return NoContent();
            }

            var item = await _context.TodoLists.FindAsync(id);
            if (item == null)
            {
                return NoContent();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return NoContent();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Remove(todoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckList(todoList.Id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(todoList);
        }

        public bool CheckList(int id)
        {
            return _context.TodoLists.Any(c => c.Id == id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}