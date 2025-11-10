using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using techflow_task_manager.Constants;
using techflow_task_manager.Extensions;
using techflow_task_manager.Models;

namespace techflow_task_manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = GetTasks();
            ViewData["Username"] = HttpContext.Session.GetString(SessionConstants.Username);
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View(new TaskItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskItem task)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var tasks = GetTasks();
            var nextId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
            task.Id = nextId;
            tasks.Add(task);
            SaveTasks(tasks);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskItem task)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var tasks = GetTasks();
            var existing = tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existing == null)
            {
                return RedirectToAction(nameof(Index));
            }

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.IsCompleted = task.IsCompleted;
            SaveTasks(tasks);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
                SaveTasks(tasks);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString(SessionConstants.LoggedIn) == bool.TrueString;
        }

        private List<TaskItem> GetTasks()
        {
            var tasks = HttpContext.Session.GetObject<List<TaskItem>>(SessionConstants.Tasks);
            if (tasks == null)
            {
                tasks = new List<TaskItem>();
                SaveTasks(tasks);
            }

            return tasks;
        }

        private void SaveTasks(List<TaskItem> tasks)
        {
            HttpContext.Session.SetObject(SessionConstants.Tasks, tasks);
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
