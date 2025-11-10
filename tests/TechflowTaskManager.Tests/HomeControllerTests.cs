using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using techflow_task_manager.Constants;
using techflow_task_manager.Controllers;
using techflow_task_manager.Extensions;
using techflow_task_manager.Models;
using Xunit;

namespace TechflowTaskManager.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Create_Get_WhenNotLoggedIn_RedirectsToLogin()
    {
        var session = new TestSession();
        var controller = CreateControllerWithSession(session);

        var result = controller.Create();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirect.ActionName);
        Assert.Equal("Account", redirect.ControllerName);
    }

    [Fact]
    public void Create_Get_WhenLoggedIn_ReturnsViewWithEmptyTask()
    {
        var session = new TestSession();
        session.SetString(SessionConstants.LoggedIn, bool.TrueString);
        var controller = CreateControllerWithSession(session);

        var result = controller.Create();

        var view = Assert.IsType<ViewResult>(result);
        Assert.IsType<TaskItem>(view.Model);
    }

    [Fact]
    public void Create_Post_WithInvalidModel_ReturnsViewWithSameModel()
    {
        var session = new TestSession();
        session.SetString(SessionConstants.LoggedIn, bool.TrueString);
        var controller = CreateControllerWithSession(session);
        controller.ModelState.AddModelError("Title", "Required");
        var task = new TaskItem { Title = string.Empty };

        var result = controller.Create(task);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(task, view.Model);
    }

    [Fact]
    public void Create_Post_WithValidModel_PersistsTaskAndRedirects()
    {
        var session = new TestSession();
        session.SetString(SessionConstants.LoggedIn, bool.TrueString);
        session.SetObject(SessionConstants.Tasks, new List<TaskItem>
        {
            new() { Id = 1, Title = "Existing" }
        });
        var controller = CreateControllerWithSession(session);
        var task = new TaskItem
        {
            Title = "New Task",
            Description = "Details"
        };

        var result = controller.Create(task);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(HomeController.Index), redirect.ActionName);

        var tasks = session.GetObject<List<TaskItem>>(SessionConstants.Tasks);
        Assert.NotNull(tasks);
        Assert.Equal(2, tasks!.Count);
        var newTask = tasks!.Last();
        Assert.Equal(2, newTask.Id);
        Assert.Equal("New Task", newTask.Title);
        Assert.Equal("Details", newTask.Description);
    }

    private static HomeController CreateControllerWithSession(TestSession session)
    {
        var logger = NullLogger<HomeController>.Instance;
        var controller = new HomeController(logger);
        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<ISessionFeature>(new TestSessionFeature { Session = session });
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        return controller;
    }

    private sealed class TestSessionFeature : ISessionFeature
    {
        public ISession? Session { get; set; }
    }

    private sealed class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;
        public string Id { get; } = System.Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _store.Keys;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Clear() => _store.Clear();
        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
    }
}
