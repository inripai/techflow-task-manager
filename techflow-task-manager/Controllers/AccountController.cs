using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using techflow_task_manager.Constants;
using techflow_task_manager.Extensions;
using techflow_task_manager.Models;

namespace techflow_task_manager.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString(SessionConstants.LoggedIn) == bool.TrueString)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            HttpContext.Session.SetString(SessionConstants.LoggedIn, bool.TrueString);
            HttpContext.Session.SetString(SessionConstants.Username, username ?? string.Empty);
            HttpContext.Session.SetObject(SessionConstants.Tasks, GetDefaultTasks());
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        private static List<TaskItem> GetDefaultTasks()
        {
            return new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Revisar emails", Description = "Verificar novas mensagens na caixa de entrada." },
                new TaskItem { Id = 2, Title = "Planejar reunião", Description = "Preparar pauta para a reunião semanal." },
                new TaskItem { Id = 3, Title = "Atualizar Kanban", Description = "Mover tarefas concluídas para a coluna final." },
                new TaskItem { Id = 4, Title = "Documentar processo", Description = "Escrever detalhes do fluxo de trabalho atual." },
                new TaskItem { Id = 5, Title = "Estudar nova ferramenta", Description = "Explorar funcionalidades do novo software." }
            };
        }
    }
}
