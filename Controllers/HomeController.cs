using LiquidationDashboard.Helpers;
using LiquidationDashboard.Models;
using LiquidationDashboard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiquidationDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IApiService apiService)
        {
            _logger = logger;
            _userService = userService;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            //var aa = await _apiService.GetStorages("vALGO");
            var aa = await _apiService.GetActiveSymbols();
            return Json(aa);
        }

        public async Task<IActionResult> Login(string name, string password)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction(nameof(Dashboard));
            //}

            var user = await _userService.GetUser(name);

            if (name == null || password == null)
            {
                return Json(new { Success = false, Message = "Please enter username and password." });
            }

            if (user == null)
            {
                user = new User()
                {
                    Name = name,
                    Password = password.Sha256(),
                };
                await _userService.Add(user);
            }
            else if (user.Password != password.Sha256())
            {
                return Json(new { Success = false, Message = "incorrect password" });
            }

            var identity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, user.Name),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            });

            return Json(new { Success = true, Message = "Login Successful" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index));
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
