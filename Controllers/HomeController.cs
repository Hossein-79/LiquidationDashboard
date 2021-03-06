using LiquidationDashboard.Helpers;
using LiquidationDashboard.Models;
using LiquidationDashboard.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LiquidationDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IApiService _apiService;
        private readonly ISymbolService _symbolService;
        private readonly IStorageService _storageService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IApiService apiService, ISymbolService symbolService, IStorageService storageService)
        {
            _logger = logger;
            _userService = userService;
            _apiService = apiService;
            _symbolService = symbolService;
            _storageService = storageService;
        }

        public async Task<IActionResult> Index()
        {
            var storages = await _storageService.GetStorages();

            return View(storages.OrderByDescending(u => u.Health));
        }

        [Authorize]
        public async Task<IActionResult> AddAlert(string address)
        {
            var storage = await _storageService.GetStorage(address);

            if (storage is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(storage);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAlert(string address, decimal alertLimit, string email)
        {
            var storage = await _storageService.GetStorage(address);

            if (storage is null)
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userService.GetUser(User.Identity.Name);

            var alert = new Alert
            {
                UserId = user.UserId,
                Email = email,
                Address = storage.Address,
                AlertLimit = alertLimit,
                IsSend = false
            };
            await _userService.AddAlert(alert);
                
            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var user = await _userService.GetUser(User.Identity.Name);
            await _userService.DeleteAlert(id, user.UserId);
            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userService.GetUser(User.Identity.Name);
            var alerts = await _userService.GetUserAlerts(user.UserId);
            return View(alerts);
        }

        public async Task<IActionResult> Search(string id)
        {
            var searched = await _apiService.SearchAddress(id);
            if (searched is not null)
            {
                await _storageService.Add(searched);
            }
            var addresses = await _storageService.Search(id);
            return View(addresses);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUser(name);

            if (name == null || password == null)
            {
                ViewBag.Error = "Please enter username and password.";
                return View();
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
                ViewBag.Error = "incorrect password";
                return View();
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

            return RedirectToAction(nameof(Index));
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
