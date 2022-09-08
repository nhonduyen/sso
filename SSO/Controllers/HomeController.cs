using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using SSO.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public HomeController(ILogger<HomeController> logger,
            GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["Name"] = user.DisplayName;
            ViewData["Id"] = user.Id;
            _logger.LogInformation($"Login user: {user.DisplayName} - User Id: {user.Id} - Email {user.Mail}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
